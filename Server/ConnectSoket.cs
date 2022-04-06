using System.Net;
using System.Net.Sockets;

namespace ServerClass
{
    internal static class ConnectSoket
    {
        private static string ip = String.Empty;
        private static int port = 0;
        private static Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        public static string IP
        {
            get => ip;
            set
            {
                ip = value;
                Console.WriteLine($"IP адресс успешно изменён на: {ip}\n");
            }
        }

        public static int Port
        {
            get => port;
            set
            {
                if (0 <= value && value < 65536)
                {
                    port = value;
                    Console.WriteLine($"Порт успешно изменён на: {ip}\n");
                }
                else Console.WriteLine("Неверный значение для порта. Корректное значение в диапазоне 0-65535\n");

            }
        }
        //static IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(ip), port);
    }
    

    
}

