using Server;

Console.WriteLine("Добро пожаловать в настройки сервера CSA!\nВсе доступные команды можно узнать с помощью команды help\n\n");
while (true)
{
    Console.Write("Введите команду: ");
    CheckCommand(Console.ReadLine());
}

void CheckCommand(string command)
{
    switch (command.ToLower())
    {

        #region forDebug
        //case ("load"):
        //    var list = SqliteConnecter.Load();
        //    foreach (var item in list)
        //    {
        //        Console.WriteLine($"{item.FullInfo}");
        //    }
        //    break;
        //case ("save"):
        //    var stud = new Student() { FirstName = "Maria", LastName = "Craft", BirthYear = 2001, Course = 1 };
        //    SqliteConnecter.Save(stud);
        //    break;
        #endregion // forDebug

        case ("help"):
            Console.WriteLine("Доступные команды: \nhelp - список доступных команд" +
                              "\ninfo - узнать текущие параметры сервера" +
                              "\nset ip/port - устанавливаем значение для конкретных полей" +
                              "\nstart - запустить сервер" +
                              "\nexit - завершить программу CSA\n");
            break;
        case ("info"):
            MyServer.Info();
            break;
        case ("set ip"):
            Console.Write("Введите IP адресс: ");
            MyServer.IP = Console.ReadLine();
            break;
        case ("set port"):
            Console.Write("Введите порт: ");
            MyServer.Port = Int32.Parse(Console.ReadLine());
            break;
        case ("exit"):
            Environment.Exit(0);
            break;
        case ("start"):
            MyServer.Start(1);
            break;
        default:
            Console.WriteLine($"Неизвестная команда \"{command}\". Повторите запрос...\n");
            break;
    }
}
