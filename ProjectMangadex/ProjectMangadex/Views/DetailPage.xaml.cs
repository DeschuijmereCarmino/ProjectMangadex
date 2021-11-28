using ProjectMangadex.Models;
using ProjectMangadex.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProjectMangadex.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DetailPage : ContentPage
    {
        public Manga manga { get; set; }
        public DetailPage(Manga selectedManga)
        {
            InitializeComponent();
            manga = selectedManga;
            showMangaDetails();

            tbiLogo.Clicked += TbiLogo_Clicked;
            tbiCollection.Clicked += TbiCollection_Clicked;
            tbiLogout.Clicked += TbiLogout_Clicked;
        }

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

        private async void showMangaDetails()
        {
            tbiLogo.IconImageSource = ImageSource.FromResource("ProjectMangadex.Assets.Mangadex.png");
            List<string> creators = await MangadexRepository.GetAuthorsForMangaAsync(manga);

            lblAuthor.Text = string.Join(", ", creators); ;

            imgCover.Source = manga.Cover;
            lblTitle.Text = manga.Title;
            lblDescription.Text = manga.Description;
        }
    }
}