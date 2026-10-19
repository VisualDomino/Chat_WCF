using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace Chat_WCF_Service
{
    [ServiceContract]
    public interface IChat
    {
        [OperationContract]
        string WhichServiceAmI();

        [OperationContract]
        void SendMessage(string message);

        [OperationContract]
        void SendMessage2(string username, string message);

        [OperationContract]
        string ReceiveMessages(ref int id);
    }

    public class Chat : IChat
    {
        public const string ServiceName = "WCF_GoluVodi_Service";

        private static readonly List<string> Messages = new List<string>();

        public string WhichServiceAmI()
        {
            return ServiceName;
        }

        public void SendMessage(string message)
        {
            string newMessage = message;

            Messages.Add(newMessage);
        }

        public void SendMessage2(string username, string message)
        {
            string newMessage =
                username + ": " + message;

            Messages.Add(newMessage);
        }

        public string ReceiveMessages(ref int id)
        {
            if (id < 0)
                id = 0;

            string result = "";

            for (int i = id; i < Messages.Count; i++)
            {
                result += "[" + (i + 1) + "] "
                    + Messages[i]
                    + "\n";
            }

            id = Messages.Count;

            return result;
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            const string URL = "http://localhost:2310/Chat";

            Console.WriteLine(Chat.ServiceName);
            Console.WriteLine("URL: " + URL);
            Console.Write("Starting...");

            ServiceHost myService =
                new ServiceHost(typeof(Chat));

            myService.AddServiceEndpoint(
                typeof(IChat),
                new BasicHttpBinding(BasicHttpSecurityMode.None),
                URL);

            myService.Open();

            Console.WriteLine("Running!");
            Console.WriteLine("Press ENTER to close");

            Console.ReadLine();

            myService.Close();
        }
    }
}