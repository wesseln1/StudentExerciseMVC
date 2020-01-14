# StudentExerciseMVC

### Student Exercise Web Application

### Displaying a List of Cohorts

Your first task for this exercise is to start a new Visual Studio Web Application (MVC) project named `StudentExercisesMVC`. Then make a controller and corresponding Razor templates in order to manage the cohorts for your database.

Use scaffolding to...

1. Create a `CohortsController` in your controllers directory.
1. Create a `Views > Cohorts` directory and use the scaffolding to the create the `Index`, `Details`, `Create`, `Edit`, and `Delete` views.
1. In your controller, use ADO.NET to execute SQL statements for all of those actions.

### Displaying a List of Instructors

Use scaffolding to...

1. Create an `InstructorsController` in your controllers directory.
1. Create a `Views > Instructors` directory and use the scaffolding to the create the `Index`, `Details`, `Create`, `Edit`, and `Delete` views.
1. In your controller, use ADO.NET to execute SQL statements for all of those actions.

When you create or edit an instructor, you should be able to assign the instructor to a cohort from a `select` element in the form.

### Displaying a List of Students

Use scaffolding to...

1. Create a `StudentsController` in your controllers directory.
1. Create a `Views > Students` directory and use the scaffolding to the create the `Index`, `Details`, `Create`, `Edit`, and `Delete` views.
1. In your controller, use ADO.NET to execute SQL statements for all of those actions.

When you create or edit a student, you should be able to assign the student to a cohort from a `select` element in the form.

### Displaying a List of Exercises

Use scaffolding to...

1. Create an `ExercisesController` in your controllers directory.
1. Create a `Views > Exercises` directory and use the scaffolding to the create the `Index`, `Details`, `Create`, `Edit`, and `Delete` views.
1. In your controller, use ADO.NET to execute SQL statements for all of those actions.

## Part 2

### Assigning Exercises to Students

#### Multi-select for Many-to-Many

> **Note:** You will need a custom view model for this task _(e.g. `StudentEditViewModel`)_

Modify your student edit form to display all exercises in a multi-select element. The user should be able to select one, or more exercises, in that element. When the user submits the form, then the `StudentExercises` table in your database should have a new entry added for each of the exercises that were selected in the form.

#### Details

When you view the details of an individual student, then there should be a list of assigned exercises in the view.

## Part 3

### Assigning Cohort to Instructor

#### Editing an Instructor

> **Note:** You will need a custom view model for this task _(e.g. `InstructorEditViewModel`)_

Modify your Instructor edit form to display all cohorts in a select element. The user should be able to select one of the cohorts in the dropdown. When the user submits the form, then the corresponding row in the `Instructor` table in your database should have its `CohortId` column value updated.

#### Details

When you view the details of an instructor, it should display the name of the cohort she is currently teaching.

### Starter Code

#### ViewModel Starter Code

```cs
namespace StudentExerciseMVC.Models.ViewModels
{
    public class InstructorEditViewModel
    {
        private readonly IConfiguration _config;

        public List<SelectListItem> Cohorts { get; set; }
        public Instructor Instructor { get; set; }

        public InstructorEditViewModel() { }

        public InstructorEditViewModel(IConfiguration config)
        {

            /*
                Query the database to get all cohorts
            */


            /*
                Use the LINQ .Select() method to convert
                the list of Cohort into a list of SelectListItem
                objects
            */
        }
    }
}
```

#### Controller Starter Code

```cs
// GET: Instructors/Edit/5
[HttpGet]
public async Task<ActionResult> Edit(int id)
{
    string sql = $@"
    SELECT
        i.Id,
        i.FirstName,
        i.LastName,
        i.SlackHandle,
        i.CohortId
    FROM Instructor i
    WHERE i.Id = {id}
    ";

    /*
        Run the query above and create an instance of Instructor
        populated with the data it returns
     */

     /*
        Create an instance of your InstructorEditViewModel
      */

    /*
        Assign the instructor you created to the .Instructor
        property of your view model
     */

    return View(viewModel);
}
```

#### Razor Template Starter Code

Use the follow tag helper in your Razor template for instructor edit.

```html
<div class="form-group">
  <label asp-for="Cohorts" class="control-label"></label>
  <select asp-for="Instructor.CohortId" asp-items="@Model.Cohorts"></select>
</div>
```

## Part 4

### Assigning Cohort to Student

#### Editing a Student's Cohort and Exercises

Take the work you did in the last exercise of assigning a cohort to an instructor and do the same for the student edit form. When you are done with this exercise, you should be able to successfully do the following actions when editing a student.

1. Assign a student to a single cohort
1. Assign 1-_n_ exercises to a student
