using Schools.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net;

namespace Schools.Controllers
{
    public class StudentsController : ApiController
    {
        // GET api/students/get/5
        public HttpResponseMessage Get(int id)
        {
            try
            {
                using (var container = new SchoolsModelContainer())
                {
                    var student = container.StudentSet.Find(id);
                    if (student != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, student);
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, $"Не найден школьник с Id {id}.");
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, $"Ошибка при выполнении запроса: {ex.Message}");
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
                return Request.CreateResponse(HttpStatusCode.OK, "Школьник успешно добавлен.");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, $"Ошибка при выполнении запроса: {ex.Message}");
            }
        }

        // POST api/students/getbyclass/4
        [HttpPost]
        public HttpResponseMessage GetByClass([FromBody]SearchParameters input)
        {
            try
            {
                using (var container = new SchoolsModelContainer())
                {
                    return Request.CreateResponse(HttpStatusCode.OK, container.StudentSet.Where(s => s.Class.Id == input.Id && 
                        !container.ClassOperationSet.Any(op => op.Student.Id == s.Id && 
                        op.Class.Id == input.Id && op.Date <= input.Date && op.OperationType == GradeOperationType.Exclude)).ToArray());
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, $"Ошибка при выполнении запроса: {ex.Message}");
            }
        }

        // POST api/students/getbyschool/1
        [HttpPost]
        public HttpResponseMessage GetBySchool([FromBody]SearchParameters input)
        {
            try
            {
                using (var container = new SchoolsModelContainer())
                {
                    return Request.CreateResponse(HttpStatusCode.OK, container.StudentSet.Where(s => s.Class.School.Id == input.Id &&
                    !container.ClassOperationSet.Any(op => op.Student.Id == s.Id &&
                    op.Class.School.Id == input.Id && op.Date <= input.Date && op.OperationType == GradeOperationType.Exclude)).ToArray());
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, $"Ошибка при выполнении запроса: {ex.Message}");
            }
        }

        public class SearchParameters
        {
            public int Id { get; set; }
            public DateTime Date { get; set; }
        }
    }
}