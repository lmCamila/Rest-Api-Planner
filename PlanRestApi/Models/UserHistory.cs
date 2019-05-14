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
        public User User { get; set; }
        public byte Status { get; set; }
        public byte CreateNewPlan { get; set; }
        public DateTime Date { get; set; }
    }
  
}
