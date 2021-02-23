using System;
using System.Threading.Tasks;
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
    public class NewItemFragment : Fragment
    {
        private DescriptionLengthObserver _descriptionLengthObserver;
        private NewItemViewModel _viewModel;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            _viewModel = new ViewModelProvider(this, new NewItemViewModel.Factory())
                .Get(Class.FromType(typeof(NewItemViewModel))) as NewItemViewModel;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            if (container is null) return null;
            base.OnCreateView(inflater, container, savedInstanceState);

            return inflater.Inflate(Resource.Layout.fragment_new_item, container, false);
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
            if (_viewModel is null) return;

            var text = view.FindViewById<EditText>(Resource.Id.fragment_new_item_edit_text);
            if (text is { }) text.TextChanged += (sender, args) => _viewModel.Text.SetValue(text.Text);

            var description = view.FindViewById<EditText>(Resource.Id.fragment_new_item_edit_description);
            if (description is { })
                description.TextChanged += (sender, args) => _viewModel.Description.SetValue(description.Text);

            var descriptionCount = view.FindViewById<TextView>(Resource.Id.fragment_new_item_description_count);
            if (descriptionCount is { })
            {
                _descriptionLengthObserver ??= new DescriptionLengthObserver(descriptionCount);
                _viewModel.Description.Observe(this, _descriptionLengthObserver);
            }

            var button = view.FindViewById<Button>(Resource.Id.fragment_new_item_save);
            if (button is { }) button.Click += Save;
        }

        protected override void Dispose(bool disposing)
        {
            _descriptionLengthObserver.Dispose();
            base.Dispose(disposing);
        }

        private void Save(object sender, EventArgs args) => _ = SaveAsync();

        private async Task SaveAsync()
        {
            await _viewModel.SaveItemAsync();
            ParentFragmentManager.PopBackStack();
        }

        #region Inner classes

        private class DescriptionLengthObserver : Object, IObserver
        {
            private readonly TextView _textView;
            public DescriptionLengthObserver(TextView textView) => _textView = textView;

            public void OnChanged(Object p0) => _textView.Text = $"Current: {p0.ToString().Length} characters";
        }

        #endregion
    }
}