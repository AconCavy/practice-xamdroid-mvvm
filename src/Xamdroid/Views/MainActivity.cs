using Android.App;
using Android.OS;
using AndroidX.AppCompat.App;
using Xamdroid.Services;

namespace Xamdroid.Views
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            Container.Initialize();

            SupportFragmentManager?.BeginTransaction()
                .Add(Resource.Id.activity_main_container, new ItemsFragment())
                .Commit();
        }
    }
}