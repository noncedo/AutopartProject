using DataAccess;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Query;

namespace Web.Controllers
{
    /*
    The WebApiConfig class may require additional changes to add a route for this controller. Merge these statements into the Register method of the WebApiConfig class as applicable. Note that OData URLs are case sensitive.

    using System.Web.Http.OData.Builder;
    using System.Web.Http.OData.Extensions;
    using DataAccess;
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
    builder.EntitySet<DealerGroup>("DealerGroups");
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class DealerGroupsController : ODataController
    {
        private static ODataValidationSettings _validationSettings = new ODataValidationSettings();

        IRepository repository;

        public DealerGroupsController(IRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        [EnableQuery]
        public IHttpActionResult GetDealerGroups()
        {
            try
            {
                var result = repository.GetQueryable<DealerGroup>();
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
        public IHttpActionResult GetDealerGroup([FromODataUri] int key)
        {
            try
            {
                var result = repository.GetQueryable<DealerGroup>(x => x.DealerGroupId == key).FirstOrDefault();
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        // PUT: odata/DealerGroups(5)
        [HttpPut]
        [EnableQuery]
        public async Task<IHttpActionResult> Put([FromODataUri] int key, DealerGroup DealerGroup)
        {
            Validate(DealerGroup);

            try
            {
                repository.Update<DealerGroup>(DealerGroup);
                await repository.SaveAsync();
                return Updated(DealerGroup);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        [HttpPost]
        [EnableQuery]
        // POST: odata/DealerGroups
        public async Task<IHttpActionResult> Post(DealerGroup DealerGroup)
        {
            try
            {
                repository.Create<DealerGroup>(DealerGroup);
                await repository.SaveAsync();
                return Created(DealerGroup);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        // PATCH: odata/DealerGroups(5)
        [HttpPatch]
        [AcceptVerbs("MERGE")]
        [EnableQuery]
        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<DealerGroup> delta)
        {
            try
            {
                var entity = await repository.GetByIdAsync<DealerGroup>(key);
                delta.Patch(entity);
                repository.Update<DealerGroup>(entity);
                await repository.SaveAsync();
                return Updated(entity);
            }
            catch (Exception ex)
            {
                return StatusCode(HttpStatusCode.InternalServerError);
            }

            // TODO: Get the entity here.

            // delta.Patch(DealerGroup);

            // TODO: Save the patched entity.

            // return Updated(DealerGroup);
        }

        [HttpDelete]
        [EnableQuery]
        // DELETE: odata/DealerGroups(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            try
            {
                repository.Delete<DealerGroup>(key);
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
