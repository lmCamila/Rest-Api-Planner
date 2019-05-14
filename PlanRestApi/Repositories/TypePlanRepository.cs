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
    public class TypePlanRepository : AbstractRepository<TypePlan>
    {
        public TypePlanRepository(IConfiguration configuration): base(configuration) { }
        public override bool Delete(int id)
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                if (db.State == ConnectionState.Closed)
                {
                    db.Open();
                }
                int result = db.Execute("DELETE FROM plan_types WHERE id = @Id", new { Id = id }, commandType: CommandType.Text);
                return (result > 0);
            }
        }

        public override TypePlan Get(int id)
        {

            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                if (db.State == ConnectionState.Closed)
                {
                    db.Open();
                }
                var type = db.QueryFirst<TypePlan>("SELECT * FROM plan_types WHERE id = @Id", new { Id = id });
                return type;
            }
        }
        public override TypePlan GetLastInserted()
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                if (db.State == ConnectionState.Closed)
                {
                    db.Open();
                }
                var type = db.QueryFirst<TypePlan>("SELECT * FROM plan_types WHERE id = IDENT_CURRENT('plan_types')");
                return type;
            }
        }

        public override IEnumerable<TypePlan> GetAll()
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                if (db.State == ConnectionState.Closed)
                {
                    db.Open();
                }
                IEnumerable<TypePlan> types = db.Query<TypePlan>("SELECT * FROM plan_types");
                return (List<TypePlan>)types;
            }
        }

        public override bool Insert(TypePlan item)
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                if (db.State == ConnectionState.Closed)
                {
                    db.Open();
                }
                int result = db.Execute(@"INSERT INTO plan_types(name)
                            VALUES(@NAME)", item);
                return (result > 0);
            }
        }

        public override bool Update(TypePlan item)
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                if (db.State == ConnectionState.Closed)
                {
                    db.Open();
                }
                int result = db.Execute(@"UPDATE  plan_types 
                           SET name = @Name
                           WHERE id = @Id", item);
                return (result > 0);
            }
        }
    }
}
