using System.Text;
using System.Net;
using System.Net.Sockets;

namespace Server
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
                IPAddress forCheck = null; // заглушка для метода TryParse

                if (IPAddress.TryParse(value,out forCheck))
                {
                    ip = value;
                    Console.WriteLine($"IP адресс успешно изменён на: {ip}\n");
                }
                else
                    Console.WriteLine($"IP адресс не удалось изменить на: {value}\n");
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
                else 
                    Console.WriteLine("Неверный значение для порта. Корректное значение в диапазоне 0-65535\n");
            }
        }

        static public void Info()
        {
            Console.WriteLine($"ip:\t{IP}\nport:\t{Port}\n");
        }

        static private Socket ConnectSocket()
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            // связываем сокет с локальной точкой сервера, по которой будем принимать данные
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

        // запускаем сервер и ставим в режим прослушивания
        static public void Start(int queue)
        {
            try
            {
                Socket socket = ConnectSocket();

                if (socket != null)
                {
                    // начинаем прослушивание с ограничением очереди
                    socket.Listen(queue);
                    Console.WriteLine("Сервер запущен. Ожидание подключения пользователя...");
                    ListenTo(socket);
                }                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка на этапе подключения сервера: {ex.Message}");
            }
        }

        static private void ListenTo(Socket socket)
        {
            try
            {
                while (true)
                {
                    // режим ожидания нового подключения
                    Socket listener = socket.Accept();

                    var message = new StringBuilder(); // для приёма сообщений
                    byte[] buffer = new byte[256]; // буфер для получаемых данных

                    do
                    {
                        int bytes = listener.Receive(buffer); // количество полученных байтов
                        message.Append(Encoding.Unicode.GetString(buffer, 0, bytes)); // принимаем информацию в виде байтовой последовательности
                    }
                    while (listener.Available > 0);
                    Console.WriteLine($"{DateTime.Now.ToLongTimeString()}: {message.ToString()}");

                    listener.Send(Encoding.Unicode.GetBytes("Сообщение получено")); // отправляем результат

                    // закрываем и отключаем соединение
                    listener.Shutdown(SocketShutdown.Both);
                    listener.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка на этапе прослушивания: {ex.Message}");
            }
        }
    }
}

