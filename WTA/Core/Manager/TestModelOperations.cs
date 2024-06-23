using System;
using System.Collections.Generic;
using Core.Data;
using Core.Enums;
using Core.Helper;
using Core.Model;

namespace Core.Manager
{
    public static class TestModelOperations
    {
        public static readonly RepoHandler<TestModel> repo = new RepoHandler<TestModel>();

        public static void CreateTestModel()
        {
            try
            {
                var newModel = InputHandler.GetTestModelFromUser();
                if (newModel == null) return;
                repo.Create(newModel);
                LoggerHelper.Log($"Created new TestModel with ID: {newModel.ID}", LogLevel.Info);
            }
            catch (Exception ex)
            {
                LoggerHelper.Log($"Error creating TestModel: {ex.Message}", LogLevel.Crit);
                ApplicationReloader.ReloadApplication();
            }
        }

        public static void ReadTestModelById()
        {
            try
            {
                Console.Write("Enter TestModel ID: ");
                string id = Console.ReadLine();
                if (id == "/q") return;
                var model = repo.ReadOne(id);
                if (model != null)
                {
                    ConsoleHelper.WriteTable(new List<TestModel> { model });
                }
                else
                {
                    ConsoleHelper.WriteLine("TestModel not found.", ConsoleColor.Red);
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Log($"Error reading TestModel: {ex.Message}", LogLevel.Crit);
                ApplicationReloader.ReloadApplication();
            }
        }

        public static void ReadAllTestModels()
        {
            try
            {
                var models = repo.ReadAll();
                if (models.Count > 0)
                {
                    ConsoleHelper.WriteTable(models);
                }
                else
                {
                    ConsoleHelper.WriteLine("No TestModels found.", ConsoleColor.Yellow);
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Log($"Error reading all TestModels: {ex.Message}", LogLevel.Crit);
                ApplicationReloader.ReloadApplication();
            }
        }

        public static void UpdateTestModel()
        {
            try
            {
                Console.Write("Enter TestModel ID to update: ");
                string id = Console.ReadLine();
                if (id == "/q") return;
                var model = repo.ReadOne(id) ?? new TestModel { ID = id };
                Console.WriteLine("Updating TestModel...");
                var updatedModel = InputHandler.GetTestModelFromUser(model);
                if (updatedModel == null) return;
                repo.Update(updatedModel);
                LoggerHelper.Log($"Updated TestModel with ID: {updatedModel.ID}", LogLevel.Info);
            }
            catch (Exception ex)
            {
                LoggerHelper.Log($"Error updating TestModel: {ex.Message}", LogLevel.Crit);
                ApplicationReloader.ReloadApplication();
            }
        }

        public static void DeleteTestModel()
        {
            try
            {
                Console.Write("Enter TestModel ID to delete: ");
                string id = Console.ReadLine();
                if (id == "/q") return;
                repo.Delete(id);
                LoggerHelper.Log($"Deleted TestModel with ID: {id}", LogLevel.Info);
            }
            catch (Exception ex)
            {
                LoggerHelper.Log($"Error deleting TestModel: {ex.Message}", LogLevel.Crit);
                ApplicationReloader.ReloadApplication();
            }
        }

        public static void SearchTestModels()
        {
            try
            {
                Console.Write("Enter search term: ");
                string searchTerm = Console.ReadLine();
                if (searchTerm == "/q") return;
                var results = repo.Search(m => m.Name.Contains(searchTerm) || m.Description.Contains(searchTerm));
                if (results.Count > 0)
                {
                    ConsoleHelper.WriteTable(results);
                }
                else
                {
                    ConsoleHelper.WriteLine("No TestModels found matching the search term.", ConsoleColor.Yellow);
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Log($"Error searching TestModels: {ex.Message}", LogLevel.Crit);
                ApplicationReloader.ReloadApplication();
            }
        }

        public static void Exit()
        {
            ConsoleHelper.WriteLine("Exiting...", ConsoleColor.Cyan);
            repo.Shutdown();
            Environment.Exit(0);
        }
    }
}
