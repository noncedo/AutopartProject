using DataAccess;
using System.Web.Http;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;

namespace Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            ODataModelBuilder builder = new ODataConventionModelBuilder();
            config.Count().Filter().OrderBy().Expand().Select().MaxTop(null);
            
            builder.EntitySet<DealerGroup>("DealerGroups");
            builder.EntitySet<Dealer>("Dealers");
            builder.EntitySet<User>("Users");
            builder.EntitySet<ILHAudit>("ILHAudits");
            builder.EntityType<vwILHConsolidation>().HasKey(x => x.ProcessId);
            builder.EntitySet<vwILHConsolidation>("ILHConsolidations");
            builder.EntitySet<Process>("Processes");
            builder.EntityType<vwProcessRunConsolidation>().HasKey(x => x.ProcessRunId);
            builder.EntitySet<vwProcessRunConsolidation>("ProcessRuns");
            builder.EntityType<FileSource>().HasKey(x => x.FileId);
            builder.EntitySet<FileSource>("SourceFiles");
            builder.EntityType<FileDestination>().HasKey(x => x.FileId);
            builder.EntitySet<FileDestination>("DestinationFiles");
            builder.EntityType<Log>().HasKey(x => x.LogID);
            builder.EntitySet<Log>("Logs");
            builder.EntityType<FileLine>().HasKey(x => x.FileLineId);
            builder.EntitySet<FileLine>("FileLines");
            builder.EntityType<DmsProvider>().HasKey(x => x.DmsProviderId);
            builder.EntitySet<DmsProvider>("DmsProviders");
            builder.EntityType<Setting>().HasKey(x => x.SettingId);
            builder.EntitySet<Setting>("Settings");


            config.MapODataServiceRoute("odata", "odata", model: builder.GetEdmModel());
            

        }
    }
}
