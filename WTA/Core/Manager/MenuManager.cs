using System;
using System.Collections.Generic;
using Core.Helper;

namespace Core.Manager
{
    public static class MenuManager
    {
        public static void DisplayMenu()
        {
            while (true)
            {
                var menuItems = new Dictionary<char, (string description, Action action)>
                {
                    { '1', ("Create TestModel", TestModelOperations.CreateTestModel) },
                    { '2', ("Read TestModel by ID", TestModelOperations.ReadTestModelById) },
                    { '3', ("Read All TestModels", TestModelOperations.ReadAllTestModels) },
                    { '4', ("Update TestModel", TestModelOperations.UpdateTestModel) },
                    { '5', ("Delete TestModel", TestModelOperations.DeleteTestModel) },
                    { '6', ("Search TestModels", TestModelOperations.SearchTestModels) },
                    { '7', ("Exit", TestModelOperations.Exit) }
                };

                ConsoleMenuHelper.DisplayMenu("TestModel Management Menu", menuItems);
            }
        }
    }
}
