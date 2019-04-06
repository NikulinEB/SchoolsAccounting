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
    public class SchoolsController: ApiController
    {
        // GET api/schools/get/5
        public School Get(int id)
        {
            try
            {
                using (var container = new SchoolsModelContainer())
                {
                     return container.SchoolSet.Find(id);
                }
            }
            catch
            {
                return null;
            }
        }

        public School[] GetAll()
        {
            try
            {
                using (var container = new SchoolsModelContainer())
                {
                    return container.SchoolSet.ToArray();
                }
            }
            catch
            {
                return null;
            }
        }

        // POST api/schools/add
        public HttpResponseMessage Add([FromBody]School newSchool)
        {
            try
            {
                using (var container = new SchoolsModelContainer())
                {
                    container.SchoolSet.Add(newSchool);
                    container.SaveChanges();
                }
                return new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            }
            catch
            {
                return new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);
            }
        }
    }
}