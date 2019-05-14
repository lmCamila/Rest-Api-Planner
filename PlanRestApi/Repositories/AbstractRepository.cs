using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlanRestApi.Repositories
{
    public abstract class AbstractRepository<T>
    {
        private string _connectionString;
        protected string ConnectionString => _connectionString;
        public AbstractRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetValue<string>("DbInfo:ConnectionString");
        }
        public abstract bool Insert(T item);
        public abstract bool Update(T  item);
        public abstract bool Delete(int id);
        public abstract T Get(int id);
        public abstract IEnumerable<T> GetAll();
        public abstract T GetLastInserted();
    }
}
