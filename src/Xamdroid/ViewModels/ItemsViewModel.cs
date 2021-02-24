using System;
using System.Threading.Tasks;
using Android.Runtime;
using AndroidX.Lifecycle;
using Java.Lang;
using Microsoft.Extensions.DependencyInjection;
using Xamdroid.Extensions;
using Xamdroid.Models;
using Xamdroid.Services;
using Object = Java.Lang.Object;

namespace Xamdroid.ViewModels
{
    public class ItemsViewModel : ViewModel
    {
        private readonly IDataStore<Item> _dataStore;
        private readonly MutableLiveData _items;

        private ItemsViewModel(IDataStore<Item> dataStore)
        {
            _dataStore = dataStore ?? throw new ArgumentNullException(nameof(dataStore));
            _items = new MutableLiveData(new JavaList<Item>());
        }

        public LiveData Items => _items;

        public async Task LoadItemsAsync() => _items.SetValue((await _dataStore.GetItemsAsync()).ToJavaList());

        public class Factory : Object, ViewModelProvider.IFactory
        {
            private readonly IDataStore<Item> _dataStore;

            public Factory(IDataStore<Item> dataStore = null) =>
                _dataStore = dataStore ?? Container.Service.GetService<IDataStore<Item>>()
                    ?? throw new ArgumentNullException(nameof(dataStore));

            public Object Create(Class modelClass) => new ItemsViewModel(_dataStore);
        }
    }
}