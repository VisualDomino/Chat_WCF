using System;
using System.ServiceModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Data.Common;
using System.CodeDom.Compiler;

namespace Chat_WCF_Service
{
    [ServiceContract]
    public interface IChat
    {
        [OperationContract] string WhichServiceAmI();
        [OperationContract] void SendMessage(string message);
        [OperationContract] string ReceiveMessages(ref int id);
    }

    public class Chat : IChat
    {
        public const string ServiceName = "WCF_GoluVodi_Service";
        static string[] NewMessage = {};
        public string WhichServiceAmI()
        {
            return ServiceName;
        }

        public void SendMessage (string message)
        {
            try
            {
                NewMessage = NewMessage.Append(message).ToArray();
                int id = NewMessage.Length;
                ReceiveMessages(ref id);
                Thread.Sleep(1000);
                NewMessage.Skip(NewMessage.Length).ToArray();
            }
            catch
            {
                Console.WriteLine("Hilfe ich bin in Gefahr!!!!!!!");
            }

        }

        public string ReceiveMessages(ref int id)
        {
            if(id == 0) return "";

            string[] CombinedMessage = {};
            for(int i = 0; i<=id; i++)
            {
                CombinedMessage = CombinedMessage.Append(NewMessage[i] + "\n").ToArray();
            }

            return string.Join("", CombinedMessage);
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            const string URL = "http://localhost:2310";

            Console.WriteLine(Chat.ServiceName);
            Console.WriteLine("URL: " + URL);
            Console.Write("Starting...");

            ServiceHost myService = new ServiceHost(typeof(Chat));

            myService.AddServiceEndpoint(
                typeof(IChat),
                new BasicHttpBinding(BasicHttpSecurityMode.None),
                URL);

            myService.Open();

            Console.WriteLine("Running!");
            Console.WriteLine("Press 'ENTER' to close");
            Console.ReadLine();

            myService.Close();
        }
    }
}
