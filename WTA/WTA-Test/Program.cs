using Core.Log;
using Core.Enum;
using Core.Model;
using Core.Output;
using Core.Config;
using Core.Data;
using WTA_Test;

ConfigManager.SetConfigValue("AppConfig", "Name", "WorkTimeAttach-Test");
ConfigManager.SetConfigValue("AppConfig", "Version", "0.0.1-Alpha");
ConfigManager.SetConfigValue("AppConfig", "DataPfad", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data"));
ConfigManager.SetConfigValue("AppConfig", "DataName", "data.json");

var repo = new RepoHandler<TestModel>();
var item = new TestModel
{
    Name = "Test",
    Description = "Beschreibung",
    Start = DateTime.Now,
    Ende = DateTime.Now.AddHours(8),
    Pause = 1
};
repo.Create(item);

var readItem = repo.ReadOne(item.ID);
Console.WriteLine($"Read item: {readItem.Name}, {readItem.Description}, {readItem.Start}, {readItem.Ende}, {readItem.WorkTime}");

item.Name = "Updated Test";
item.Description = "Aktualisierte Beschreibung";
repo.Update(item);

var allItems = repo.ReadAll();
foreach (var i in allItems)
{
    Console.WriteLine($"Item: {i.ID}, {i.Name}, {i.Description}, {i.Start}, {i.Ende}, {i.WorkTime}");
}
repo.Shutdown();

string appName = ConfigManager.GetConfigValue("AppConfig", "Name");
string appVersion = ConfigManager.GetConfigValue("AppConfig", "Version");



Logger.Instance.Log($"App Name: {appName}", LogLevel.Info);
Logger.Instance.Log($"App Version: {appVersion}", LogLevel.Info);

Logger.Instance.Log("This is an info message", LogLevel.Info);
Logger.Instance.Log("This is a debug message", LogLevel.Dbug);
Logger.Instance.Log("This is a warning message", LogLevel.Warn);
Logger.Instance.Log("This is a critical message", LogLevel.Crit);

List<LogEntry> logs = LoggerExtensions.GetCurrentMonthLogs();
LogOutput.DisplayLogs(logs);

Console.WriteLine();
Console.WriteLine("Please press [Enter]");
Console.ReadLine();