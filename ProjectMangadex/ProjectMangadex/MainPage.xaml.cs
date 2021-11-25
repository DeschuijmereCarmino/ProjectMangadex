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
        public MainPage()
        {
            InitializeComponent();
            //LoadData();
            TestModelsAndRepository();

            //clvMangas.SelectionChanged += ClvMangas_SelectionChanged;
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
            clvMangas.ItemsSource = await MangadexRepository.GetMangasAsync();
        }

        private async void TestModelsAndRepository()
        {
            List<Manga> mangas = await MangadexRepository.GetMangasAsync();

            foreach (var manga in mangas)
            {
                Debug.WriteLine(manga.Title);
                Debug.WriteLine(manga.Description);
            }

            foreach (var relationship in mangas[0].Relationships)
            {
                if(relationship.Type == "author")
                {
                    string author = await MangadexRepository.GetAuthorByIdAsync(mangas[0].Relationships[0].Id);
                    Debug.WriteLine(author);
                }
            }

            //User user = new User
            //{
            //    Username = "",
            //    Password = ""
            //};

            //await MangadexRepository.GetUserAsync(user);

            //await MangadexRepository.LogToken();

            //List<Manga> followedMangas = await MangadexRepository.GetFollowedMangasAsync();
            //foreach (var followedManga in followedMangas)
            //{
            //    Debug.WriteLine(followedManga.Title);
            //}

            //await MangadexRepository.FollowMangaAsync("40bc649f-7b49-4645-859e-6cd94136e722");

            //await MangadexRepository.GetUserLoggedOutAsync();
        }
    }
}
