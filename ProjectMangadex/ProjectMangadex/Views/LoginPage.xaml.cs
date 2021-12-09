using ProjectMangadex.Models;
using ProjectMangadex.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProjectMangadex.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();

            btnLogin.Clicked += BtnLogin_Clicked;
            TapGestureRecognizer recognizer = new TapGestureRecognizer();

            recognizer.Tapped += Recognizer_Tapped;

            lblCreate.GestureRecognizers.Add(recognizer);
        }

        private async void Recognizer_Tapped(object sender, EventArgs e)
        {
            //Navigation.PushAsync(new CreateAccountPage());
            await Browser.OpenAsync("https://mangadex.org/account/signup", BrowserLaunchMode.SystemPreferred);
        }

        private async void BtnLogin_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(entPassword.Text) & !string.IsNullOrEmpty(entUsername.Text))
            {
                User user = new User
                {
                    Username = entUsername.Text,
                    Password = entPassword.Text,
                    Email = ""
                };

                bool succes = await MangadexRepository.GetUserAsync(user);

                if (succes)
                {
                    await Navigation.PushAsync(new MainPage());
                }
            }
        }

    }
}