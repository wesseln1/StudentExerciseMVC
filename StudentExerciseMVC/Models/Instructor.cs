using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudentExerciseMVC.Models
{
    public class Instructor
    {
        [Display(Name = "Instructor Id")]
        public int Id { get; set; }
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required]
        [Display(Name = "Slack Handle")]
        public string SlackHandle { get; set; }
        public string Speciality { get; set; }
        [Display(Name = "Cohort Id")]
        public int CohortId { get; set; }
        [Display(Name = "Cohort")]
        public Cohort Cohort { get; set; }
    }
}
