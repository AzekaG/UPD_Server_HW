
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace UPD_Server_HW
{
    internal class Controller
    {
        Random random = new Random();
        int port = 49152;
        Socket socket;
         public delegate void ControllerEventHandler(string msg);
         public event ControllerEventHandler GetMessageFromClientToConsole;
        [Serializable]
        public struct Message
        {
            public string mes; 
            public string user;
            public override string ToString()
            {
                return $"{user} : {mes}    {DateTime.Now.ToShortTimeString()}";
            }
        }




        public void WaitClientQuery()
        {
            try
            { 
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Any, port);
            socket = new Socket(AddressFamily.InterNetwork,  SocketType.Dgram, ProtocolType.Udp);
            socket.Bind(iPEndPoint);
                while (true)
                {
                    
                    EndPoint remote = new IPEndPoint(0x7F000000, 100);
                    byte[] arr = new byte[socket.ReceiveBufferSize];
                    
                    socket.ReceiveFrom(arr, ref remote);
                    Console.WriteLine("arr size = " + arr.Length);
                    string clientIp = ((IPEndPoint)remote).Address.ToString();
                    

                    MemoryStream stream = new MemoryStream(arr);
                    BinaryFormatter formatter = new BinaryFormatter();
                   
                    string msg = (string)formatter.Deserialize(stream);
                    
                    GetMessageFromClientToConsole(clientIp + " : " +  msg.ToString());

                    Task.Run(()=> SendAnswer(msg, (IPEndPoint)remote));
 

                }


            }catch (Exception ex) { Console.WriteLine(ex.ToString()); }

            Console.WriteLine("Waiting Close");


        }

        void SendAnswer(string message , IPEndPoint iPEndPoint)
         {


            Console.WriteLine("SendAnswer start");
            string text =   HandlerAnswer(message);

            Console.WriteLine("HandlerAnswer start    " + HandlerAnswer(message));
            byte[] arr = new byte[Encoding.UTF8.GetBytes(text).Length];
            arr = Encoding.UTF8.GetBytes(text);
            Console.WriteLine(arr.Length + "     arr length");
             socket.SendTo(arr , iPEndPoint);
            Console.WriteLine("Sent");


        }
       
            public string HandlerAnswer(string dataMSG)
            {
                string data = dataMSG.ToLower();
                string res = "Сервер : ";
                if (data.Contains("привет"))
                    res += "Привет)";
                if (data.Contains("дела"))
                    res += "У меня дела нормально , а твои как?";
                if (data.Contains("погода"))
                    res += "та вроде бы у нас сегодня ночью прохладно , но завтра обещают тепло)";
                if (data.Contains("анекдот"))
                    res += AnecdotRand();
                if (data.Contains("зовут"))
                res += "ну..скуажем так , мне пока имя не давали)";

                if(res== "Сервер : ") res += "Я не знаю как обработать это сообщение , вохможно в ближайшее время научусь))) ";


                return res;
            }



            public string AnecdotRand()
            {
                List<string> anecdots = new List<string>();

                anecdots.Add("Мужчины без материальных проблем - это те, кого женщины ищут. С материальными проблемами- те, кого женщины уже нашли.");
                anecdots.Add("Приходит Сара домой из поликлиники. Абрам, ты знаешь, то что мы с тобой тридцать лет имели за оргазм, на самом деле астма!");
                anecdots.Add("- Вы случайно не сын портного Изи?- Таки да, но почему случайно?");
                anecdots.Add("- Скажите, Додик, расстегай - это рыба или мясо?- Фирочка, это глагол.");
                anecdots.Add("- Сёма, а ты приснился мне в эротическом сне…. - Да? Ну и шо я там вытворял?- Пришёл и тока всё испортил!");
                anecdots.Add("Как говорит заслуженный учитель младших классов Изольда Давидовна, если вас знают только с хорошей стороны, так сидите и не вертитесь.");





                return anecdots.ElementAt(random.Next(0, anecdots.Count - 1));
            }
        

    }

}
