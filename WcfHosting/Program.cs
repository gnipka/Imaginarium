using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace WcfHosting
{
    class Program
    {
        static void Main(string[] args)
        {
            // Если запускает пользователь сам
            if (Environment.UserInteractive)
            {
                WcfService s = WcfService.Service;
                s.Start();
                Console.WriteLine("Хост стартовал.");

                while (Console.ReadLine().ToLower() != "exit") { }
                s.Stop();
                return;
            }
            else
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[] { new WinService() };
                ServiceBase.Run(ServicesToRun);
            }
        }
    }
}
