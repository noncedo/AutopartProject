using DataAccess;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.OData.Query;
using System.Web.OData;

namespace Web.Controllers
{
    /*
    The WebApiConfig class may require additional changes to add a route for this controller. Merge these statements into the Register method of the WebApiConfig class as applicable. Note that OData URLs are case sensitive.

    using System.Web.Http.OData.Builder;
    using System.Web.Http.OData.Extensions;
    using DataAccess;
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
    builder.EntitySet<Setting>("Settings");
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class SettingsController : ODataController
    {
        private static ODataValidationSettings _validationSettings = new ODataValidationSettings();

        IRepository repository;

        public SettingsController(IRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        [EnableQuery]
        public IHttpActionResult GetSettings()
        {
            try
            {
                var result = repository.GetQueryable<Setting>();
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }


        [HttpGet]
        [EnableQuery]
        public IHttpActionResult GetSetting([FromODataUri] int key)
        {
            try
            {
                var result = repository.GetQueryable<Setting>(x => x.SettingId == key).FirstOrDefault();
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        [HttpPut]
        [EnableQuery]
        public async Task<IHttpActionResult> Put([FromODataUri] int key, Setting Setting)
        {
            Validate(Setting);

            try
            {
                var user = await repository.GetFirstAsync<User>(x => x.Username == HttpContext.Current.User.Identity.Name);
                repository.Update<Setting>(Setting);
                await repository.SaveAsync();
                return Updated(Setting);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        [HttpPost]
        [EnableQuery]

        public async Task<IHttpActionResult> Post(Setting Setting)
        {
            try
            {
                var t = User.Identity.Name;
                var user = await repository.GetFirstAsync<User>(x => x.Username == t);
                repository.Create<Setting>(Setting);
                await repository.SaveAsync();
                return Created(Setting);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }


        [HttpPatch]
        [AcceptVerbs("MERGE")]
        [EnableQuery]
        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<Setting> delta)
        {
            try
            {
                var user = await repository.GetFirstAsync<User>(x => x.Username == HttpContext.Current.User.Identity.Name);
                var entity = await repository.GetByIdAsync<Setting>(key);

                delta.Patch(entity);
                repository.Update<Setting>(entity);
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
                var user = await repository.GetFirstAsync<User>(x => x.Username == HttpContext.Current.User.Identity.Name);
                repository.Delete<Setting>(key);
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
