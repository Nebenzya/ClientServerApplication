using System.Data.SqlClient; // устанавливается через диспетчер пакетов NuGet
using ServerClass;

Console.WriteLine("Добро пожаловать в настройки сервера CSA!\nВсе доступные команды можно узнать с помощью команды help\n\n");
while (true)
{
    Console.Write("Введите команду: ");
    CheckCommand(Console.ReadLine());
}

//обрабатывает ввод пользователя на соответствие команд
void CheckCommand(string command)
{
    switch (command.ToLower())
    {
        case ("help"):
            Console.WriteLine("Доступные команды: \nhelp - список доступных команд" +
                              "\nset ip/port - устанавливаем значение для конкретных полей" +
                              "\nexit - завершить программу CSA\n");
            break;
        case ("set ip"):
            Console.Write("Введите IP адресс: ");
            ConnectSoket.IP = Console.ReadLine();
            break;
        case ("set port"):
            Console.Write("Введите порт: ");
            ConnectSoket.Port = Int32.Parse(Console.ReadLine());
            break;
        case ("exit"):
            Environment.Exit(0);
            break;
        default:
            Console.WriteLine($"Неизвестная команда \"{command}\". Повторите запрос...\n");
            break;
    }
}
