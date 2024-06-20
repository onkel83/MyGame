using Core.Config;
using Core.Data;
using Core.Enum;
using Core.Helper;
using Core.Log;
using WTA_Test;

ConfigManager.SetConfigValue("AppConfig", "Name", "WorkTimeAttach-Test");
ConfigManager.SetConfigValue("AppConfig", "Version", "0.0.1-Alpha");
ConfigManager.SetConfigValue("AppConfig", "DataPfad", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data"));
ConfigManager.SetConfigValue("AppConfig", "DataName", "data.json");
ConfigManager.SetConfigValue("TableConfig", "BorderChars", "═║╔╗╚╝╬╦╩╬╟╢");

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
if (readItem != null)
{
    Console.WriteLine($"Read item: {readItem.Name}, {readItem.Description}, {readItem.Start}, {readItem.Ende}, {readItem.WorkTime}");
}

item.Name = "Updated Test";
item.Description = "Aktualisierte Beschreibung";
repo.Update(item);

var allItems = repo.ReadAll();
foreach (var i in allItems)
{
    Console.WriteLine($"Item: {i.ID}, {i.Name}, {i.Description}, {i.Start}, {i.Ende}, {i.WorkTime}");
}
repo.Shutdown();

var items = new List<TestModel>
            {
                new() { Name = "Task 1", Description = "Description of Task 1", Start = DateTime.Now, Ende = DateTime.Now.AddHours(8), Pause = 1 },
                new() { Name = "Task 2", Description = "Description of Task 2", Start = DateTime.Now, Ende = DateTime.Now.AddHours(7.5), Pause = 0.5M },
                new() { Name = "Task 3", Description = "Description of Task 3", Start = DateTime.Now, Ende = DateTime.Now.AddHours(6), Pause = 0 }
            };

string[] headers = ["Name", "Description", "Start", "Ende", "Pause", "WorkTime"];

ConsoleTableHelper<TestModel>.PrintTable(items, headers);

string appName = ConfigManager.GetConfigValue("AppConfig", "Name");
string appVersion = ConfigManager.GetConfigValue("AppConfig", "Version");

Logger.Instance.Log($"App Name: {appName}", LogLevel.Info);
Logger.Instance.Log($"App Version: {appVersion}", LogLevel.Info);

Console.WriteLine();
Console.WriteLine("Please press [Enter]");
Console.ReadLine();