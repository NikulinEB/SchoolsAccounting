using Schools.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json;
using System.Net.Http;

namespace Schools.Controllers
{
    public class StudentsController : ApiController
    {
        // GET api/students/get/5
        public Student Get(int id)
        {
            try
            {
                using (var container = new SchoolsModelContainer())
                {
                    return container.StudentSet.Find(id);
                }
            }
            catch
            {
                return null;
            }
        }

        // POST api/students/add
        public HttpResponseMessage Add([FromBody]Student newStudent)
        {
            try
            {
                using (var container = new SchoolsModelContainer())
                {
                    container.StudentSet.Add(newStudent);
                    container.SaveChanges();
                }
                return new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            }
            catch
            {
                return new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);
            }
        }

        // POST api/students/getbyclass/4
        [HttpPost]
        public Student[] GetByClass([FromBody]SearchParameters input)
        {
            try
            {
                using (var container = new SchoolsModelContainer())
                {
                    return container.StudentSet.Where(s => s.Class.Id == input.Id && 
                        !container.ClassOperationSet.Any(op => op.Student.Id == s.Id && 
                        op.Class.Id == input.Id && op.Date <= input.Date && op.OperationType == GradeOperationType.Exclude)).ToArray();
                }
            }
            catch
            {
                return null;
            }
        }

        // POST api/students/getbyschool/1
        [HttpPost]
        public Student[] GetBySchool([FromBody]SearchParameters input)
        {
            try
            {
                using (var container = new SchoolsModelContainer())
                {
                    return container.StudentSet.Where(s => s.Class.School.Id == input.Id &&
                    !container.ClassOperationSet.Any(op => op.Student.Id == s.Id &&
                    op.Class.School.Id == input.Id && op.Date <= input.Date && op.OperationType == GradeOperationType.Exclude)).ToArray();
                }
            }
            catch
            {
                return null;
            }
        }

        public class SearchParameters
        {
            public int Id { get; set; }
            public DateTime Date { get; set; }
        }
    }
}