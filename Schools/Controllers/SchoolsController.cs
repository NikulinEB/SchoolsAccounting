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
    public class SchoolsController: ApiController
    {
        // GET api/schools/get/5
        public HttpResponseMessage Get(int id)
        {
            try
            {
                using (var container = new SchoolsModelContainer())
                {
                    var school = container.SchoolSet.Find(id);
                    if (school != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, school);
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, $"Не найдена школа с Id {id}.");
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, $"Ошибка при выполнении запроса: {ex.Message}");
            }
        }

        public HttpResponseMessage GetAll()
        {
            try
            {
                using (var container = new SchoolsModelContainer())
                {
                    var schools = container.SchoolSet.ToArray();
                    if (schools.Count() > 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, schools);
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, $"Список школ пуст.");
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, $"Ошибка при выполнении запроса: {ex.Message}");
            }
        }

        // POST api/schools/add
        public HttpResponseMessage Add([FromBody]School newSchool)
        {
            try
            {
                if (newSchool == null)
                {
                    ModelState.AddModelError("School", "Неверный формат. Передайте объект School в формате JSON.");
                }
                else
                {
                    if (string.IsNullOrEmpty(newSchool.Name))
                    {
                        ModelState.AddModelError("Name", "Не указано название школы. Укажите название в поле Name.");
                    }
                    if (string.IsNullOrEmpty(newSchool.Address))
                    {
                        ModelState.AddModelError("Address", "Не указан адрес школы. Укажите адрес в поле Address.");
                    }
                }
                if (!ModelState.IsValid)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, ModelState.Values);
                }
                using (var container = new SchoolsModelContainer())
                {
                    container.SchoolSet.Add(newSchool);
                    container.SaveChanges();
                }
                return Request.CreateResponse(HttpStatusCode.OK, "Школа успешно добавлена.");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, $"Ошибка при выполнении запроса: {ex.Message}");
            }
        }
    }
}