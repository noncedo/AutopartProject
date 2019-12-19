using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface II02Service
    {
        void RunProcess(IRepository repository, ILogger logger);
    }
}
