using System;
using System.ServiceModel;
using System.Threading;

namespace Chat_WCF_Client
{
    [ServiceContract]
    public interface IChat
    {
        [OperationContract]
        string WhichServiceAmI();

        [OperationContract]
        void SendMessage(string username, string message);

        [OperationContract]
        string ReceiveMessages(int lastMessageId);
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            const string URL = "http://localhost:2310/IChat";

            Console.Write("Username: ");
            string username = Console.ReadLine();

            ChannelFactory<IChat> factory =
                new ChannelFactory<IChat>(
                    new BasicHttpBinding(BasicHttpSecurityMode.None),
                    new EndpointAddress(URL));

            IChat chat = factory.CreateChannel();

            Console.WriteLine();
            Console.WriteLine("Verbunden mit: " + chat.WhichServiceAmI());
            Console.WriteLine("Du bist: " + username);
            Console.WriteLine("Schreibe eine Nachricht und drücke ENTER.");
            Console.WriteLine("Mit /exit beendest du den Chat.");
            Console.WriteLine();

            int lastMessageId = 0;

            while (true)
            {
                Console.Write("> ");
                string message = Console.ReadLine();

                if (message == "/exit")
                    break;

                if (string.IsNullOrWhiteSpace(message))
                    continue;

                try
                {
                    chat.SendMessage(username, message);

                    string newMessages =
                        chat.ReceiveMessages(lastMessageId);

                    if (!string.IsNullOrEmpty(newMessages))
                    {
                        Console.WriteLine(newMessages);

                        // Anzahl der erhaltenen Nachrichten bestimmen
                        string[] lines =
                            newMessages.Split(
                                new[] { '\n' },
                                StringSplitOptions.RemoveEmptyEntries);

                        lastMessageId += lines.Length;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Fehler: " + ex.Message);
                }
            }

            ((IClientChannel)chat).Close();
            factory.Close();

            Console.WriteLine("Chat beendet.");
        }
    }
}