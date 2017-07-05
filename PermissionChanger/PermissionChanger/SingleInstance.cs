using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PermissionChanger
{
    static public class SingleInstance
    {
        static Mutex mutex;

        static public bool Start()
        {
            bool onlyInstance = false;
            string mutexName = $"Local\\{ProgramInfo.AssemblyGuid}";
            mutex = new Mutex(true, mutexName, out onlyInstance);
            return onlyInstance;
        }

        static public void Stop()
        {
            mutex.ReleaseMutex();
        }
    }
}
