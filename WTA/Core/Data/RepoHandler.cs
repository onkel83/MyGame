using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Core.Helper;
using Core.Model;

namespace Core.Data
{
    public class RepoHandler<T> where T : BaseModel
    {
        private readonly List<T> _items;
        private readonly string _dataFilePath;

        public RepoHandler()
        {
            var dataPath = ConfigHelper.GetConfigValue("AppConfig", "DataPfad", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data"));
            var dataName = ConfigHelper.GetConfigValue("AppConfig", "DataName", $"{nameof(T)}.json");
            _dataFilePath = Path.Combine(dataPath, dataName);

            _items = Load() ?? new List<T>();
        }

        public void Create(T item)
        {
            item.ID = (_items.Count > 0 ? _items.Max(x => int.Parse(x.ID)) + 1 : 1).ToString("D6");
            _items.Add(item);
            Save();
        }

        public T ReadOne(string id)
        {
            return _items.FirstOrDefault(x => x.ID == id);
        }

        public List<T> ReadAll()
        {
            return _items;
        }

        public void Update(T item)
        {
            var index = _items.FindIndex(x => x.ID == item.ID);
            if (index >= 0)
            {
                _items[index] = item;
            }
            else
            {
                _items.Add(item);
            }
            Save();
        }

        public void Delete(string id)
        {
            var item = _items.FirstOrDefault(x => x.ID == id);
            if (item != null)
            {
                _items.Remove(item);
                Save();
            }
        }

        public List<T> Search(Func<T, bool> predicate)
        {
            return _items.Where(predicate).ToList();
        }

        public void Save()
        {
            try
            {
                var jsonData = JsonConvert.SerializeObject(_items, Formatting.Indented);
                FileHelper.WriteFile(_dataFilePath, jsonData);
            }
            catch (Exception ex)
            {
                LoggerHelper.Log($"Error saving data: {ex.Message}", LogLevel.Crit);
            }
        }

        private List<T> Load()
        {
            try
            {
                if (FileHelper.FileExists(_dataFilePath))
                {
                    var jsonData = FileHelper.ReadFile(_dataFilePath);
                    return JsonConvert.DeserializeObject<List<T>>(jsonData)??new List<T>();
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Log($"Error loading data: {ex.Message}", LogLevel.Crit);
            }
            return new List<T>();
        }

        public void Shutdown()
        {
            Save();
        }
    }
}
