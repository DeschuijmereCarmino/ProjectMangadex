using ProjectMangadex.Models;
using ProjectMangadex.Repository;
using ProjectMangadex.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ProjectMangadex
{
    public partial class MainPage : ContentPage
    {
        public int Offset { get; set; }
        public MainPage()
        {
            InitializeComponent();
            LoadData();
            //TestModelsAndRepository();

            clvMangas.SelectionChanged += ClvMangas_SelectionChanged;

            tbiLogo.Clicked += TbiLogo_Clicked;
            tbiCollection.Clicked += TbiCollection_Clicked;
            tbiLogout.Clicked += TbiLogout_Clicked;
        }

        //private async void ClvMangas_RemainingItemsThresholdReached(object sender, EventArgs e)
        //{
        //    clvMangas.ItemsSource = await MangadexRepository.GetMangasAsync();
        //}

        private async void TbiLogout_Clicked(object sender, EventArgs e)
        {
            await MangadexRepository.GetUserLoggedOutAsync();
            await Navigation.PopToRootAsync();
        }

        private void TbiCollection_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new UserPage());
        }

        private void TbiLogo_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new MainPage());
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

        private async void LoadData()
        {
            tbiLogo.IconImageSource = ImageSource.FromResource("ProjectMangadex.Assets.Mangadex.png");
            clvMangas.ItemsSource = await MangadexRepository.GetMangasAsync();

            //clvMangas.RemainingItemsThreshold = 1;
            //clvMangas.RemainingItemsThresholdReached += ClvMangas_RemainingItemsThresholdReached;
        }

        private async void TestModelsAndRepository()
        {
            List<Manga> mangas = await MangadexRepository.GetMangasAsync();

            foreach (var manga in mangas)
            {
                Debug.WriteLine(manga.Title);
                Debug.WriteLine(manga.Description);
            }

            List<string> creators = await MangadexRepository.GetAuthorsForMangaAsync(mangas[0]);

            foreach(string creator in creators)
            {
                Debug.WriteLine(creator);
            }

            User user = new User
            {
                Username = "",
                Password = ""
            };

            await MangadexRepository.GetUserAsync(user);

            await MangadexRepository.LogToken();

            List<Manga> followedMangas = await MangadexRepository.GetFollowedMangasAsync();
            foreach (var followedManga in followedMangas)
            {
                Debug.WriteLine(followedManga.Title);
            }

            Guid mangaid = Guid.Parse("40bc649f-7b49-4645-859e-6cd94136e722");

            await MangadexRepository.FollowMangaAsync(mangaid);

            await MangadexRepository.GetUserLoggedOutAsync();
        }
    }
}
