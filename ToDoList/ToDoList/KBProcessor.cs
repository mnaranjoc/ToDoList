using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;

namespace ToDoList
{
    class KBProcessor
    {
        KBItem[] items;

        public void processAction(int action)
        {
            this.rereadFile();

            switch (action)
            {
                case 0:
                    this.displayFileInScreen();
                    break;
                case 1:
                    this.createItem();
                    break;
                case 2:
                    this.moveItem();
                    break;
                case 3:
                    this.editItem();
                    break;
                case 4:
                    this.deleteItem();
                    break;
                case 5:
                    this.clearFile();
                    break;
                default: break;
            }

            this.updateFile();
        }

        public void rereadFile()
        {
            try
            {
                using (FileStream stream = File.OpenRead(ConfigurationManager.AppSettings.Get("dbFile")))
                {
                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(KBItem[]));
                    items = (KBItem[])serializer.ReadObject(stream);
                    Array.Sort(items);
                }
            }
            catch (Exception ex)
            {
                // log error
            }
        }

        public void updateFile()
        {
            this.deleteOldfile();

            using (FileStream stream = File.Create(ConfigurationManager.AppSettings.Get("dbFile")))
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(KBItem[]));
                serializer.WriteObject(stream, items);
            }
        }

        public void createItem()
        {
            List<KBItem> newItems = new List<KBItem>();

            if (items != null && items.Count() > 0)
                foreach (var item in items)
                {
                    newItems.Add(item);
                }

            int nextItemId = (items == null) ? 1 : items.Count() + 1;
            Console.WriteLine("Please enter the item description");
            string description = Console.ReadLine();

            newItems.Add(new KBItem() { Id = nextItemId, Description = description, Status = 1 });

            items = newItems.ToArray();
        }

        public KBItem getItem()
        {
            int item2Edit;
            int.TryParse(Console.ReadLine(), out item2Edit);

            KBItem item = null;

            if (items != null &&
                items.Count() > 0 &&
                items.Where(x => x.Id == item2Edit).Any())
            {
                item = items.Where(x => x.Id == item2Edit).First();
            }

            return item;
        }

        public void moveItem()
        {
            Console.WriteLine("Input the item number to move:");
            KBItem item = this.getItem();

            if (item != null)
            {
                int newStatus;

                do
                {
                    Console.WriteLine("Select the new status\t(1) TODO, (2) IN PROCESS, (3) DONE");
                    int.TryParse(Console.ReadLine(), out newStatus);
                }
                while (newStatus < 0 || newStatus > 3); // status should be 1, 2 or 3

                foreach (var localItem in items)
                {
                    if (localItem.Id == item.Id)
                    {
                        localItem.Status = newStatus;
                    }
                }
            }
        }

        public void editItem()
        {
            Console.WriteLine("Input the item number to edit:");
            KBItem item = this.getItem();

            if (item != null)
            {
                foreach (var localItem in items)
                {
                    if (localItem.Id == item.Id)
                    {
                        Console.WriteLine("Please set the new description: ");
                        localItem.Description = Console.ReadLine();
                    }
                }
            }
        }

        public void deleteItem()
        {
            Console.WriteLine("Input the item number to REMOVE:");
            KBItem item = this.getItem();

            if (item != null)
            {
                items = items.Where(x => x.Id != item.Id).ToArray();
            }
        }

        public void clearFile()
        {
            Console.WriteLine("Are you sure you want to CLEAR the file?\n(y) To delete\n(any key) to cancel");
            string userInput = Console.ReadLine();
            if (userInput.Equals("y") || userInput.Equals("Y"))
            {
                this.deleteOldfile();
                items = new KBItem[0];
            }
        }

        public void displayFileInScreen()
        {
            printSection("TODO", 1);
            printSection("IN PROCESS", 2);
            printSection("DONE", 3);
        }

        public void printSeparator()
        {
            Console.WriteLine("---------------------------------------------");
        }

        public void printSection(string name, int status)
        {
            Console.WriteLine(name);
            printSeparator();

            if (items == null)
            {
                Console.WriteLine("()");
            }
            else
            {
                var sectionItems = items.Where(x => x.Status == status);

                if (sectionItems.Count() == 0)
                {
                    Console.WriteLine("()");
                }
                else
                {
                    foreach (var item in sectionItems)
                    {
                        Console.WriteLine(item.ToString());
                    }
                }
            }

            Console.WriteLine();
        }

        public void deleteOldfile()
        {
            if (File.Exists(ConfigurationManager.AppSettings.Get("dbFile")))
            {
                File.Delete(ConfigurationManager.AppSettings.Get("dbFile"));
            }
        }
    }
}
