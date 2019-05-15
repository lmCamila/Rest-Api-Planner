using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlanRestApi.Models
{
    public class PlanUpload
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Sponsor { get; set; }
        public int Type { get; set; }
        public int Status { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Description { get; set; }
        public double Cost { get; set; }
        public string Interested { get; set; }

    }
}
