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
    public class PlanHistoryRepository : AbstractRepository<PlanHistory>
    {
        public PlanHistoryRepository(IConfiguration configuration):base(configuration) { }
        public override bool Delete(int id)
        {
            throw new Exception("Não é permitida a alteração no histórico do Plano");
        }

        public override PlanHistory Get(int id)
        {
            throw new Exception("Não é aplicavel a histórico de planos");
        }

        public override IEnumerable<PlanHistory> GetAll()
        {
            using(IDbConnection db = new SqlConnection(ConnectionString))
            {
                Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
                if(db.State == ConnectionState.Closed)
                {
                    db.Open();
                }
                var query = db.Query<PlanHistory,PlanStatus,PlanHistory>(@"SELECT ph.*, ps.* 
                                                    FROM plans_history ph INNER JOIN plan_status ps
                                                    ON ph.id_plan_status = ps.id"
                ,(ph,ps) =>
                {
                    ph.Status = ps;
                    return ph;
                }, null, splitOn:"id,id").AsList();
                return (List<PlanHistory>)query;
            }
        }

        public IEnumerable<PlanHistory> GetAll(int idPlan)
        {
            using(IDbConnection db = new SqlConnection(ConnectionString))
            {
                Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
                if(db.State == ConnectionState.Closed)
                {
                    db.Open();
                }
                var query = db.Query<PlanHistory, PlanStatus, PlanHistory>(@"SELECT ph.*, ps.* 
                                                    FROM plans_history ph INNER JOIN plan_status ps
                                                    ON ph.id_plan_status = ps.id
                                                    WHERE id_plan = @Id"
                ,(ph, ps) => 
                {
                    ph.Status = ps;
                    return ph;
                }, new { Id = idPlan },splitOn:"id,id").AsList();
                return (List<PlanHistory>) query;
            }
        }
        public override PlanHistory GetLastInserted()
        {
            throw new Exception("Não é aplicavel a histórico de planos");
        }

        public override bool Insert(PlanHistory item)
        {
            throw new Exception("Não é permitida a alteração no histórico do Plano");
        }

        public override bool Update(PlanHistory item)
        {
            throw new Exception("Não é permitida a alteração no histórico do Plano");
        }
    }
}
