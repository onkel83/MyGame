using Core.Helper;
using Core.Model;
using System;

namespace Core.Manager
{
    public static class InputHandler
    {
        public static TestModel GetTestModelFromUser(TestModel? model = null)
        {
            var newModel = model ?? new TestModel();

            Console.Write("Enter Name: ");
            string name = Console.ReadLine();
            if (name == "/q") return newModel;
            newModel.Name = name;

            Console.Write("Enter Description: ");
            string description = Console.ReadLine();
            if (description == "/q") return newModel;
            newModel.Description = description;

            Console.Write("Enter Start Date (yyyy-MM-dd HH:mm): ");
            if (!ValidationHelper.ValidateDateTime(Console.ReadLine(), out DateTime start))
            {
                ConsoleHelper.WriteLine("Invalid date format. Please try again.", ConsoleColor.Red);
                return GetTestModelFromUser(model);
            }
            newModel.Start = start;

            Console.Write("Enter End Date (yyyy-MM-dd HH:mm): ");
            if (!ValidationHelper.ValidateDateTime(Console.ReadLine(), out DateTime end))
            {
                ConsoleHelper.WriteLine("Invalid date format. Please try again.", ConsoleColor.Red);
                return GetTestModelFromUser(model);
            }
            newModel.Ende = end;

            Console.Write("Enter Pause (in hours): ");
            if (!ValidationHelper.ValidateDecimal(Console.ReadLine(), out decimal pause))
            {
                ConsoleHelper.WriteLine("Invalid number format. Please try again.", ConsoleColor.Red);
                return GetTestModelFromUser(model);
            }
            newModel.Pause = pause;

            return newModel;
        }
    }
}
