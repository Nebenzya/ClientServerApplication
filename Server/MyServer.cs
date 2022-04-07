using System.Net;
using System.Net.Sockets;

namespace ServerClass
{
    internal static class MyServer
    {
        private static string ip = "127.0.0.1";
        private static int port = 8080;

        public static string IP
        {
            get => ip;
            set
            {
                IPAddress forCheck = null;
                if (IPAddress.TryParse(value,out forCheck))
                {
                    ip = value;
                    Console.WriteLine($"IP адресс успешно изменён на: {ip}\n");
                }
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
                    Console.WriteLine($"Порт успешно изменён на: {port}\n");
                }
                else Console.WriteLine("Неверный значение для порта. Корректное значение в диапазоне 0-65535\n");

            }
        }

        static private Socket ConnectSocket()
        {
            try
            {
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                // связываем сокет с точкой сервера, по которой будем принимать данные
                IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(ip), port);
                socket.Bind(ipPoint);

                if (socket != null)
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
                System.Console.WriteLine($"Ошибка подключения: {message}\n");
                return null;
            }
        }
        static public void Start(int queue)
        {
            Socket socket = ConnectSocket();
            
            try
            {
                if (socket != null)
                {
                    socket.Listen(queue);
                    Console.WriteLine("Сервер запущен. Ожидание подключения пользователя...");
                    while (true)
                    {
                        socket.Accept();

                        socket.Shutdown(SocketShutdown.Both);
                        socket.Close();
                    }
                }
            }
            catch (Exception message)
            {
                Console.WriteLine(message);
            }
            

        }
    }
    
}

