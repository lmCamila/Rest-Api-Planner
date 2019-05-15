using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlanRestApi.Models
{
    public class PlanHistory
    {
        public int Id { get; set; }
        public int IdPlan { get; set; }
        public PlanStatus Status { get; set; }
        public DateTime Date { get; set; }
    }

    public class PlanHistoryConfig
    {
        public Plan Plan { get; set; }
        public List<PlanHistory> History { get; set; }
    }

    public static class PlanHistoryConfigExtensions
    {
        public static PlanHistoryConfig ToPlanHistoryConfig(this IEnumerable<PlanHistory> history, Plan plan)
        {
            return new PlanHistoryConfig
            {
                Plan = plan,
                History = history.ToList()
            };
        }
    }
}
