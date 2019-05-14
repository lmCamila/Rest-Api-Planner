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
    public class PlanStatusRepository : AbstractRepository<PlanStatus>
    {
        public PlanStatusRepository(IConfiguration configuration) : base(configuration) { }
        public override bool Delete(int id)
        {
            using(IDbConnection db = new SqlConnection(ConnectionString))
            {
                if(db.State == ConnectionState.Closed)
                {
                    db.Open();
                }
                int result = db.Execute("DELETE FROM plan_status WHERE id = @Id", new { Id = id });
                return (result > 0);
            }
        }

        public override PlanStatus Get(int id)
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                if (db.State == ConnectionState.Closed)
                {
                    db.Open();
                }
                PlanStatus status = db.QueryFirst<PlanStatus>(@"SELECT * 
                                                                FROM plan_status 
                                                                WHERE id = @Id", new { Id = id });
                return status;
            }
        }

        public override IEnumerable<PlanStatus> GetAll()
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                if (db.State == ConnectionState.Closed)
                {
                    db.Open();
                }
                IEnumerable<PlanStatus> listStatus = db.Query<PlanStatus>("SELECT * FROM plan_status");
                return (List<PlanStatus>)listStatus;
            }
        }

        public override PlanStatus GetLastInserted()
        {
            using(IDbConnection db = new SqlConnection(ConnectionString))
            {
                if(db.State == ConnectionState.Closed)
                {
                    db.Open();
                }
                var planStatus = db.QueryFirst<PlanStatus>(@"SELECT * FROM plan_status 
                                                             WHERE id = IDENT_CURRENT('plan_status')");
                return planStatus;
            }
        }

        public override bool Insert(PlanStatus item)
        {
            using(IDbConnection db = new SqlConnection(ConnectionString))
            {
                if(db.State == ConnectionState.Closed)
                {
                    db.Open();
                }
                int result = db.Execute(@"INSERT INTO plan_status(name)
                                          VALUES(@Name)", item);
                return (result > 0);
            }
        }

        public override bool Update(PlanStatus item)
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                if (db.State == ConnectionState.Closed)
                {
                    db.Open();
                }
                int result = db.Execute(@"UPDATE  plan_status 
                           SET name = @Name
                           WHERE id = @Id", item);
                return (result > 0);
            }
        }
    }
}
