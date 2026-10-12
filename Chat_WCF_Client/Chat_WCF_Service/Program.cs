using System;
using System.ServiceModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat_WCF_Service
{
    // SERVICE
    [ServiceContract]
    public interface IChat
    {
        [OperationContract] string WhichServiceAmI();
        [OperationContract] void SendMessage(string message);
        [OperationContract] string ReceiveMessages(ref int id);
    }

    public class Mathe : ChatInterface
    {
        public double Add(double a, double b)
        {
            return a + b;
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            const string URL = "http://localhost:2310";

            Console.WriteLine("WCF-Service - 5AHEL - Golubic");
            Console.WriteLine("URL: " + URL);
            Console.Write("Starting...");

            ServiceHost myService = new ServiceHost(typeof(Mathe));

            myService.AddServiceEndpoint(
                typeof(IMathe),
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
