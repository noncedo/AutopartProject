using DataAccess;
using System;
using System.Linq;
using System.Web.Http;
using System.Web.OData.Query;
using System.Web.OData;

namespace Web.Controllers
{
    //To Do: Add valdtion to prevent things like duplicate data or incorrect input. 
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


        [HttpGet]
        [EnableQuery]
        public IHttpActionResult GetLogs([FromODataUri] int key)
        {
            try
            {
                var result = repository.GetQueryable<Log>(x => x.LogID == key).FirstOrDefault();
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }


        
   
    }
}
