using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Epicodus
{
  public class Student
  {
    private int _id;
    private string _firstName;
    private string _lastName;
    private string _email;
    private string _picture;
    private DateTime _startDate;

    public Student(string firstName, string lastName, string email, string picture, DateTime startDate, int id = 0)
    {
      _firstName = firstName;
      _lastName = lastName;
      _email = email;
      _picture = picture;
      _startDate = startDate;
      _id = id;
    }

    public int GetId()
    {
      return _id;
    }

    public string GetFName()
    {
      return _firstName;
    }
    public string GetLName()
    {
      return _lastName;
    }
    public string GetEmail()
    {
      return _email;
    }
    public string GetPicture()
    {
      return _picture;
    }
    public DateTime GetStartDate()
    {
      return _startDate;
    }

    public override bool Equals(System.Object otherStudent)
    {
      if (!(otherStudent is Student))
      {
        return false;
      }
      else
      {
        Student newStudent = (Student) otherStudent;
        bool idEquality = (this.GetId() == newStudent.GetId());
        bool firstNameEquality = (this.GetFName() == newStudent.GetFName());
        bool lastNameEquality = (this.GetLName() == newStudent.GetLName());
        bool emailEquality = (this.GetEmail() == newStudent.GetEmail());
        bool startDateEquality = (this.GetStartDate() == newStudent.GetStartDate());
        return (idEquality && firstNameEquality && lastNameEquality && emailEquality && startDateEquality);
      }
    }
    //GetHash
    public override int GetHashCode()
    {
      return this.GetFName().GetHashCode();
    }

    public static List<Student> GetAll()
    {
      List<Student> allStudents = new List<Student>{};

      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM students;", conn);
      SqlDataReader rdr = cmd.ExecuteReader();

      while (rdr.Read())
      {
        int id = rdr.GetInt32(0);
        string firstName = rdr.GetString(1);
        string lastName = rdr.GetString(2);
        string email = rdr.GetString(3);
        string picture = rdr.GetString(4);
        DateTime startDate = rdr.GetDateTime(5);
        Student newStudent = new Student(firstName, lastName, email, picture, startDate, id);
        allStudents.Add(newStudent);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return allStudents;
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO students (fname, lname, email, picture, sdate) OUTPUT INSERTED.id VALUES (@fname, @lname, @email, @picture, @sDate);", conn);

      cmd.Parameters.Add(new SqlParameter("@fname", this.GetFName()));
      cmd.Parameters.Add(new SqlParameter("@lname", this.GetLName()));
      cmd.Parameters.Add(new SqlParameter("@email", this.GetEmail()));
      cmd.Parameters.Add(new SqlParameter("@picture", this.GetPicture()));
      cmd.Parameters.Add(new SqlParameter("@sDate", this.GetStartDate()));

      SqlDataReader rdr = cmd.ExecuteReader();

      while (rdr.Read())
      {
        _id = rdr.GetInt32(0);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
    }

    public static Student Find(int id)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM students WHERE id = @id;", conn);

      cmd.Parameters.Add(new SqlParameter("@id", id));
      SqlDataReader rdr = cmd.ExecuteReader();

      int studentId = 0;
      string firstName = null;
      string lastName = null;
      string email = null;
      string picture = null;
      DateTime defaultDate = new DateTime (2016, 08, 01);

      while (rdr.Read())
      {

        studentId = rdr.GetInt32(0);
        firstName = rdr.GetString(1);
        lastName = rdr.GetString(2);
        email = rdr.GetString(3);
        picture = rdr.GetString(4);
        defaultDate = rdr.GetDateTime(5);

      }
      Student foundStudent = new Student(firstName, lastName, email, picture, defaultDate, studentId);

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return foundStudent;
    }

    // public void Update(string firstName, string lastName, string email, string picture, DateTime startDate, int id = 0)
    public void UpdateAll(Student currentStudent)
    {//-->get error cannot implicitly convert type 'void'
    //-->static void .. error CS0176: Member 'Student.UpdateAll(Student)' cannot be accessed with an instance reference; qualify it with a type name instead

      // this;
      // currentStudent;
      //in routing .. need to have already set form @Model ...
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("UPDATE students SET fname = @fname, lname = @lname, email = @email, picture = @picture, sdate = @sDate OUTPUT INSERTED.fname, INSERTED.lname, INSERTED.email, INSERTED.picture, INSERTED.sdate WHERE id = @StudentId;", conn);
// CMD is already diffined in this scope - try googling long string update / multiple collums
      cmd.Parameters.Add(new SqlParameter("@fname", currentStudent.GetFName()));
      cmd.Parameters.Add(new SqlParameter("@lname", currentStudent.GetLName()));
      cmd.Parameters.Add(new SqlParameter("@email", currentStudent.GetEmail()));
      cmd.Parameters.Add(new SqlParameter("@picture", currentStudent.GetPicture()));
      cmd.Parameters.Add(new SqlParameter("@sDate", currentStudent.GetStartDate()));
      cmd.Parameters.Add(new SqlParameter("@StudentId", currentStudent.GetId()));
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this._firstName = rdr.GetString(0);
        this._lastName = rdr.GetString(1);
        this._email = rdr.GetString(2);
        this._picture = rdr.GetString(3);
        this._startDate = rdr.GetDateTime(4);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
    }

    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("DELETE FROM students;", conn);
      cmd.ExecuteNonQuery();
      if (conn != null)
      {
        conn.Close();
      }
    }
  }
}
