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
    public class ItemDetailViewModel : ViewModel
    {
        private readonly IDataStore<Item> _dataStore;
        private readonly MutableLiveData _description;
        private readonly MutableLiveData _text;

        private ItemDetailViewModel(IDataStore<Item> dataStore = null)
        {
            _dataStore = dataStore ?? Container.Service.GetService<IDataStore<Item>>() ??
                throw new ArgumentNullException(nameof(dataStore));
            _description = new MutableLiveData(string.Empty);
            _text = new MutableLiveData(string.Empty);
        }

        public LiveData Description => _description;
        public LiveData Text => _text;

        public async Task LoadItemAsync(string itemId)
        {
            try
            {
                var item = await _dataStore.GetItemAsync(itemId);
                _text.PostValue(item.Text);
                _description.PostValue(item.Description);
            }
            catch
            {
                _description.SetValue("Failed to Load Item");
            }
        }

        public class Factory : Object, ViewModelProvider.IFactory
        {
            public Object Create(Class modelClass) => new ItemDetailViewModel();
        }
    }
}