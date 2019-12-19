using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public interface IFTP
    {
        //void RunProcess(IRepository repository);
        void Put(String username, String password, String file);

        Boolean Get(String ipAddress, String username, String password, String remoteFile, String localFile);
    }
}
