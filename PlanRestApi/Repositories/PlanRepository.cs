using Dapper;
using Microsoft.Extensions.Configuration;
using PlanRestApi.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace PlanRestApi.Repositories
{
    public class PlanRepository : AbstractRepository<Plan>
    {
        public PlanRepository(IConfiguration configuration) : base(configuration) { }
        public override bool Delete(int id)
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                if (db.State == ConnectionState.Closed)
                {
                    db.Open();
                }
                int result = db.Execute("UPDATE plans SET removed = 1 WHERE id = @Id", new { Id = id },
                                        commandType: CommandType.Text);
                return (result > 0);
            }
        }

        public override Plan Get(int id)
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
                if (db.State == ConnectionState.Closed)
                {
                    db.Open();
                }

                var query = db.Query<Plan, PlanStatus, TypePlan, User, Plan>(@"SELECT p.*, ps.*, pt.*, u.*
                                FROM plans p INNER JOIN plan_status ps ON p.id_status = ps.id 
                                INNER JOIN plan_types pt ON p.id_type = pt.id
                                INNER JOIN users u ON p.id_user = u.id
                                WHERE p.id = @Id AND p.removed = 0"
                , (p, ps, pt, u) =>
                {
                    p.Sponsor = u;
                    p.Status = ps;
                    p.Type = pt;
                    return p;
                }, new { Id = id }, splitOn: "id,id,id,id"
                ).AsList();
                var queryInterested = db.Query<User>(@"SELECT u.*
                                                    FROM users u INNER JOIN plan_interested_users piu 
                                                    ON u.id = piu.id_user
                                                    WHERE id_plan = @Id ", new { Id = id });
                query[0].Interested = (List<User>)queryInterested;
                return query[0];
            }
        }

        public override IEnumerable<Plan> GetAll()
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
                if (db.State == ConnectionState.Closed)
                {
                    db.Open();
                }

                var query = db.Query<Plan, PlanStatus, TypePlan, User, Plan>(@"SELECT p.*, ps.*, pt.*, u.*
                                FROM plans p INNER JOIN plan_status ps ON p.id_status = ps.id 
                                INNER JOIN plan_types pt ON p.id_type = pt.id
                                INNER JOIN users u ON p.id_user = u.id
                                WHERE p.removed = 0"
                , (p, ps, pt, u) =>
                {
                    p.Sponsor = u;
                    p.Status = ps;
                    p.Type = pt;
                    return p;
                }, null, splitOn: "id,id,id,id"
                ).AsList();
                foreach (var item in query)
                {
                    var queryInterested = db.Query<User>(@"SELECT u.*
                                                    FROM users u INNER JOIN plan_interested_users piu 
                                                    ON u.id = piu.id_user
                                                    WHERE id_plan = @Id ", new { Id = item.Id });
                    item.Interested = (List<User>)queryInterested;
                }
                return (List<Plan>)query;
            }
        }

        public override Plan GetLastInserted()
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
                if (db.State == ConnectionState.Closed)
                {
                    db.Open();
                }

                var query = db.Query<Plan, PlanStatus, TypePlan, User, Plan>(@"SELECT p.*, ps.*, pt.*, u.*
                                FROM plans p INNER JOIN plan_status ps ON p.id_status = ps.id 
                                INNER JOIN plan_types pt ON p.id_type = pt.id
                                INNER JOIN users u ON p.id_user = u.id
                                WHERE p.id = IDENT_CURRENT('plans')"
                , (p, ps, pt, u) =>
                {
                    p.Sponsor = u;
                    p.Status = ps;
                    p.Type = pt;
                    return p;
                }, null, splitOn: "id,id,id,id"
                ).AsList();
                foreach (var item in query)
                {
                    var queryInterested = db.Query<User>(@"SELECT u.*
                                                    FROM users u INNER JOIN plan_interested_users piu 
                                                    ON u.id = piu.id_user
                                                    WHERE id_plan = @Id ", new { Id = item.Id });
                    item.Interested = (List<User>)queryInterested;
                }
                return query[0];
            }
        }

        public override bool Insert(Plan item)
        {
            throw new Exception("Insira um tipo PlanUpload para esta função.");
        }

        public bool Insert(PlanUpload item)
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                if (db.State == ConnectionState.Closed)
                {
                    db.Open();
                }
                var results = db.Execute("sp_insert_plans", new
                {
                    Name = item.Name,
                    IdSponsor = item.Sponsor,
                    IdType = item.Type,
                    IdStatus = item.Status,
                    InterestedUser = item.Interested,
                    Description = item.Description,
                    Costs = item.Cost,
                    StartDate = validConvertDate(item.StartDate),
                    EndDate = validConvertDate(item.EndDate)
                }, commandType: CommandType.StoredProcedure);

                return (results > 0);
            }
        }

        public override bool Update(Plan item)
        {
            throw new Exception("Insira um tipo PlanUpload para essa função.");
        }

        public bool Update(PlanUpload item)
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                if (db.State == ConnectionState.Closed)
                {
                    db.Open();
                }
                DateTime? start = validConvertDate(item.StartDate);
                DateTime? end = validConvertDate(item.EndDate);

                string SQL = @"UPDATE plans SET
                                  name = @Name,
                                  id_user = @IdSponsor,
                                  id_type = @IdTypePlan,
                                  id_status = @IdStatus,
                                  description = @Description,
                                  cost = @Cost,
                                  start_date = @StartDate,
                                  end_date = @EndDate
                            WHERE id = @Id";

                int result = db.Execute(SQL, new
                {
                    Name = item.Name,
                    IdSponsor = item.Sponsor,
                    IdStatus = item.Status,
                    IdTypePlan = item.Type,
                    Description = item.Description,
                    Cost = item.Cost,
                    StartDate = start,
                    EndDate = end,
                    Id = item.Id
                });
                if (!String.IsNullOrEmpty(item.Interested))
                {
                    result += AlterInterested(item.Interested, item.Id);

                }
                return (result > 0);
            }
        }
        private int AlterInterested(string idsInterested, int id)
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                if (db.State == ConnectionState.Closed)
                {
                    db.Open();
                }
                var result = db.Execute("DELETE FROM plan_interested_users WHERE id_plan = @Id", new { Id = id },
                                       commandType: CommandType.Text);
                var ids = idsInterested.Split(',');
                foreach (var item in ids)
                {
                    result += db.Execute(@"INSERT INTO plan_interested_users (id_plan,id_user)
                                              VALUES(@IdPlan, @IdUser)", new { IdPlan = id, IdUser = item });
                }
                return result;
            }
        }
        private DateTime? validConvertDate(String date)
        {
            if (DateTime.Compare(Convert.ToDateTime(date), DateTime.MinValue) > 0 &&
                   DateTime.Compare(Convert.ToDateTime(date), DateTime.MaxValue) < 0)
            {
                return Convert.ToDateTime(date);
            }
            return null;
        }
    }
}
