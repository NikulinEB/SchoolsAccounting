using Newtonsoft.Json;
using Schools.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Net;

namespace Schools.Controllers
{
    public class ClassesController : ApiController
    {
        // GET api/class/get/5
        public HttpResponseMessage Get(int id)
        {
            try
            {
                using (var container = new SchoolsModelContainer())
                {
                    var resultClass = container.ClassSet.Find(id);
                    if (resultClass != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, resultClass);
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, $"Не найден класс с Id {id}.");
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, $"Ошибка при выполнении запроса: {ex.Message}");
            }
        }

        // POST api/class/add
        public HttpResponseMessage Add([FromBody]Class newClass)
        {
            try
            {
                if (newClass == null)
                {
                    ModelState.AddModelError("Class", "Неверный формат. Передайте объект Class в формате JSON.");
                }
                else
                {
                    if (string.IsNullOrEmpty(newClass.Letter))
                    {
                        ModelState.AddModelError("Letter", "Не указана литера класса. Укажите литеру в поле Letter.");
                    }
                    if (newClass.Number == 0)
                    {
                        ModelState.AddModelError("Number", "Не указан номер класса. Укажите номер в поле Number.");
                    }
                }
                if (!ModelState.IsValid)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, ModelState.Values);
                }
                using (var container = new SchoolsModelContainer())
                {
                    var school = container.SchoolSet.Find(newClass.SchoolId);
                    if (school != null)
                    {
                        container.ClassSet.Add(newClass);
                        container.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, "Класс успешно добавлен.");
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, $"Не найдена школа с Id: {newClass.SchoolId}. Укажите верный Id школы в поле SchoolId.");
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, $"Ошибка при выполнении запроса: {ex.Message}");
            }
        }

        // api/class/getbyschool
        public HttpResponseMessage GetBySchool(int id)
        {
            try
            {
                using (var container = new SchoolsModelContainer())
                {
                    if (container.SchoolSet.Find(id) == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, $"Не найдена школа с Id {id}.");
                    }
                    var classes = container.ClassSet.Where(s => s.School.Id == id).ToArray();
                    if (classes.Count() > 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, classes);
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, $"Не найдены классы для школы с Id {id}.");
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, $"Ошибка при выполнении запроса: {ex.Message}");
            }
        }
    }
}