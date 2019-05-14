using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PlanRestApi.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        //public string Email { get; set; }
        public DateTime RegisterDate { get; set; }
        public DateTime LastChangedDate { get; set; }
    }
}
