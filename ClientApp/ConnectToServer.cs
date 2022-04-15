using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Windows;
using System.Text.Json;
using System;

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
        private static Socket _host;

        #region Public:
        public static string IP
        {
            get { return _ip; }
            set
            {
                if (IPAddress.TryParse(value, out _))
                {
                    _ip = value;
                    _isCorrect = true;
                }
                else
                {
                    MessageBox.Show("Неверно указан IP-адрес!");
                    _isCorrect = false;
                }

            }
        }

        public static string Port
        {
            get { return _port.ToString(); }
            set
            {
                if (int.TryParse(value, out _))
                {
                    _port = int.Parse(value);
                    _isCorrect = true;
                }
                else
                {
                    MessageBox.Show("Неверно указан порт!");
                    _isCorrect = false;
                }

            }
        }

        public static bool IsCorrect { get => _isCorrect; }

        /// <summary>
        /// Обрабатывает поступаемую информацию от сервера. Возвращает пустой список, если данные не были получены
        /// </summary>
        public static List<Student> ReceiveData(string message)
        {
            SendMessage(message);

            // Этап согласованно приёма данных:
            // 1. получение информации о размере данных (одно целое число типа Int32)
            byte[] size = new byte[4];
            _host.Receive(size);

            // 2. получение данных в сериализованном виде (коллекция типа List<Student>)
            byte[] data = new byte[BitConverter.ToInt32(size, 0)];
            _host.Receive(data);

            if (_host.Connected)
            {
                _host.Shutdown(SocketShutdown.Both);
                _host.Close();
            }

            if (BitConverter.ToInt32(size, 0) > 0)
                return JsonSerializer.Deserialize<List<Student>>(data);
            else
                return new List<Student>();
        }

        /// <summary>
        /// Отправляет текущие данные на сервер
        /// </summary>
        public static void SendDate(ref List<Student> students, string message)
        {
            SendMessage(message);

            // Этап согласованной отправки данных:
            // 1. отправляем информацию о размере данных (одно целое число типа Int32)
            byte[] buffer = JsonSerializer.SerializeToUtf8Bytes(students);
            byte[] size = BitConverter.GetBytes(buffer.Length);
            _host.Send(size);
            // 2. отправка данных в сериализованном виде (коллекция типа List<Student>)
            _host.Send(buffer);

            if (_host.Connected)
            {
                _host.Shutdown(SocketShutdown.Both);
                _host.Close();
            }
        }
        #endregion // Public

        #region Private:

        /// <summary>
        /// Отправляет команду на сервер
        /// </summary>
        private static void SendMessage(string message)
        {
            ToConnect();

            if (_host != null)
            {
                byte[] data = Encoding.Unicode.GetBytes(message);
                _host.Send(data);
            }
        }

        /// <summary>
        /// Устанавливает соединение с сервером и уведомляет о результате
        /// </summary>
        private static void ToConnect()
        {
            if (IsCorrect)
            {
                IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(_ip), _port);
                _host = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                _host.Connect(ipPoint);
                MessageBox.Show("Соединение установлено!");
            }
        }
        #endregion // Private
    }
}