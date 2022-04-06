using System.Net;
using System.Net.Sockets;

namespace ServerClass
{
    internal static class MyServer
    {
        private static string ip = String.Empty;
        private static int port = 0;

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

        static private Socket ConnectSocket()
        {
            try
            {
                static Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                // связываем сокет с точкой сервера, по которой будем принимать данные
                static IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(ip), port);
                socket.Bind(ipPoint);

                if (socket.Connect)
                {
                    return socket;
                }
                else
                {
                    System.Console.WriteLine("Не получилось установить точку доступа! Проверьте настройки подключения...");
                    return null;
                }

            }
            catch (System.Exception message)
            {
                System.Console.WriteLine($"Ошибка подключения: {message}");
            }
        }
    }
    

    
}

