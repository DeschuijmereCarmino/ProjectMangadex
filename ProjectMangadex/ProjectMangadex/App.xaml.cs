using ProjectMangadex.Views;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProjectMangadex
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
            if (Connectivity.NetworkAccess == NetworkAccess.None)
            {
                MainPage = new NavigationPage(new NoNetworkPage());
            }
            else
            {
                MainPage = new NavigationPage(new MainPage());
            }
        }

        private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            if (Connectivity.NetworkAccess == NetworkAccess.None)
            {
                MainPage = new NavigationPage(new NoNetworkPage());
            }
            else
            {
                MainPage = new NavigationPage(new MainPage());
            }
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
