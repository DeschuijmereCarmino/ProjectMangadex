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
        public Manga Manga { get; set; }
        public Boolean IsFollowed { get; set; }
        public DetailPage(Manga selectedManga)
        {
            InitializeComponent();
            Manga = selectedManga;
            ShowMangaDetails();

            btnFollow.Clicked += BtnFollow_Clicked;

            tbiLogo.Clicked += TbiLogo_Clicked;
            tbiCollection.Clicked += TbiCollection_Clicked;
            tbiLogout.Clicked += TbiLogout_Clicked;
        }

        private async void BtnFollow_Clicked(object sender, EventArgs e)
        {
            if (!IsFollowed)
            {
                await MangadexRepository.FollowMangaAsync(Manga.Id);
                SetFollowButton();
            }
            else
            {
                await MangadexRepository.UnfollowMangaAsync(Manga.Id);
                SetFollowButton();
            }
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

        private async void ShowMangaDetails()
        {
            SetFollowButton();
            tbiLogo.IconImageSource = ImageSource.FromResource("ProjectMangadex.Assets.Mangadex.png");
            List<string> creators = await MangadexRepository.GetAuthorsForMangaAsync(Manga);

            lblAuthor.Text = string.Join(", ", creators); ;

            imgCover.Source = Manga.Cover;
            lblTitle.Text = Manga.Title;
            lblDescription.Text = Manga.Description;


        }

        private async void SetFollowButton()
        {
            IsFollowed = await MangadexRepository.CheckMangaFollowed(Manga.Id);
            if (!IsFollowed)
            {
                btnFollow.Text = "Follow";
            }
            else
            {
                btnFollow.Text = "Unfollow";
            }
        }
    }
}