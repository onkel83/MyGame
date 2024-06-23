
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Helper
{
    public static class ThreadHelper
    {
        public static void RunTask(Action action, CancellationToken token)
        {
            Task.Run(() =>
            {
                try
                {
                    action();
                }
                catch (Exception ex)
                {
                    LoggerHelper.Log($"Error running task: {ex.Message}", LogLevel.Crit);
                }
            }, token);
        }
    }
}
