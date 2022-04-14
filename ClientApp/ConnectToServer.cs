using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Windows;
using System.Text.Json;
using System.ComponentModel;

namespace ClientApp
{
    /// <summary>
    /// Устанавливает подключение к серверу через Socket
    /// </summary>
    public static class ConnectToServer
    {
        private static int _port = 8080;
        private static string _ip = "127.0.0.1";
        private static bool _isCorrect = true;

        #region Public:
        public static string IP
        {
            get { return _ip; }
            set 
            {
                IPAddress forCheck;
                if (IPAddress.TryParse(value, out forCheck))
                {
                    _ip = value;
                }
                else
                {
                    MessageBox.Show("Неверно указан IP-адрес!");
                    IsCorrect = false;
                }

            }
        }

        public static string Port
        {
            get { return _port.ToString(); }
            set 
            {
                int x;
                if (int.TryParse(value, out x))
                {
                    _port = int.Parse(value);
                }
                else
                {
                    MessageBox.Show("Неверно указан порт!");
                    IsCorrect = false;
                }
                 
            }
        }

        public static bool IsCorrect { get => _isCorrect; set => _isCorrect = value; }

        public static void SendMessage(string message,ref BindingList<Student> list )
        {
            try
            {
                Socket socket = Connect();

                if (socket != null)
                {
                    byte[] data = Encoding.Unicode.GetBytes(message);
                    socket.Send(data);

                    list = ReceiveMessage(socket);

                    socket.Shutdown(SocketShutdown.Both);
                    socket.Close();
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion // Public

        #region Private:
        private static BindingList<Student> ReceiveMessage(Socket socket)
        {
            byte[] data = new byte[256];

            int bytes = socket.Receive(data, data.Length, 0);


            // TODO переработать прежде чем разкомментировать

            //if (bytes > 0)
            //    return JsonSerializer.Deserialize<BindingList<Student>>(data);
            //else
                return null;
        }

        private static Socket Connect()
        {
            if (IsCorrect)
            {
                IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(_ip), _port);
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.Connect(ipPoint);
                MessageBox.Show("Соединение установлено!");
                return socket;
            }
            else
            {
                MessageBox.Show("Соединение не установлено!");
                return null;
            }
        }
        #endregion // Private
    }
}
