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
    public partial class CreateAccountPage : ContentPage
    {
        public CreateAccountPage()
        {
            InitializeComponent();

            btnCreate.Clicked += BtnCreate_Clicked;
        }

        private async void BtnCreate_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(entPassword.Text) & !string.IsNullOrEmpty(entUsername.Text) & !string.IsNullOrEmpty(entEmail.Text))
            {

                User user = new User
                {
                    Username = entUsername.Text,
                    Password = entPassword.Text,
                    Email = entEmail.Text
                };

                await MangadexRepository.CreateAccount(user);
                await Navigation.PopAsync();
            }
        }
    }
}