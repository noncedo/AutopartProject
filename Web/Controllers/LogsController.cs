using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using System.Web.Http.OData.Routing;
using DataAccess;
using Microsoft.Data.OData;

namespace Web.Controllers
{
    /*
    The WebApiConfig class may require additional changes to add a route for this controller. Merge these statements into the Register method of the WebApiConfig class as applicable. Note that OData URLs are case sensitive.

    using System.Web.Http.OData.Builder;
    using System.Web.Http.OData.Extensions;
    using DataAccess;
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
    builder.EntitySet<Log>("Logs");
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class LogsController : ODataController
    {
        private static ODataValidationSettings _validationSettings = new ODataValidationSettings();

        IRepository repository;

        public LogsController(IRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        [EnableQuery]
        public IHttpActionResult GetLogs()
        {
            try
            {
                var result = repository.GetQueryable<Log>();
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        // GET: odata/DealerGroups(5)
        [HttpGet]
        [EnableQuery]
        public IHttpActionResult GetLogs([FromODataUri] int key)
        {
            try
            {
                var result = repository.GetQueryable<Log>(x => x.LogID == key);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        // PUT: odata/Logs(5)
        public IHttpActionResult Put([FromODataUri] int key, Delta<Log> delta)
        {
            Validate(delta.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // TODO: Get the entity here.

            // delta.Put(log);

            // TODO: Save the patched entity.

            // return Updated(log);
            return StatusCode(HttpStatusCode.NotImplemented);
        }

        // POST: odata/Logs
        public IHttpActionResult Post(Log log)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // TODO: Add create logic here.

            // return Created(log);
            return StatusCode(HttpStatusCode.NotImplemented);
        }

        // PATCH: odata/Logs(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] int key, Delta<Log> delta)
        {
            Validate(delta.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // TODO: Get the entity here.

            // delta.Patch(log);

            // TODO: Save the patched entity.

            // return Updated(log);
            return StatusCode(HttpStatusCode.NotImplemented);
        }

        // DELETE: odata/Logs(5)
        public IHttpActionResult Delete([FromODataUri] int key)
        {
            // TODO: Add delete logic here.

            // return StatusCode(HttpStatusCode.NoContent);
            return StatusCode(HttpStatusCode.NotImplemented);
        }
    }
}
