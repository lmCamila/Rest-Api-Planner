using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlanRestApi.Models
{
    public class Plan
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public User Sponsor { get; set; }
        public TypePlan Type { get; set; }
        public PlanStatus Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }
        public double Cost { get; set; }
        public List<User> Interested { get; set; }

    }
}
