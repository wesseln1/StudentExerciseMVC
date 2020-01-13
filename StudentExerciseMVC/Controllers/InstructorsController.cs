using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using StudentExerciseMVC.Models;
using StudentExercsieMVC.Models.ViewModels;

namespace StudentExerciseMVC.Controllers
{
    public class InstructorsController : Controller
    {
        private readonly IConfiguration _config;

        public InstructorsController(IConfiguration config)
        {
            _config = config;
        }

        public SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }
        // GET: Instructors
        public ActionResult Index()
        {

            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT Id, FirstName, LastName, SlackHandle, Speciality, CohortId FROM Instructor";

                    var reader = cmd.ExecuteReader();

                    var instructors = new List<Instructor>();

                    while (reader.Read())
                    {
                        instructors.Add(new Instructor()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            SlackHandle = reader.GetString(reader.GetOrdinal("SlackHandle")),
                            Speciality = reader.GetString(reader.GetOrdinal("Speciality")),
                            CohortId = reader.GetInt32(reader.GetOrdinal("CohortId"))


                        });
                    }

                    reader.Close();
                    return View(instructors);
                }
            }
        }

        // GET: Instructors/Details/5
        public ActionResult Details(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT c.Id as CohortId, c.Name, i.Id as InstructorId, i.FirstName, i.LastName, i.SlackHandle, i.Speciality, i.CohortId 
                                     FROM Instructor i
                                     LEFT JOIN Cohort c ON c.Id = i.CohortId
                                     WHERE i.Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    var reader = cmd.ExecuteReader();

                    Instructor instructor = null;

                    while (reader.Read())
                    {
                        if (instructor == null)
                        {

                            instructor = new Instructor
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("InstructorId")),
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                SlackHandle = reader.GetString(reader.GetOrdinal("SlackHandle")),
                                Speciality = reader.GetString(reader.GetOrdinal("Speciality")),
                                CohortId = reader.GetInt32(reader.GetOrdinal("CohortId")),
                                Cohort = new Cohort()
                            };
                        }
                        var hasCohort = !reader.IsDBNull(reader.GetOrdinal("CohortId"));
                        if (hasCohort)
                        {
                            instructor.Cohort = new Cohort()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("CohortId")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                            };
                        }
                    }

                    reader.Close();
                    if (instructor == null)
                    {
                        return NotFound();
                    }
                    return View(instructor);
                }
            }
        }

        // GET: Instructors/Create
        public ActionResult Create()
        {
            var cohorts = GetCohorts().Select(cohorts => new SelectListItem
            {
                Text = cohorts.Name,
                Value = cohorts.Id.ToString()
            }).ToList();
            var viewModel = new InstructorViewModel
            {
                Instructor = new Instructor(),
                Cohorts = cohorts
            };
            return View(viewModel);
        }

        // POST: Instructors/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Instructor instructor)
        {
            try
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"INSERT INTO Instructor (FirstName, LastName, CohortId, SlackHandle, Speciality) VALUES (@firstName, @lastName, @cohortId, @slackHandle, @speciality)";
                        cmd.Parameters.Add(new SqlParameter("@firstName", instructor.FirstName));
                        cmd.Parameters.Add(new SqlParameter("@lastName", instructor.LastName));
                        cmd.Parameters.Add(new SqlParameter("@cohortId", instructor.CohortId));
                        cmd.Parameters.Add(new SqlParameter("@slackHandle", instructor.SlackHandle));
                        cmd.Parameters.Add(new SqlParameter("@speciality", instructor.Speciality));


                        cmd.ExecuteNonQuery();
                    }
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        // GET: Instructors/Edit/5
        public ActionResult Edit(int id)
        {
            var cohorts = GetCohorts().Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            }).ToList();

            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT Id, FirstName, LastName, CohortId, SlackHandle, Speciality
                                        FROM Instructor
                                        WHERE Id = @id";

                    cmd.Parameters.Add(new SqlParameter("@id", id));

                    var reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        var instructor = new Instructor
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            CohortId = reader.GetInt32(reader.GetOrdinal("CohortId")),
                            SlackHandle = reader.GetString(reader.GetOrdinal("SlackHandle")),
                            Speciality = reader.GetString(reader.GetOrdinal("Speciality"))
                        };
                        reader.Close();

                        var viewModel = new InstructorViewModel
                        {
                            Instructor = instructor,
                            Cohorts = cohorts
                        };
                        return View(viewModel);
                    }
                    reader.Close();
                    return View();
                }
            }
        }

        // POST: Instructors/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Instructor instructor)
        {
            try
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"UPDATE Instructor 
                                            SET 
                                            FirstName = @firstName, 
                                            LastName = @lastName, 
                                            CohortId = @cohortId,
                                            Speciality = @speciality,
                                            SlackHandle = @slackHandle
                                            WHERE Id = @id";

                        cmd.Parameters.Add(new SqlParameter("@firstName", instructor.FirstName));
                        cmd.Parameters.Add(new SqlParameter("@lastName", instructor.LastName));
                        cmd.Parameters.Add(new SqlParameter("@cohortId", instructor.CohortId));
                        cmd.Parameters.Add(new SqlParameter("@slackHandle", instructor.SlackHandle));
                        cmd.Parameters.Add(new SqlParameter("@speciality", instructor.Speciality));
                        cmd.Parameters.Add(new SqlParameter("@id", id));

                        cmd.ExecuteNonQuery();
                    }
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        // GET: Instructors/Delete/5
        public ActionResult Delete(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT FirstName, LastName, SlackHandle, Speciality, CohortId, Id FROM Instructor WHERE Id = @id";

                    cmd.Parameters.Add(new SqlParameter("@id", id));

                    var reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        var instructor = new Instructor
                        {
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            SlackHandle = reader.GetString(reader.GetOrdinal("SlackHandle")),
                            Speciality = reader.GetString(reader.GetOrdinal("Speciality")),
                            CohortId = reader.GetInt32(reader.GetOrdinal("CohortId")),
                            Id = reader.GetInt32(reader.GetOrdinal("Id"))
                        };
                        reader.Close();
                        return View(instructor);
                    }

                    return NotFound();
                }
            }
        }

        // POST: Instructors/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete([FromRoute]int id, Instructor instructor)
        {
            try
            {
                // TODO: Add delete logic here
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"DELETE FROM Instructor WHERE Id = @id";

                        cmd.Parameters.Add(new SqlParameter("Id", id));

                        cmd.ExecuteNonQuery();

                        return RedirectToAction(nameof(Index));
                    }
                }

            }
            catch
            {
                return View();
            }
        }
        private List<Cohort> GetCohorts()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, Name FROM Cohort";

                    var reader = cmd.ExecuteReader();

                    var cohorts = new List<Cohort>();

                    while (reader.Read())
                    {
                        cohorts.Add(new Cohort
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name"))
                        });
                    }

                    reader.Close();

                    return cohorts;
                }
            }
        }
    }
}