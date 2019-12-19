using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface IDMService
    {
        void RunProcess(IRepository repository, IFTP ftp, ILogger logger);
    }
}
