using System;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.Fragment.App;
using AndroidX.Lifecycle;
using Java.Lang;
using Xamdroid.ViewModels;
using Object = Java.Lang.Object;

namespace Xamdroid.Views
{
    public class ItemDetailFragment : Fragment
    {
        private TextViewObserver _descriptionObserver;
        private TextViewObserver _textObserver;
        private ItemDetailViewModel _viewModel;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            _viewModel = new ViewModelProvider(this, new ItemDetailViewModel.Factory())
                .Get(Class.FromType(typeof(ItemDetailViewModel))) as ItemDetailViewModel;

            var query = GetString(Resource.String.query_selected_item_id);
            ParentFragmentManager.SetFragmentResultListener(query, this,
                new FragmentResultListener(x => _ = _viewModel?.LoadItemAsync(x)));
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            if (container is null) return null;
            base.OnCreateView(inflater, container, savedInstanceState);

            return inflater.Inflate(Resource.Layout.fragment_item_detail, container, false);
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
            if (_viewModel is null) return;

            var text = view.FindViewById<TextView>(Resource.Id.fragment_item_detail_text);
            if (text is { })
            {
                _textObserver ??= new TextViewObserver(text);
                _viewModel.Text.Observe(this, _textObserver);
            }

            var description = view.FindViewById<TextView>(Resource.Id.fragment_item_detail_description);
            if (description is { })
            {
                _descriptionObserver ??= new TextViewObserver(description);
                _viewModel.Description.Observe(this, _descriptionObserver);
            }
        }

        protected override void Dispose(bool disposing)
        {
            _textObserver.Dispose();
            _descriptionObserver.Dispose();
            base.Dispose(disposing);
        }

        #region Inner classes

        private class TextViewObserver : Object, IObserver
        {
            private readonly TextView _textView;

            public TextViewObserver(TextView textView) =>
                _textView = textView ?? throw new ArgumentNullException(nameof(textView));

            public void OnChanged(Object p0) => _textView.Text = p0.ToString();
        }

        private class FragmentResultListener : Object, IFragmentResultListener
        {
            private readonly Action<string> _action;
            public FragmentResultListener(Action<string> action) => _action = action;

            public void OnFragmentResult(string p0, Bundle p1)
            {
                var selectedItemId = p1.GetString(p0, string.Empty);
                _action(selectedItemId);
            }
        }

        #endregion
    }
}