using ProjectMangadex.Models;
using ProjectMangadex.Repository;
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
            TestModelsAndRepository();
        }

        private async void TestModelsAndRepository()
        {
            List<Manga> mangas = await MangadexRepository.GetMangasAsync();

            foreach (var manga in mangas)
            {
                Debug.WriteLine(manga.Title);
                Debug.WriteLine(manga.Description);
            }

            User user = new User
            {
                Username = "",
                Password = ""
            };

            await MangadexRepository.GetUserAsync(user);

            await MangadexRepository.LogToken();
        }
    }
}
