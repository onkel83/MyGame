using System;
using System.Collections.Generic;
using Core.Helper;

namespace Core.Manager
{
    public static class MenuManager
    {
        private static bool _workTime = false;
        private static bool _worker = false;
        private static bool _event = false;
        private static bool _search = false;
        private static bool _topMenue = true;

        public static string MenuTitle { get; set; } = string.Empty;

        public static bool Worker { get => _worker; set => _worker = value; }
        public static bool WorkTime { get => _workTime; set => _workTime = value; }
        public static bool Event { get => _event; set => _event = value; }
        public static bool Search { get => _search; set => _search = value; }

        public static void DisplayMenu()
        {
            _topMenue = true;
            while (_topMenue)
            {
                Console.Clear();
                var TopMenu = new Dictionary<char, (string description, Action action)>
                {
                    { '1', (ConfigHelper.GetConfigValue("MenuConfig", "AMTitle","[Ar]beitszeit - [V]erwaltung"), DisplayWorkTimeMenu)},
                    { '2', (ConfigHelper.GetConfigValue("MenuConfig", "MMTitle","[Mi]tarbeiter - [V]erwaltung"), DisplayWorkerMenu)},
                    { '3', (ConfigHelper.GetConfigValue("MenuConfig", "EMTitle","[Au]ftrag's   - [V]erwaltung"), DisplayEventMenu)},
                    { '9', (ConfigHelper.GetConfigValue("MenuConfig", "HMNote0","[Ei]nstellungen   (gespeert)"), DisplaySeetings)},
                    { '0', (ConfigHelper.GetConfigValue("MenuConfig", "GExit"  ,"[Be]enden")                   , ShutDown)}
                };

                ConsoleMenuHelper.DisplayMenu(ConfigHelper.GetConfigValue("MenuConfig", "HMTitle", "[W]ork[T]ime[A]ttach - [M]ain[M]enu"), TopMenu);
            }
            ShutDown();
        }
        private static void DisplayWorkTimeMenu()
        {
            WorkTime = true;
            while (WorkTime)
            {
                Console.Clear();
                var menuItems = new Dictionary<char, (string description, Action action)>
                {
                    { '1', (ConfigHelper.GetConfigValue("MenuConfig", "AMNote1","[Ar]beitszeit      hinzufügen"), TestModelOperations.CreateTestModel) },
                    { '2', (ConfigHelper.GetConfigValue("MenuConfig", "AMNote2","[Ar]beitszeit      bearbeiten"), TestModelOperations.UpdateTestModel) },
                    { '3', (ConfigHelper.GetConfigValue("MenuConfig", "AMNote3","[Ar]beitszeit        löschen"), TestModelOperations.DeleteTestModel) },
                    { '4', (ConfigHelper.GetConfigValue("MenuConfig", "AMNote4","[Ar]beitszeit       anzeigen"), TestModelOperations.Search) },
                    { '0', (ConfigHelper.GetConfigValue("MenuConfig", "GBack"  ,"[Z]urück [z]um [M]ain[M]enue"), TestModelOperations.Exit) }
                };

                ConsoleMenuHelper.DisplayMenu(ConfigHelper.GetConfigValue("MenuConfig", "AMTitle", "[Ar]beitszeit - [V]erwaltung"), menuItems);
            }
        }
        public static void DisplayShowAndSearch(Dictionary<char, (string description, Action action)> menuItems)
        {
            Search = true;
            while (Search)
            {
                Console.Clear();
                ConsoleMenuHelper.DisplayMenu(MenuTitle, menuItems);
                Console.ReadLine();
            }
        }
        private static void DisplayWorkerMenu()
        {
            Worker = true;
            while (Worker)
            {
                Console.Clear();
                var menuItems = new Dictionary<char, (string description, Action action)>
                {
                    { '1', (ConfigHelper.GetConfigValue("MenuConfig", "MMNote1", "[Mi]tarbeiter hinzufügen"), TestModelOperations.CreateTestModel) },
                    { '2', (ConfigHelper.GetConfigValue("MenuConfig", "MMNote2", "[Mi]tarbeiter hinzufügen"), TestModelOperations.ReadTestModelById) },
                    { '3', (ConfigHelper.GetConfigValue("MenuConfig", "MMNote3", "[Mi]tarbeiter hinzufügen"), TestModelOperations.ReadAllTestModels) },
                    { '4', (ConfigHelper.GetConfigValue("MenuConfig", "MMNote4", "[Mi]tarbeiter hinzufügen"), TestModelOperations.UpdateTestModel) },
                    { '0', (ConfigHelper.GetConfigValue("MenuConfig", "GBack","[Z]urück [z]um [M]ain[M]enue"), TestModelOperations.Exit) }
                };

                ConsoleMenuHelper.DisplayMenu(ConfigHelper.GetConfigValue("MenuConfig", "MMTitle", "[Mi]tarbeiter - [V]erwaltung"), menuItems);
            }
        }
        private static void DisplayEventMenu()
        {
            Event = true;
            while (Event)
            {
                Console.Clear();
                var menuItems = new Dictionary<char, (string description, Action action)>
                {
                    { '1', (ConfigHelper.GetConfigValue("MenuConfig", "MMNote1", "[Au]ftrag hinzufügen"), TestModelOperations.CreateTestModel) },
                    { '2', (ConfigHelper.GetConfigValue("MenuConfig", "MMNote2", "[Au]ftrag hinzufügen"), TestModelOperations.ReadTestModelById) },
                    { '3', (ConfigHelper.GetConfigValue("MenuConfig", "MMNote3", "[Au]ftrag hinzufügen"), TestModelOperations.ReadAllTestModels) },
                    { '4', (ConfigHelper.GetConfigValue("MenuConfig", "MMNote4", "[Au]ftrag hinzufügen"), TestModelOperations.UpdateTestModel) },
                    { '0', (ConfigHelper.GetConfigValue("MenuConfig", "GBack","[Z]urück [z]um [M]ain[M]enue"), TestModelOperations.Exit) }
                };

                ConsoleMenuHelper.DisplayMenu(ConfigHelper.GetConfigValue("MenuConfig", "EMTitle", "[Au]ftrag's  -  [V]erwaltung"), menuItems);
            }
        }
        private static void DisplaySeetings()
        {
            Console.WriteLine("You Will be warnt!");
            Console.ReadLine();
            ShutDown();
        }

        private static void ShutDown()
        {
            TestModelOperations.Exit();
            Environment.Exit(0);
        }
        public static void Back()
        {
            if (_topMenue&&Search) { Search = false; if (Event) { DisplayEventMenu(); } else if (Worker) { DisplayWorkerMenu(); } else if (WorkTime) { DisplayWorkTimeMenu(); } }
        }
    }
}
