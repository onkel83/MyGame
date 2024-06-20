using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Core.Config;
using Core.Helper;
using Core.Log;
using Core.Enum;
using Core.Model;

namespace Core.Data
{
    public class RepoHandler<T> where T : BaseModel
    {
        private readonly string _filePath;
        private readonly ConcurrentQueue<Action> _queue = new ConcurrentQueue<Action>();
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private List<T> _items = new List<T>();
        private int _nextId = 1;
        private readonly object _lock = new object();

        public RepoHandler()
        {
            string dataPath = ConfigManager.GetConfigValue("AppConfig", "DataPfad");
            _filePath = Path.Combine(dataPath, $"{typeof(T).Name}.json");
            FileHelper.EnsureDirectoryExists(dataPath);
            Load();

            var fileWatcher = new FileSystemWatcher(dataPath)
            {
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName,
                Filter = $"{typeof(T).Name}.json",
                IncludeSubdirectories = false,
                EnableRaisingEvents = true
            };
            fileWatcher.Changed += (s, e) => Load();

            Task.Run(() => ProcessQueue(_cancellationTokenSource.Token));
        }

        private void ProcessQueue(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                if (_queue.TryDequeue(out var action))
                {
                    lock (_lock)
                    {
                        action();
                    }
                }
                else
                {
                    Thread.Sleep(100);
                }
            }
        }

        public void Create(T item)
        {
            Enqueue(() =>
            {
                item.ID = _nextId.ToString("D6");
                _nextId++;
                _items.Add(item);
                Save();
                Logger.Instance.Log($"Created item with ID: {item.ID}", LogLevel.Info);
            });
        }

        public void Update(T item)
        {
            Enqueue(() =>
            {
                var existingItem = _items.FirstOrDefault(i => i.ID == item.ID);
                if (existingItem != null)
                {
                    _items.Remove(existingItem);
                    _items.Add(item);
                    Save();
                    Logger.Instance.Log($"Updated item with ID: {item.ID}", LogLevel.Info);
                }
            });
        }

        public void Delete(string id)
        {
            Enqueue(() =>
            {
                var item = _items.FirstOrDefault(i => i.ID == id);
                if (item != null)
                {
                    _items.Remove(item);
                    Save();
                    Logger.Instance.Log($"Deleted item with ID: {item.ID}", LogLevel.Info);
                }
            });
        }

        public List<T> ReadAll()
        {
            lock (_lock)
            {
                return new List<T>(_items);
            }
        }

        public T ReadOne(string id)
        {
            lock (_lock)
            {
                return _items.FirstOrDefault(i => i.ID == id);
            }
        }

        public List<T> Search(Func<T, bool> predicate)
        {
            lock (_lock)
            {
                return _items.Where(predicate).ToList();
            }
        }

        public void ResetIds()
        {
            Enqueue(() =>
            {
                _items = _items.OrderBy(i => Convert.ToInt32(i.ID)).ToList();
                _nextId = 1;
                foreach (var item in _items)
                {
                    item.ID = _nextId.ToString("D6");
                    _nextId++;
                }
                Save();
                Logger.Instance.Log("Reset IDs and redistributed them.", LogLevel.Info);
            });
        }

        private void Enqueue(Action action)
        {
            _queue.Enqueue(action);
        }

        private void Load()
        {
            if (!File.Exists(_filePath)) return;

            lock (_lock)
            {
                string json = File.ReadAllText(_filePath);
                _items = JsonConvert.DeserializeObject<List<T>>(json) ?? new List<T>();
                ResetIds();
                Logger.Instance.Log($"Loaded data from {_filePath}", LogLevel.Info);
            }
        }

        private void Save()
        {
            lock (_lock)
            {
                string json = JsonConvert.SerializeObject(_items, Formatting.Indented);
                File.WriteAllText(_filePath, json);
                Logger.Instance.Log($"Saved data to {_filePath}", LogLevel.Info);
            }
        }

        public void Shutdown()
        {
            _cancellationTokenSource.Cancel();
            Logger.Instance.Log($"RepoHandler for {typeof(T).Name} shut down.", LogLevel.Info);
        }
    }
}
