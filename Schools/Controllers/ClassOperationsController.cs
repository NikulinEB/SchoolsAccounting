using Newtonsoft.Json;
using Schools.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Schools.Controllers
{
    public class ClassOperationsController : ApiController
    {
        // POST api/classoperations/includestudent
        public HttpResponseMessage IncludeStudent([FromBody]ClassOperation operation)
        {
            try
            {
                using (var container = new SchoolsModelContainer())
                {
                    var student = container.StudentSet.Find(operation.Student.Id);
                    operation.Student = student;
                    var newClass = container.ClassSet.Find(operation.Class.Id);
                    operation.Class = newClass;
                    student.Class = newClass;
                    container.Entry(student).State = System.Data.Entity.EntityState.Modified;
                    operation.OperationType = GradeOperationType.Include;
                    if (operation.Date.Ticks == 0)
                    {
                        operation.Date = DateTime.Now;
                    }
                    container.ClassOperationSet.Add(operation);
                    container.SaveChanges();
                }
                return Request.CreateResponse(HttpStatusCode.OK, "Зачисление выполнено успешно.");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, $"Ошибка при выполнении запроса: {ex.Message}");
            }
        }

        // POST api/classoperations/excludestudent
        public HttpResponseMessage ExcludeStudent([FromBody]Student student)
        {
            try
            {
                using (var container = new SchoolsModelContainer())
                {
                    var currentStudent = container.StudentSet.Find(student.Id);
                    var currentClass = container.ClassSet.Find(currentStudent.Class.Id);
                    var operation = new ClassOperation() { Date = DateTime.Now, Student = currentStudent, Class = currentClass, OperationType = GradeOperationType.Exclude };
                    container.ClassOperationSet.Add(operation);
                    currentStudent.Class = null;
                    container.Entry(currentStudent).State = System.Data.Entity.EntityState.Modified;
                    container.SaveChanges();
                }
                return Request.CreateResponse(HttpStatusCode.OK, "Отчисление выполнено успешно.");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, $"Ошибка при выполнении запроса: {ex.Message}");
            }
        }
    }
}