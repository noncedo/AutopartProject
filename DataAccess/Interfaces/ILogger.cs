using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace DataAccess
{
    public interface ILogger
    {
        void Log(IRepository repository, Int32 processRunID, String log, [CallerMemberName] string callerName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);
        void Log(IRepository repository, Int32 processRunID, String log, Int32 logType, [CallerMemberName] string callerName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);
        void LogToTextFile(IRepository repository, Int32 processRunID, String source, String log, String stackTrace, Int32 logType);
        void LogToDatabase(IRepository repository, Int32 processRunID, String source, String log, String stackTrace, Int32 logType);
    }
}
