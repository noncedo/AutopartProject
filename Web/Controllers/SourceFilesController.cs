using DataAccess;
using System;
using System.Linq;
using System.Web.Http;
using System.Web.OData.Query;
using System.Web.OData;

namespace Web.Controllers
{
    //To Do: Add valdtion to prevent things like duplicate data or incorrect input. 
    public class SourceFilesController : ODataController
    {
        private static ODataValidationSettings _validationSettings = new ODataValidationSettings();

        IRepository repository;

        public SourceFilesController(IRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        [EnableQuery]
        public IHttpActionResult GetSourceFiles()
        {
            try
            {
                var result = repository.GetQueryable<FileSource>();
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }


        [HttpGet]
        [EnableQuery]
        public IHttpActionResult GetSourceFiles([FromODataUri] int key)
        {
            try
            {
                var result = repository.GetQueryable<FileSource>(x => x.FileId == key).FirstOrDefault();
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }


        
   
    }
}
