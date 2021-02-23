using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamdroid.Models;

namespace Xamdroid.Services
{
    public class MockDataStore : IDataStore<Item>
    {
        private readonly List<Item> _items;

        public MockDataStore() =>
            _items = new List<Item>
            {
                new Item
                {
                    Id = Guid.NewGuid().ToString(),
                    Text = "First item",
                    Description = "This is an item description."
                },
                new Item
                {
                    Id = Guid.NewGuid().ToString(),
                    Text = "Second item",
                    Description = "This is an item description."
                },
                new Item
                {
                    Id = Guid.NewGuid().ToString(),
                    Text = "Third item",
                    Description = "This is an item description."
                },
                new Item
                {
                    Id = Guid.NewGuid().ToString(),
                    Text = "Fourth item",
                    Description = "This is an item description."
                },
                new Item
                {
                    Id = Guid.NewGuid().ToString(),
                    Text = "Fifth item",
                    Description = "This is an item description."
                },
                new Item
                {
                    Id = Guid.NewGuid().ToString(),
                    Text = "Sixth item",
                    Description = "This is an item description."
                }
            };

        public Task<bool> AddItemAsync(Item item)
        {
            _items.Add(item);

            return Task.FromResult(true);
        }

        public Task<bool> UpdateItemAsync(Item item)
        {
            if (_items.All(x => x.Id != item.Id)) return Task.FromResult(false);

            var oldItem = _items.First(x => x.Id == item.Id);
            _items.Remove(oldItem);
            _items.Add(item);

            return Task.FromResult(true);
        }

        public Task<bool> DeleteItemAsync(string id)
        {
            if (_items.All(x => x.Id != id)) return Task.FromResult(false);

            var oldItem = _items.First(x => x.Id == id);
            _items.Remove(oldItem);

            return Task.FromResult(true);
        }

        public Task<Item> GetItemAsync(string id)
        {
            var item = _items.FirstOrDefault(s => s.Id == id);
            return Task.FromResult(item);
        }

        public Task<IEnumerable<Item>> GetItemsAsync(bool forceRefresh = false) =>
            Task.FromResult(_items.AsEnumerable());
    }
}