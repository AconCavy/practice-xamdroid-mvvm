using System;
using System.Collections.Generic;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.Fragment.App;
using AndroidX.Lifecycle;
using Google.Android.Material.FloatingActionButton;
using Java.Lang;
using Xamdroid.Models;
using Xamdroid.ViewModels;
using Object = Java.Lang.Object;
using String = System.String;

namespace Xamdroid.Views
{
    public class ItemsFragment : Fragment
    {
        private ItemsAdapter _adapter;
        private JavaList<Item> _items;
        private ListObserver _listObserver;
        private ItemsViewModel _viewModel;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            _viewModel ??= new ViewModelProvider(this, new ItemsViewModel.Factory())
                .Get(Class.FromType(typeof(ItemsViewModel))) as ItemsViewModel;

            _items = new JavaList<Item>();
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            if (container is null) return null;
            base.OnCreateView(inflater, container, savedInstanceState);

            return inflater.Inflate(Resource.Layout.fragment_items, container, false);
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
            if (_viewModel is null) return;

            var list = view.FindViewById<ListView>(Resource.Id.fragment_items_list);
            if (list is { })
            {
                _adapter ??= new ItemsAdapter(Context, Resource.Id.list_item, _items);
                list.Adapter = _adapter;
                list.ItemClick += OnListClick;
            }

            var fab = view.FindViewById<FloatingActionButton>(Resource.Id.fragment_items_fab);
            if (fab is { }) fab.Click += OnFabClick;

            _listObserver ??= new ListObserver(_items);
            _viewModel.Items.Observe(this, _listObserver);

            _ = _viewModel.LoadItemsAsync();
        }

        protected override void Dispose(bool disposing)
        {
            _listObserver.Dispose();
            base.Dispose(disposing);
        }

        private void OnListClick(object sender, AdapterView.ItemClickEventArgs args)
        {
            var item = _adapter.GetItem(args.Position);
            var query = GetString(Resource.String.query_selected_item_id);
            var bundle = new Bundle();
            bundle.PutString(query, item?.Id ?? String.Empty);
            ParentFragmentManager.SetFragmentResult(query, bundle);

            ParentFragmentManager.BeginTransaction()
                .Replace(Resource.Id.activity_main_container, new ItemDetailFragment { Arguments = bundle })
                .AddToBackStack(null)
                .Commit();
        }

        private void OnFabClick(object sender, EventArgs args) =>
            ParentFragmentManager.BeginTransaction()
                .Replace(Resource.Id.activity_main_container, new NewItemFragment())
                .AddToBackStack(null)
                .Commit();

        #region Inner classes

        private class ListObserver : Object, IObserver
        {
            private readonly JavaList<Item> _items;
            public ListObserver(JavaList<Item> list) => _items = list;

            public void OnChanged(Object p0)
            {
                if (!(p0 is JavaList<Item> list)) return;
                _items.Clear();
                foreach (var item in list) _items.Add(item);
            }
        }

        private class ItemsAdapter : ArrayAdapter<Item>
        {
            public ItemsAdapter(IntPtr handle, JniHandleOwnership transfer) : base(handle, transfer) { }
            public ItemsAdapter(Context context, int textViewResourceId) : base(context, textViewResourceId) { }

            public ItemsAdapter(Context context, int resource, int textViewResourceId) : base(context, resource,
                textViewResourceId)
            {
            }

            public ItemsAdapter(Context context, int textViewResourceId, Item[] objects) : base(context,
                textViewResourceId, objects)
            {
            }

            public ItemsAdapter(Context context, int resource, int textViewResourceId, Item[] objects) : base(context,
                resource, textViewResourceId, objects)
            {
            }

            public ItemsAdapter(Context context, int textViewResourceId, IList<Item> objects) : base(context,
                textViewResourceId, objects)
            {
            }

            public ItemsAdapter(Context context, int resource, int textViewResourceId, IList<Item> objects) : base(
                context, resource, textViewResourceId, objects)
            {
            }

            public override View GetView(int position, View convertView, ViewGroup parent)
            {
                convertView ??= LayoutInflater.FromContext(Context)?.Inflate(Resource.Layout.list_item, parent, false);
                if (convertView is null) return null;

                var item = GetItem(position);
                if (item is null) return convertView;

                var text = convertView.FindViewById<TextView>(Resource.Id.list_item_text);
                if (text is { }) text.Text = item.Text;

                var description = convertView.FindViewById<TextView>(Resource.Id.list_item_description);
                if (description is { }) description.Text = item.Description;

                return convertView;
            }
        }

        #endregion
    }
}