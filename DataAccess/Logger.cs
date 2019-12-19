using DataAccess;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Runtime.CompilerServices;
// Get call stack


namespace DataAccess
{
    class Logger : ILogger
    {
        public const Int32 INFO = 1;
        public const Int32 DEBUG = 2;
        public const Int32 ERROR = 3;

        public const String LogFile = "LogFile";

        public void Log(IRepository repository, Int32 processRunID, String log, [CallerMemberName] string callerName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            String source = String.Format("{0}{1} (Line {2})", Path.GetFileName(sourceFilePath).Replace("cs", ""), callerName, sourceLineNumber);

            LogToTextFile(repository, processRunID, source, log, String.Empty, INFO);
            LogToDatabase(repository, processRunID, source, log, String.Empty, INFO);
        }

        public void Log(IRepository repository, Int32 processRunID, String log, Int32 logType = INFO, [CallerMemberName] string callerName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            String source = String.Format("{0}{1} (Line {2})", Path.GetFileName(sourceFilePath).Replace("cs", ""), callerName, sourceLineNumber);

            LogToTextFile(repository, processRunID, source, log, String.Empty, logType);
            LogToDatabase(repository, processRunID, source, log, String.Empty, logType);
        }


        public void LogToTextFile(IRepository repository, Int32 processRunID, String source, String log, String stackTrace, Int32 logType = INFO)
        {
            var settings = repository.Get<Setting>();
            var sourcePath = settings.First(x => x.Key == LogFile).Value;

            String logPath = String.Format("{0}{1:yyyyMMdd}.{2}", sourcePath, DateTime.Now, "DealerMovementLog.txt");

            StreamWriter sw = new StreamWriter(logPath, true);
            sw.WriteLine(String.Format("{0:yyyyMMdd H:mm:ss} {1}", DateTime.Now, log));
            sw.Close();
        }

        public void LogToDatabase(IRepository repository, Int32 processRunID, String source, String log, String stackTrace, Int32 logType = INFO)
        {
            var logger = new DataAccess.Log { LogTypeID = logType, ProcessRunID = processRunID, Source = source, Message = log, StackTrace = stackTrace, LogDate = DateTime.Now };
            repository.Create<DataAccess.Log>(logger);
            repository.Save();
        }
    }
}
