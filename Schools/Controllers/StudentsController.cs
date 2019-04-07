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
                if (newStudent == null)
                {
                    ModelState.AddModelError("Student", "Неверный формат. Передайте объект Class в формате JSON.");
                }
                else
                {
                    if (string.IsNullOrEmpty(newStudent.FirstName) || string.IsNullOrEmpty(newStudent.LastName) || string.IsNullOrEmpty(newStudent.Patronymic))
                    {
                        ModelState.AddModelError("Name", "Не указано имя, фамилия или отчество. Укажите их в соответствующих полях: FirstName, LastName, Patronymic.");
                    }
                    if (string.IsNullOrEmpty(newStudent.Address))
                    {
                        ModelState.AddModelError("Address", "Не указан адрес. Укажите адрес в поле Address.");
                    }
                    if (newStudent.Birthday.Ticks == 0)
                    {
                        ModelState.AddModelError("Birthday", "Не указана дата рождения. Укажите дату рождения в поле Birthday, например, 1994 11 08.");
                    }
                }
                if (!ModelState.IsValid)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, ModelState.Values);
                }
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
                    // Поиск учеников на текущую дату.
                    if (input.Date.Ticks == 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, container.StudentSet.Where(s => s.Class.Id == input.Id).ToArray());
                    }
                    // Поиск учеников на указанную дату.
                    else
                    {
                        var includedStudents = container.StudentSet.ToList().Where(s => WasInClass(s.Id, input.Id, input.Date)).ToArray();
                        if (includedStudents.Count() > 0)
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, includedStudents);
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, "Отсутствуют ученики в указанном классе на указанную дату.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, $"Ошибка при выполнении запроса: {ex.Message}");
            }
        }

        private bool WasInClass(int studentId, int classId, DateTime date)
        {
            using (var container = new SchoolsModelContainer())
            {
                var operations = container.ClassOperationSet.Where(op => op.Student.Id == studentId && op.Class.Id == classId &&
                op.Date <= date);
                if (operations.Count() > 0)
                {
                    var sorted = operations.OrderBy(op => op.Date).ToList();
                    var last = sorted.Last();
                    return last.OperationType == GradeOperationType.Include;
                }
                return false;
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
                    // Поиск учеников на текущую дату.
                    if (input.Date.Ticks == 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, container.StudentSet.Where(s => s.Class.School.Id == input.Id).ToArray());
                    }
                    // Поиск учеников на указанную дату.
                    else
                    {
                        var includedStudents = container.StudentSet.ToList().Where(s => WasInSchool(s.Id, input.Id, input.Date)).ToArray();
                        if (includedStudents.Count() > 0)
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, includedStudents);
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, "Отсутствуют ученики в указанной школе на указанную дату.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, $"Ошибка при выполнении запроса: {ex.Message}");
            }
        }

        private bool WasInSchool(int studentId, int schoolId, DateTime date)
        {
            using (var container = new SchoolsModelContainer())
            {
                var operations = container.ClassOperationSet.Where(op => op.Student.Id == studentId && op.Class.School.Id == schoolId &&
                op.Date <= date);
                if (operations.Count() > 0)
                {
                    var sorted = operations.OrderBy(op => op.Date).ToList();
                    var last = sorted.Last();
                    return last.OperationType == GradeOperationType.Include;
                }
                return false;
            }
        }


        public class SearchParameters
        {
            public int Id { get; set; }
            public DateTime Date { get; set; }
        }
    }
}