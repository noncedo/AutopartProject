using DataAccess;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData.Query;
using System.Web.OData;

namespace Web.Controllers
{
    //To Do: Add valdtion to prevent things like duplicate data or incorrect input. 
    public class ProcessesController : ODataController
    {
        private static ODataValidationSettings _validationSettings = new ODataValidationSettings();

        IRepository repository;

        public ProcessesController(IRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        [EnableQuery]
        public IHttpActionResult GetProcesses()
        {
            try
            {
                var result = repository.GetQueryable<Process>();
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }


        [HttpGet]
        [EnableQuery]
        public IHttpActionResult GetProcesses([FromODataUri] int key)
        {
            try
            {
                var result = repository.GetQueryable<Process>(x => x.ProcessId == key).FirstOrDefault();
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }


        
        [HttpPut]
        [EnableQuery]
        public async Task<IHttpActionResult> Put([FromODataUri] int key, Process process)
        {
            Validate(process);

            try
            {
                repository.Update<Process>(process);
                await repository.SaveAsync();
                return Updated(process);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }


        [HttpPost]
        [EnableQuery]
        public async Task<IHttpActionResult> Post(Process process)
        {
            try
            {
                repository.Update<Process>(process);
                await repository.SaveAsync();
                return Updated(process);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }


       
        [HttpPatch]
        [AcceptVerbs("MERGE")]
        [EnableQuery]
        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<Process> delta)
        {
            try
            {
                var entity = await repository.GetByIdAsync<Process>(key);
                delta.Patch(entity);
                repository.Update<Process>(entity);
                await repository.SaveAsync();
                return Updated(entity);
            }
            catch (Exception ex)
            {
                return StatusCode(HttpStatusCode.InternalServerError);
            }

           
        }

        [HttpDelete]
        [EnableQuery]
        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            try
            {
                repository.Delete<Process>(key);
                repository.Save();
                return Ok();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
    }
}
