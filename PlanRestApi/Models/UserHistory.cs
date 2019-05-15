using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PlanRestApi.Models
{
    public class UserHistory
    {
        public int Id { get; set; }
        public byte Status { get; set; }
        public byte CreateNewPlan { get; set; }
        public DateTime Date { get; set; }
        public int IdUser { get; set; }
    }

    public class UserHistoryConfig
    {
        public User User { get; set; }
        public List<UserHistory> History { get; set; }
    }

    public static class UserHistoryConfigExtensions
    {
        public static UserHistoryConfig ToUserHistoryConfig(this IEnumerable<UserHistory> history, User user)
        {
            return new UserHistoryConfig
            {
                User = user,
                History = history.ToList()
            };
        }
    }
}
