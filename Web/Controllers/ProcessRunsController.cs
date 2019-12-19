using DataAccess;
using System;
using System.Web.Http;
using System.Web.OData.Query;
using System.Web.OData;
using System.Linq;

namespace Web.Controllers
{
    //To Do: Add valdtion to prevent things like duplicate data or incorrect input. 
    public class ProcessRunsController : ODataController
    {
        private static ODataValidationSettings _validationSettings = new ODataValidationSettings();

        IRepository repository;

        public ProcessRunsController(IRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        [EnableQuery]
        public IHttpActionResult Get()
        {
            try
            {
                var result = repository.GetQueryable<vwProcessRunConsolidation>();
                var t = result.ToList();
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }


        [HttpGet]
        [EnableQuery]
        public IHttpActionResult Get([FromODataUri] int key)
        {
            try
            {
                var result = repository.GetQueryable<vwProcessRunConsolidation>(x => x.ProcessId == key);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }


        
   
    }
}
