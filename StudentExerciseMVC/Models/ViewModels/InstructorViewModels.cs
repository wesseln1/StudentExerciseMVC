using Microsoft.AspNetCore.Mvc.Rendering;
using StudentExerciseMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentExercsieMVC.Models.ViewModels
{
    public class InstructorViewModel
    {
        public Instructor Instructor { get; set; }
        public List<SelectListItem?> Cohorts { get; set; }
    }
}
