using Newtonsoft.Json;
using Schools.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Schools.Controllers
{
    public class ClassesController : ApiController
    {
        // GET api/class/get/5
        public Class Get(int id)
        {
            try
            {
                using (var container = new SchoolsModelContainer())
                {
                    return container.ClassSet.Find(id);
                }
            }
            catch
            {
                return null;
            }
        }

        // POST api/class/add
        public HttpResponseMessage Add([FromBody]Class newClass)
        {
            try
            {
                using (var container = new SchoolsModelContainer())
                {
                    container.ClassSet.Add(newClass);
                    container.SaveChanges();
                }
                return new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            }
            catch
            {
                return new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);
            }
        }

        // api/class/getbyschool
        public Class[] GetBySchool(int id)
        {
            try
            {
                using (var container = new SchoolsModelContainer())
                {
                    return container.ClassSet.Where(s => s.School.Id == id).ToArray();
                }
            }
            catch
            {
                return null;
            }
        }
    }
}