using System;
using Core.Helper;

namespace Core.Manager
{
    public static class ApplicationReloader
    {
        public static void ReloadApplication()
        {
            TestModelOperations.repo.Shutdown();
            ConsoleHelper.WriteLine("An error occurred. Reloading application...", ConsoleColor.Red);
            System.Diagnostics.Process.Start(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            Environment.Exit(1);
        }
    }
}
