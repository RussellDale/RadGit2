﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Rad2.Models.Domian
{
    public partial class Instructor
    {
        public Instructor()
        {
            CourseAssignment = new HashSet<CourseAssignment>();
            Department = new HashSet<Department>();
        }
        public int Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public DateTime? HireDate { get; set; }
        public virtual OfficeAssignment OfficeAssignment { get; set; }
        public virtual ICollection<CourseAssignment> CourseAssignment { get; set; }
        public virtual ICollection<Department> Department { get; set; }
    }
}