using System.Text;
using System.Net;
using System.Net.Sockets;

namespace ClientApp
{
    public class ConnectToServer
    {
        // адрес и порт сервера, к которому будем подключаться
        private static int port = 8080;
        private static string ip = "127.0.0.1";
        

        private static Socket Connect()
        {
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(ipPoint);
            return socket;
        }

        public static void SendMessage(string message)
        {
            Socket socket = Connect();

            // отправляем запрос
            byte[] data = Encoding.Unicode.GetBytes(message);
            socket.Send(data);

            string answer = ReceiveMessage(socket);

            // TODO использовать ответ сервера

            // закрываем и отключаем сокет
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
        }

        private static string ReceiveMessage(Socket socket)
        {
            var data = new byte[256];
            var buffer = new StringBuilder();

            do
            {
                int bytes = socket.Receive(data, data.Length, 0); // количество полученных байт
                buffer.Append(Encoding.Unicode.GetString(data, 0, bytes)); // принимаем информацию в виде байтовой последовательности
            }
            while (socket.Available > 0);

            return buffer.ToString();
        }
    }
}
