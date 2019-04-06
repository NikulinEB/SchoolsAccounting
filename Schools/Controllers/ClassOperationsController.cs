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

        public class ClassOperationParameters
        {
            public int StudentId { get; set; }
            public int ClassId { get; set; }
            public DateTime Date { get; set; }
        }
        // POST api/classoperations/includestudent
        public HttpResponseMessage IncludeStudent([FromBody]ClassOperationParameters input)
        {
            try
            {
                if (input == null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "Неверный формат. Передайте объект IncludeStudentParameters в формате JSON.");
                }
                else if (input.StudentId == 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "Не указан Id школьника для добавления в класс. Укажите Id в поле StudentId.");
                }
                else if (input.ClassId == 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "Не указан Id класса. Укажите Id в поле ClassId.");
                }
                ExcludeStudent(new ClassOperationParameters { StudentId = input.StudentId, Date = input.Date });
                using (var container = new SchoolsModelContainer())
                {
                    var student = container.StudentSet.Find(input.StudentId);
                    if (student == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, $"Не найден школьник с Id {input.StudentId}.");
                    }
                    var operation = new ClassOperation();
                    operation.Student = student;
                    var newClass = container.ClassSet.Find(input.ClassId);
                    if (newClass == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, $"Не найден класс с Id {input.ClassId}.");
                    }
                    operation.Class = newClass;
                    operation.OperationType = GradeOperationType.Include;
                    // Если не указана нужная дата зачисления.
                    if (input.Date.Ticks == 0)
                    {
                        operation.Date = DateTime.Now;
                    }
                    else
                    {
                        operation.Date = input.Date;
                    }
                    student.Class = newClass;
                    container.Entry(student).State = System.Data.Entity.EntityState.Modified;
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
        public HttpResponseMessage ExcludeStudent([FromBody]ClassOperationParameters input)
        {
            try
            {
                if (input == null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "Неверный формат. Передайте объект IncludeStudentParameters в формате JSON.");
                }
                else if (input.StudentId == 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "Не указан Id школьника для добавления в класс. Укажите Id в поле StudentId.");
                }
                using (var container = new SchoolsModelContainer())
                {
                    var currentStudent = container.StudentSet.Find(input.StudentId);
                    if (currentStudent == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, $"Не найден школьник с Id {input.StudentId}.");
                    }
                    if (currentStudent.Class == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, $"Школьник с Id {input.StudentId} не числится ни в одном классе.");
                    }
                    var operation = new ClassOperation()
                    {
                        Student = currentStudent,
                        Class = currentStudent.Class,
                        OperationType = GradeOperationType.Exclude
                    };
                    // Если не указана нужная дата.
                    if (input.Date.Ticks == 0)
                    {
                        operation.Date = DateTime.Now;
                    }
                    else
                    {
                        operation.Date = input.Date;
                    }
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