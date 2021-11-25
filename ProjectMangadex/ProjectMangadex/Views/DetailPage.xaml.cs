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
        }

        private async void showMangaDetails()
        {
            List<string> creators = await MangadexRepository.GetAuthorsForMangaAsync(manga);

            lblAuthor.Text = string.Join(", ", creators); ;

            imgCover.Source = manga.Cover;
            lblTitle.Text = manga.Title;
            lblDescription.Text = manga.Description;
        }
    }
}