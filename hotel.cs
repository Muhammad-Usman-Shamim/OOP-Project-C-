using System;

namespace MakkahHotel
{
    class MenuItem
    {
        public string Name { get; private set; }
        public int Price { get; private set; }

        public MenuItem(string name, int price)
        {
            Name = name;
            Price = price;
        }

        public virtual void Display()
        {
            Console.WriteLine($"{Name} - Rs. {Price}");
        }
    }

    class FoodItem : MenuItem
    {
        public FoodItem(string name, int price) : base(name, price) { }

        public virtual double CalculateCost(int quantity)
        {
            return Price * quantity;
        }
    }

    class FoodItemWithOptions : FoodItem
    {
        public string[] Options { get; private set; }

        public FoodItemWithOptions(string name, int price, string[] options)
            : base(name, price)
        {
            Options = options;
        }

        public override void Display()
        {
            Console.WriteLine($"\n{this.Name} - Rs. {this.Price}");
            Console.WriteLine("Available Options:");
            for (int i = 0; i < Options.Length; i++)
            {
                Console.WriteLine($"  {i + 1}. {Options[i]}");
            }
        }

        public string GetOptionName(int index)
        {
            if (index >= 1 && index <= Options.Length)
            {
                return Options[index - 1];
            }
            return null;
        }
    }

    class Hotel
    {
        private const string HotelName = "Makkah Hotel";
        private const int GSTRate = 18;

        private static FoodItemWithOptions[] menuItems = {
            new FoodItemWithOptions("SAMOSA", 30, new[] { "Aloo Samosa", "Chicken Samosa" }),
            new FoodItemWithOptions("ROLL", 40, new[] { "Chicken Roll", "Beef Roll" }),
            new FoodItemWithOptions("CHICKEN BIRYANI", 200, new[] { "Chicken", "Beef" }),
            new FoodItemWithOptions("PULAO", 200, new[] { "Chicken", "Beef" }),
            new FoodItemWithOptions("PARATHA", 50, new[] { "Plain", "Cheese", "Aloo", "Chicken Cheese" }),
            new FoodItemWithOptions("OMELETTE", 50, new[] { "Simple", "Cheese" }),
            new FoodItemWithOptions("SHAWARMA", 170, new[] { "Chicken", "Beef" }),
            new FoodItemWithOptions("COLDRINK", 100, new[] { "Pakola", "Cola Next", "Pepsi", "7 Up" })
        };

        static void Main(string[] args)
        {
            ProcessOrder();
        }

        private static void ProcessOrder()
        {
            double grandTotal = 0;

            while (true)
            {
                Console.Clear();
                ShowMenu();

                int choice = GetValidIntInput($"\nEnter your choice (1 - {menuItems.Length}): ", 1, menuItems.Length);

                FoodItemWithOptions selectedItem = menuItems[choice - 1];

                selectedItem.Display();

                int optionChoice = GetValidIntInput("Select option number: ", 1, selectedItem.Options.Length);
                string selectedOption = selectedItem.GetOptionName(optionChoice);

                int quantity = GetValidIntInput("Enter quantity: ", 1, 100);

                double subtotal = selectedItem.CalculateCost(quantity);
                double gst = subtotal * GSTRate / 100;
                double total = subtotal + gst;

                grandTotal += total;

                ShowOrderSummary(selectedItem.Name, selectedOption, quantity, subtotal, gst, total);

                Console.Write("\nDo you want to order another item? (yes/no): ");
                string answer = Console.ReadLine()?.ToLower();
                if (answer != "yes") break;
            }

            Console.WriteLine($"\n🧾 Grand Total: Rs. {grandTotal}");
            Console.WriteLine("Thank you for ordering from Makkah Hotel!");
            Console.WriteLine(new string('=', 40));
        }

        private static void ShowMenu()
        {
            Console.WriteLine(new string('=', 50));
            Console.WriteLine($"WELCOME TO {HotelName.ToUpper()}");
            Console.WriteLine(new string('=', 50));

            for (int i = 0; i < menuItems.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {menuItems[i].Name} - Rs. {menuItems[i].Price}");
            }

            Console.WriteLine(new string('-', 50));
        }

        private static int GetValidIntInput(string prompt, int min, int max)
        {
            int result;
            bool valid;

            do
            {
                Console.Write(prompt);
                string input = Console.ReadLine();
                valid = int.TryParse(input, out result) && result >= min && result <= max;

                if (!valid)
                {
                    Console.WriteLine($"⚠️  Please enter a number between {min} and {max}.");
                }
            }
            while (!valid);

            return result;
        }

        private static void ShowOrderSummary(string itemName, string option, int quantity, double subtotal, double gst, double total)
        {
            Console.WriteLine("\n📦 Order Summary:");
            Console.WriteLine($"Item       : {itemName}");
            Console.WriteLine($"Option     : {option}");
            Console.WriteLine($"Quantity   : {quantity}");
            Console.WriteLine($"Subtotal   : Rs. {subtotal}");
            Console.WriteLine($"GST ({GSTRate}%) : Rs. {gst}");
            Console.WriteLine($"Total      : Rs. {total}");
        }
    }
}