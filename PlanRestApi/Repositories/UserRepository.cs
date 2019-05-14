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
    public class UserRepository : AbstractRepository<User>
    {
        public UserRepository(IConfiguration configuration) : base(configuration) { }
        public override bool Delete(int id)
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                if (db.State == ConnectionState.Closed)
                {
                    db.Open();
                }
                int result = db.Execute("DELETE FROM users WHERE id = @Id", 
                                                new { Id = id }, 
                                                commandType: CommandType.Text);
                return (result > 0);
            }
        }

        public override User Get(int id)
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
                if (db.State == ConnectionState.Closed)
                {
                    db.Open();
                }
                var user = db.QueryFirst<User>("SELECT * FROM users WHERE id = @Id", new { Id = id });
                return user;
            }
        }

        public override IEnumerable<User> GetAll()
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
                if (db.State == ConnectionState.Closed)
                {
                    db.Open();
                }
                IEnumerable<User> user = db.Query<User>("SELECT * FROM users");
                return (List<User>)user;
            }
        }

        public override User GetLastInserted()
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
                if (db.State == ConnectionState.Closed)
                {
                    db.Open();
                }
                var user = db.QueryFirst<User>(@"SELECT * FROM users WHERE id = IDENT_CURRENT('users')");
                return user;
            }
        }

        public override bool Insert(User item)
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                if (db.State == ConnectionState.Closed)
                {
                    db.Open();
                }
                int result = db.Execute(@"INSERT INTO users(name,register_date,last_changed_date) 
                                          VALUES('@Name',GETDATE(),GETDATE());", item);
                return (result > 0);
            }
        }

        public override bool Update(User item)
        {
            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                if (db.State == ConnectionState.Closed)
                {
                    db.Open();
                }
                int result = db.Execute(@"UPDATE  users 
                           SET name = @Name
                           WHERE id = @Id", item);
                return (result > 0);
            }
        }
    }
}
