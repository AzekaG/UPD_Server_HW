using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPD_Server_HW
{
    internal class Program
    {
        

        static void Main(string[] args)
        {
            Controller controller = new Controller();
            controller.GetMessageFromClientToConsole += ConsoleWrMessage;
            Task.Run(()=> controller.WaitClientQuery());
            Console.ReadKey();
            




        }


       static void ConsoleWrMessage(string message) 
        {
            Console.WriteLine(message);
        }
    }
}
