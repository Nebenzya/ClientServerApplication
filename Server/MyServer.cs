using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;

namespace Server
{
    internal static class MyServer
    {
        private static Socket? _socket;
        private static string _ip = "127.0.0.1";
        private static int _port = 8080;
        private static List<Student>? _students;

        public static string IP
        {
            get => _ip;
            set
            {
                if (IPAddress.TryParse(value, out _))
                {
                    _ip = value;
                    Console.WriteLine($"IP адресс успешно изменён на: {_ip}\n");
                }
                else
                    Console.WriteLine($"IP адресс не удалось изменить на: {value}\n");
            }
        }

        public static int Port
        {
            get => _port;
            set
            {
                if (0 <= value && value < 65536)
                {
                    _port = value;
                    Console.WriteLine($"Порт успешно изменён на: {_port}\n");
                }
                else 
                    Console.WriteLine("Неверный значение для порта. Корректное значение в диапазоне 0-65535\n");
            }
        }

        static public void Info()
        {
            Console.WriteLine($"ip:\t{IP}\nport:\t{Port}\n");
        }

        static private void ConnectSocket()
        {
            _socket = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ipPoint = new(IPAddress.Parse(_ip), _port);
            _socket.Bind(ipPoint);
        }

        static public void Start(int queue)
        {
            try
            {
                ConnectSocket();

                if (_socket != null && !(_socket.Connected))
                {
                    _socket.Listen(queue);
                    Console.WriteLine("Сервер запущен. Ожидание подключения пользователя...");
                    ListenTo();
                }                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка на этапе подключения сервера: {ex.Message}");
            }
        }

        static private void ListenTo()
        {
            try
            {
                while (true)
                {
                    Socket socketForAccept = _socket.Accept();

                    var message = new StringBuilder();
                    byte[] buffer = new byte[256];
                    do
                    {
                        int countBytes = socketForAccept.Receive(buffer);
                        message.Append(Encoding.Unicode.GetString(buffer, 0, countBytes));
                    }
                    while (socketForAccept.Available > 0);
                    Console.WriteLine($"{DateTime.Now.ToLongTimeString()}: {message}");


                    byte[] size = new byte[4];
                    switch (message.ToString())
                    {
                        case ("connect"):

                            // TODO вынести в отдельный метод

                            _students = SqliteConnecter.Load();
                            buffer = JsonSerializer.SerializeToUtf8Bytes(_students);
                            size = BitConverter.GetBytes(buffer.Length);
                            socketForAccept.Send(size);
                            socketForAccept.Send(buffer);
                            break;
                        case ("save"):
                            // Этап согласованно получения данных:
                            // 1. получение информации о размере данных (одно целое число типа Int32)
                            socketForAccept.Receive(size);

                            // 2. получения данных в сериализованном виде (коллекция типа List<Student>)
                            byte[] data = new byte[BitConverter.ToInt32(size, 0)];
                            socketForAccept.Receive(data);

                            if (BitConverter.ToInt32(size, 0) > 0)
                            {
                                _students = JsonSerializer.Deserialize<List<Student>>(data);
                                foreach (Student? item in _students)
                                {
                                    SqliteConnecter.Save(item);
                                }
                            }
                            break;
                        default:
                            break;
                    }
                    socketForAccept.Shutdown(SocketShutdown.Both);
                    socketForAccept.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка на этапе прослушивания: {ex.Message}");
            }
        }
    }
}

