using System;
using System.Configuration;
using System.IO;

namespace ToDoList
{
    class Menu
    {
        KBProcessor processor = new KBProcessor();

        public void display()
        {
            int userOption = 0;

            do
            {
                this.validateDBFile();

                this.displayFileInScreen();

                userOption = this.getUserOption();

                processor.processAction(userOption);
            }
            while (userOption != 99);
        }

        public void validateDBFile()
        {
            if (!this.fileExists())
            {
                this.fileCreate();
            }
        }

        public bool fileExists()
        {
            bool ret = true;

            if (!File.Exists(ConfigurationManager.AppSettings.Get("dbFile")))
            {
                ret = ret && false;
            }

            return ret;
        }

        public void fileCreate()
        {
            var stream = File.Create(ConfigurationManager.AppSettings.Get("dbFile"));
            stream.Close();

            if (!this.fileExists())
            {
                throw new FileLoadException("Error while creating file");
            }
        }

        public void displayFileInScreen()
        {
            Console.Clear();
            processor.processAction(0);
        }

        public int getUserOption()
        {
            int userOption = 0;

            processor.printSeparator();
            Console.WriteLine("\t\tWhat do you want to do?");
            processor.printSeparator();
            Console.WriteLine("\t\t (1) Add item");
            Console.WriteLine("\t\t (2) Move item");
            Console.WriteLine("\t\t (3) Edit item");
            Console.WriteLine("\t\t (4) Remove item");
            Console.WriteLine("\t\t (5) Clear file");
            Console.WriteLine("\t\t(99) Exit");
            int.TryParse(Console.ReadLine(), out userOption);

            return userOption;
        }
    }
}
