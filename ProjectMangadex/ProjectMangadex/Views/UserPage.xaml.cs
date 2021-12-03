using ProjectMangadex.Models;
using ProjectMangadex.Repository;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProjectMangadex.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserPage : ContentPage
    {
        public UserPage()
        {
            InitializeComponent();
            LoadData();

            clvMangas.SelectionChanged += ClvMangas_SelectionChanged;

            tbiLogo.Clicked += TbiLogo_Clicked;
            tbiLogout.Clicked += TbiLogout_Clicked;

        }

        private void TbiLogo_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new MainPage());
        }

        private async void TbiLogout_Clicked(object sender, EventArgs e)
        {
            await MangadexRepository.GetUserLoggedOutAsync();
            await Navigation.PopToRootAsync();
        }

        private async void LoadData()
        {
            tbiLogo.IconImageSource = ImageSource.FromResource("ProjectMangadex.Assets.Mangadex.png");
            string username = await SecureStorage.GetAsync("username");

            lblUsername.Text = $"{username}'s Library:";

            clvMangas.ItemsSource = await MangadexRepository.GetFollowedMangasAsync();
        }

        private void ClvMangas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (clvMangas.SelectedItem != null)
            {
                Manga manga = (Manga)clvMangas.SelectedItem;
                Debug.WriteLine(manga.Title);
                Navigation.PushAsync(new DetailPage(manga));
                clvMangas.SelectedItem = null;
            }
        }
    }
}