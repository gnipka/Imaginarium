using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace GameHost
{
    class Program
    {
        static void Main(string[] args)
        {
            using(var host = new ServiceHost(typeof(wcf_game.ServiceGame)))
            {
                host.Open();
                Console.WriteLine("Хост стартовал.");
                Console.ReadLine();
            }
        }
    }
}
