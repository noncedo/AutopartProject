using DataAccess;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class ILHService : IILHService
    {
        public void RunProcess(IRepository repository, IFTP ftp, ILogger logger)
        {
            ////Get the ILH process from the DB
            ////Will be used to link to this specific run
            //var process = repository.GetFirst<Process>(x => x.Name == "ILH");
            ////Create a new process run linked to the ILH Process type
            ////Set start to current time
            //var processStart = DateTime.Now;
            //var processRun = new ProcessRun { ProcessId = process.ProcessId, Start = processStart };
            ////Create the process run in the DB
            ////Actions won't actually be executed in the DB until repository.Save() is called
            //repository.Create<ProcessRun>(processRun);
            //repository.Save();
            ////Get settings for current process
            //var settings = repository.GetFirst<Setting>(x => x.ProcessId == process.ProcessId);

            //foreach(var fileInfo in new DirectoryInfo(settings.SourcePath).GetFiles())
            //{
            //    //Create a new file in the DB
            //    var file = new DataAccess.File { Path = settings.SourcePath, Name = fileInfo.Name, ProcessRunId = processRun.ProcessRunId };
            //    repository.Create<DataAccess.File>(file);
            //    repository.Save();
            //    using (StreamReader reader = new StreamReader(fileInfo.FullName))
            //    {
            //        string line;
            //        while ((line = reader.ReadLine()) != null)
            //        {
            //            // Write FileLine to DB
            //            var fileLine = new FileLine { FileId = file.FileId, Line = line };
            //            repository.Create<FileLine>(fileLine);
            //        }
            //    }
            //    repository.Save();
            //}
            
        }
    }
}
