using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLog.Targets.SignalR
{
    public class NLogToSignalRTargetException:Exception
    {
        public NLogToSignalRTargetException(string message):base(message)
        {
            
        }
    }
}
