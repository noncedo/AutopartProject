using DataAccess;
using System;
using System.Linq;
using System.Web.Http;
using System.Web.OData.Query;
using System.Web.OData;

namespace Web.Controllers
{
    //To Do: Add valdtion to prevent things like duplicate data or incorrect input. 
    public class DestinationFiles : ODataController
    {
        private static ODataValidationSettings _validationSettings = new ODataValidationSettings();

        IRepository repository;

        public DestinationFiles(IRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        [EnableQuery]
        public IHttpActionResult GetDestinationFiles()
        {
            try
            {
                var result = repository.GetQueryable<FileDestination>();
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }


        [HttpGet]
        [EnableQuery]
        public IHttpActionResult GetDestinationFiles([FromODataUri] int key)
        {
            try
            {
                var result = repository.GetQueryable<FileDestination>(x => x.FileId == key).FirstOrDefault();
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }


        
       
        
    }
}
