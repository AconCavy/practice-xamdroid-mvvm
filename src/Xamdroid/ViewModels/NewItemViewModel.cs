using System;
using System.Threading.Tasks;
using AndroidX.Lifecycle;
using Java.Lang;
using Microsoft.Extensions.DependencyInjection;
using Xamdroid.Models;
using Xamdroid.Services;
using Object = Java.Lang.Object;

namespace Xamdroid.ViewModels
{
    public class NewItemViewModel : ViewModel
    {
        private readonly IDataStore<Item> _dataStore;

        private NewItemViewModel(IDataStore<Item> dataStore)
        {
            _dataStore = dataStore ?? throw new ArgumentNullException(nameof(dataStore));
            Text = new MutableLiveData(string.Empty);
            Description = new MutableLiveData(string.Empty);
        }

        public MutableLiveData Description { get; }
        public MutableLiveData Text { get; }

        public async Task SaveItemAsync()
        {
            var newItem = new Item
            {
                Id = new Guid().ToString(), Text = Text.Value.ToString(), Description = Description.Value.ToString()
            };

            await _dataStore.AddItemAsync(newItem);
        }

        public class Factory : Object, ViewModelProvider.IFactory
        {
            private readonly IDataStore<Item> _dataStore;

            public Factory(IDataStore<Item> dataStore = null) =>
                _dataStore = dataStore ?? Container.Service.GetService<IDataStore<Item>>()
                    ?? throw new ArgumentNullException(nameof(dataStore));

            public Object Create(Class p0) => new NewItemViewModel(_dataStore);
        }
    }
}