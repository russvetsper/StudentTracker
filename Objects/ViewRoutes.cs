using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Epicodus
{
  public class ViewRoutes
  {
    public static Dictionary<string, object> IndexView()
    {
      List<Student> studentList = Student.GetAll();
      List<Course> courseList = Course.GetAll();
      List<Project> projectList = Project.GetAll();
      Dictionary<string, object> model = new Dictionary<string, object>{};
      model.Add("studentList", studentList);
      model.Add("courseList", courseList);
      model.Add("projectList", projectList);
      return model;
    }
  }
}