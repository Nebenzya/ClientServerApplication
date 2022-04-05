using System.Data.SqlClient; // устанавливается через диспетчер пакетов NuGet
using System.Text;
using System.Net;
using System.Net.Sockets;


Console.WriteLine("Добро пожаловать в настройки сервера CSA!\nВсе доступные команды можно узнать с помощью команды help");
while (true)
{
    Console.Write("Введите команду: ");
    CheckCommand(Console.ReadLine());
}


#region Socket
string ip = "127.0.0.1"; 
int port = 8080;

IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(ip), port);
Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
#endregion // Socket


void CheckCommand(string command)
{
    switch (command.ToLower())
    {
        case ("help"):
            Console.WriteLine("Доступные команды: \nhelp - список доступных команд" +
                              "\nset ip - устанавливаем IP адресс для сервера" +
                              "\nset port - устанавливаем порт для сервера" +
                              "\nexit - завершить программу CSA");
            break;
        case ("set ip"):
            Console.Write("Введите IP адресс: ");
            ip = Console.ReadLine();
            break;
        case ("set port"):
            Console.Write("Введите порт: ");
            port = Int32.Parse(Console.ReadLine());
            break;

        case ("exit"):
            Environment.Exit(0);
            break;
        default:
            Console.WriteLine($"Неизвестная команда \"{command}\". Повторите запрос...");
            break;
    }
}
