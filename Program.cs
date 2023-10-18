using System.Data;
using System.IO;
using System;
using System.Net;
using System.Text;
using System.Threading.Channels;

namespace Лаба_по_проге_2
{
    struct Building
    {
        public string Name { get; set; } // название
        public string Type { get; set; } // тип
        public string City { get; set; } // город
        public string Architect { get; set; } // архитектор
        public int DateOfFound { get; set; } // дата постройки
    } // структура Здание

    class Program
    {
        static void Main()
        {
            const string path = @"/Users/aleksandrurupin/Documents/Код/Лабы второй курс/Программирование лабы/Лаба по проге 2/Лаба по проге 2";
            string filename = "SUIIIII";
            DirectoryInfo dirInfo = new DirectoryInfo(path);
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }
            Stack<Building> buildings = new Stack<Building>();
            bool programIsRunning = true;
            getFileName(ref filename);
            readData(buildings, path, filename);
            while (programIsRunning)
            {
                printMenu("base");
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.A:
                        bool building = true;
                        while (building)
                        {
                            printMenu("build");
                            switch (Console.ReadKey().Key)
                            {
                                case ConsoleKey.D1:
                                    buildings.Push(randomBuildingGeneration());
                                    break;
                                case ConsoleKey.D2:
                                    buildings.Push(setData());
                                    break;
                                case ConsoleKey.B:
                                    building = false;
                                    break;
                            }
                        }
                        break;
                    case ConsoleKey.C:
                        showStack(ref buildings);
                        break;
                    case ConsoleKey.O:
                        findTheOldest(buildings);
                        break;
                    case ConsoleKey.T:
                        findByType(buildings);
                        break;
                    case ConsoleKey.E:
                        programIsRunning = false;
                        writeData(buildings, path, filename);
                        Console.Clear();
                        break;
                        
                        
                }
            }
        }
        static void getFileName(ref string filename)
        {
            Console.WriteLine(
                "WAG1!\nType the filename(only letters, without any extensions) to open the file or if it doesn't exist to create the file");
            filename = Console.ReadLine() ?? throw new InvalidOperationException();
            while (!filename.All(char.IsLetter))
            {
                Console.WriteLine("Oh no! You entered the unallowed filename\nPlease try again");
                filename = Console.ReadLine() ?? throw new DataException();
            }
        } // получение имени файла
        static void readData(Stack<Building> buildings, string path, string filename)
        {
            if (!File.Exists($"{path}/{filename}.txt"))
            {
                StreamWriter writer = new StreamWriter($"{path}/{filename}.txt", true);
                writer.Dispose();
            }
            Building building = new Building();
            int spacer = 0;
            try
            {
                using (StreamReader reader = new StreamReader(@$"{path}/{filename}.txt"))
                {
                    string? line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        spacer++;
                        switch (spacer)
                        {
                            case 1:
                                building.Name = line;
                                break;
                            case 2:
                                building.Type = line;
                                break;
                            case 3:
                                building.City = line;
                                break;
                            case 4:
                                building.Architect = line;
                                break;
                            case 5:
                                building.DateOfFound = int.Parse(line);
                                break;
                        }

                        if (string.IsNullOrEmpty(line) && spacer > 5)
                        {
                            buildings.Push(building);
                            spacer = 0;
                            building = new Building();
                        }
                    }
                    if (spacer > 0)
                    {
                        buildings.Push(building);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        } // считывание из файла
        static void writeData(Stack<Building> buildings, string path, string filename)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter($"{path}/{filename}.txt", false))
                {
                    foreach (var building in buildings)
                    {
                        writer.WriteLine(building.Name);
                        writer.WriteLine(building.Type);
                        writer.WriteLine(building.City);
                        writer.WriteLine(building.Architect);
                        writer.WriteLine(building.DateOfFound);
                        writer.WriteLine();
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        } // запись в файл
        static void getData(Building building)
        {
            Console.Clear();
            Console.WriteLine("Press any button to go back");
            Console.WriteLine();
            Console.WriteLine($"Name: {building.Name}");
            Console.WriteLine($"Type: {building.Type}");
            Console.WriteLine($"City: {building.City}");
            Console.WriteLine($"Architect: {building.Architect}");
            Console.WriteLine($"Date of found: {building.DateOfFound}");
            Console.ReadKey();
        } // вывод данных здания
        static Building setData()
        {
            Console.Clear();
            Building building = new Building();
            Console.WriteLine("Let's make some mess!\nWhat is the name of the building?");
            building.Name = getString();
            Console.WriteLine("Oh, ok! Print the type of the building?");
            building.Type = getString();
            Console.WriteLine("Great! In what city is it?");
            building.City = getString();
            Console.WriteLine("Who is the architect?");
            building.Architect = getString();
            Console.WriteLine("When was the building found?");
            building.DateOfFound = getDate();
            Console.Clear();
            Console.WriteLine("Data was taken successfully\nPress any button to go back");
            Console.ReadKey();
            return building;
            
        } // считывание данных здания
        static void printMenu(string type)
        {
            switch (type)
            {
                case "base":
                    Console.Clear();
                    Console.WriteLine("Press A to add a building");
                    Console.WriteLine("Press C to get buildings catalogue");
                    Console.WriteLine("Press O to find the oldest building");
                    Console.WriteLine("Press T to see all specially typed buildings under 18 century");
                    Console.WriteLine("Press E to leave the program");
                    break;
                case "build":
                    Console.Clear();
                    Console.WriteLine("Press 1 to build a random building");
                    Console.WriteLine("Press 2 to build a non-random building");
                    Console.WriteLine("Press B to go back");
                    break;
            }
        } // вывод меню на консоль
        static Building randomBuildingGeneration()
        {
            Building building = new Building();
            Random rand = new Random();
            building.Name = randomStringBuilder();
            building.Architect = randomStringBuilder();
            building.DateOfFound = rand.Next(1, 2023);
            building.City = randomStringBuilder();
            building.Type = randomStringBuilder();
            Console.Clear();
            Console.WriteLine("You have built random building!\nPress any button to go back");
            Console.ReadKey();

            return building;
        } // генератор рандомных зданий
        static string randomStringBuilder()
        {
            var pullAlpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            StringBuilder stringBuilder = new StringBuilder();
            Random rand = new Random();
            for (int i = 0; i < rand.Next(5, 15); i++)
            {
                stringBuilder.Append(pullAlpha[rand.Next(51)]);
            }
            return stringBuilder.ToString();
        } // генератор рандомных строк
        static string getString()
        {
            bool loop = true;
            string? line;
            while (loop)
            {
                line = Console.ReadLine();
                if (line.All(c => !Char.IsDigit(c)))
                {
                    return line;
                }
                else
                {
                    Console.WriteLine("Oh no, cringe... This string has numbers!\nTry to print it again");
                }
            }

            return "UNNWN";

        } // считывание строки
        static int getDate()
        {
            bool loop = true;
            string? line;
            while (loop)
            {
                line = Console.ReadLine();
                if (int.TryParse(line, out int year))
                {
                    if (year is < 2024 and > 0)
                    {
                        return year;
                    }
                }
                Console.WriteLine("Oh no, cringe... This date is awful!\nTry to print it again");
            }

            return 777;
        } // считывание даты
        static void changeStackItem(ref Stack<Building> buildings, Building itemToChange)
        {
            Stack<Building> defaultStack = new Stack<Building>(buildings);
            Stack<Building> newStack = new Stack<Building>();
            while (defaultStack.Count > 0)
            {
                if (!defaultStack.Peek().Equals(itemToChange))
                {
                    newStack.Push(defaultStack.Pop());
                }
                else
                {
                    newStack.Push(setData());
                    defaultStack.Pop();
                }
            }
            buildings = newStack;
        } // изменение здания в стэке
        static void removeBuilding(ref Stack<Building> buildings, Building itemToremove)
        {
            Stack<Building> defaultStack = new Stack<Building>(buildings);
            Stack<Building> newStack = new Stack<Building>();
            while (defaultStack.Count > 0)
            {
                if (!defaultStack.Peek().Equals(itemToremove))
                {
                    newStack.Push(defaultStack.Pop());
                }
                else
                {
                    defaultStack.Pop();
                }
            }
            buildings = newStack;
        } // удаление здания из стека
        static void showStack(ref Stack<Building> buildings)
        {
            bool loop = true;
            if (buildings.Count == 0)
            {
                Console.Clear();
                Console.WriteLine("Sorry, you have no buildings to see\nPress any button to go back");
                Console.ReadKey();
                return;
            }
            Stack<Building> copy = new Stack<Building>(buildings);
            Building current = new Building();
            int i = 0;
            while (loop)
            {
                if (copy.Count < 1)
                {
                    copy = new Stack<Building>(buildings);
                }
                if (copy.Count == 0)
                {
                    Console.Clear();
                    Console.WriteLine("Stack is empty! Please add some items there\nPress any button to go back");
                    Console.ReadKey();
                    break;
                }
                Console.Clear();
                Console.WriteLine("--BUILDINGS CATALOGUE--");
                Console.WriteLine("Press N to see next building");
                Console.WriteLine("Press B to go back");
                Console.WriteLine();
                Console.WriteLine($"Type: {copy.Peek().Type} Name: {copy.Peek().Name}");
                current = copy.Pop();
                Console.WriteLine();
                Console.WriteLine("Press I to see all information about this building");
                Console.WriteLine("Press C to change building's info");
                Console.WriteLine("Press R to remove building");
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.N:
                        break;
                    case ConsoleKey.I:
                        getData(current);
                        break;
                    case ConsoleKey.C:
                        changeStackItem(ref buildings, current);
                        copy = new Stack<Building>(buildings);
                        break;
                    case ConsoleKey.R:
                        removeBuilding(ref buildings, current);
                        copy = new Stack<Building>(buildings);
                        break;
                    case ConsoleKey.B:
                        loop = false;
                        break;
                }
            }
        } // показать стек
        static void findTheOldest(Stack<Building> buildings)
        {
            Building theOldest = new Building();
            int minYear = 2024;
            foreach (Building item in buildings)
            {
                if (item.DateOfFound < minYear)
                {
                    theOldest = item;
                    minYear = item.DateOfFound;
                }
            }
            getData(theOldest);
        } // поиск самого старого здания
        static void findByType(Stack<Building> buildings)
        {
            Console.Clear();
            Console.WriteLine("What type do you want to check?");
            string type = getString();
            Console.Clear();
            bool wasFound = false;
            foreach (Building item in buildings)
            {
                if (item.DateOfFound < 1700 && item.Type == type)
                {
                    wasFound = true;
                    Console.WriteLine($"Name: {item.Name}");
                    Console.WriteLine($"Type: {item.Type}");
                    Console.WriteLine($"City: {item.City}");
                    Console.WriteLine($"Architect: {item.Architect}");
                    Console.WriteLine($"Date of found: {item.DateOfFound}");
                    Console.WriteLine();
                }
            }

            if (!wasFound)
            {
                Console.Clear();
                Console.WriteLine("There are no buildings that match the search criteria");
                Console.WriteLine();
            }
            Console.WriteLine("Press any button to go back");
            Console.ReadKey();
        } // поиск по типу
    }
}