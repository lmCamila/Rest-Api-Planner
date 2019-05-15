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
    public class UserHistoryRepository : AbstractRepository<UserHistory>
    {
        public UserHistoryRepository(IConfiguration configuration) : base(configuration) { }
        public override bool Delete(int id)
        {
            throw new Exception("Não é permitida alteração no histórico do usuário.");
        }

        public override UserHistory Get(int idUser)
        {
            throw new Exception("Não aplicavel a histórico de usuários.");            
        }

        public override IEnumerable<UserHistory> GetAll()
        {
            using(IDbConnection db = new SqlConnection(ConnectionString))
            {
                Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
                if (db.State == ConnectionState.Closed)
                {
                    db.Open();
                }
                var query = db.Query<UserHistory>(@"SELECT uh.*
                                                    FROM users_history uh 
                                                    INNER JOIN users u ON uh.id_user = u.id ").AsList();
                return (List<UserHistory>)query;
            }
        }

        public IEnumerable<UserHistory> GetAll(int idUser)
        {
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                if (db.State == ConnectionState.Closed)
                {
                    db.Open();
                }
                var query = db.Query<UserHistory>(@"SELECT uh.*
                            FROM users_history uh INNER JOIN users u ON uh.id_user = u.id 
                            WHERE uh.id_user = @Id", new { Id = idUser }).AsList();
                return (List<UserHistory>)query;
            }
        }

        public override UserHistory GetLastInserted()
        {
            throw new Exception("Não é aplicavel a histórico de usuário.");
        }

        public override bool Insert(UserHistory item)
        {
            throw new Exception("Não é permitida alteração no histórico do usuário.");
        }

        public override bool Update(UserHistory item)
        {
            throw new Exception("Não é permitida alteração no histórico do usuário.");
        }
    }
}
