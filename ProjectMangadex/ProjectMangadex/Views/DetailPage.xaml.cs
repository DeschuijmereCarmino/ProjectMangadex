using ProjectMangadex.Models;
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

        private void showMangaDetails()
        {
            imgCover.Source = manga.Cover;
            lblTitle.Text = manga.Title;
            lblDescription.Text = manga.Description;
        }
    }
}