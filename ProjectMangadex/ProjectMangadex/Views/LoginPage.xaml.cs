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
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();

            btnLogin.Clicked += BtnLogin_Clicked;
        }

        private async void BtnLogin_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(entPassword.Text) & !string.IsNullOrEmpty(entUsername.Text))
            {
                User user = new User
                {
                    Username = entUsername.Text,
                    Password = entPassword.Text
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