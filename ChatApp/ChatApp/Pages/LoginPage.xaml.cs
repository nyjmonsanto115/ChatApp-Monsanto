﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using ChatApp.Models;
using ChatApp.DependencyServices;


namespace ChatApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            NavigationPage.SetHasBackButton(this, false);
            NavigationPage.SetHasNavigationBar(this, false);
        }

        //Navigation 
        private async void SignUpNavigate(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SignUpPage());
        }

        private async void ResetPassPageNavigate(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ResetPassPage());
        }

        //Loading Screen
        void ToggleIndicator(bool show)
        {
            bg.IsVisible = show;
            actvty_indctr.IsEnabled = show;
            actvty_indctr.IsVisible = show;
            actvty_indctr.IsRunning = show;
        }

        //Show/Hide Password
        public void ShowHidePass(object sender, EventArgs e)
        {
            Password.IsPassword = Password.IsPassword ? false : true;
            btn_ShowHide.Text = Password.IsPassword ? "Show" : "Hide";
        }

        //Changes Border Color for Entry when focused
        public void ChangeColor(object sender, EventArgs e)
        {
            if (Email.IsFocused)
            {
                Frame1.BorderColor = Color.Black;
            }

            if (Password.IsFocused)
            {
                Frame2.BorderColor = Color.Black;
            }
        }

        //For Google and Facebook Sign-in buttons
        //Will only display an alert since the functionalities of the two buttons are optional
        private async void ButtonAction(object sender, EventArgs e)
        {
            ToggleIndicator(true);
            await Task.Delay(2500);
            ToggleIndicator(false);
            await DisplayAlert("", "Functionalities for these buttons have not yet been implemented.", "Okay");
        }

        //Sign In
        private async void SignInAction(object sender, EventArgs e)
        {
            ToggleIndicator(true);
            await Task.Delay(2500);
            ToggleIndicator(false);
            App.Current.Properties["isLoggedIn"] = true;
            App.Current.MainPage = new MainPage();
        }

        public async void SignInProcess(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Email.Text) || string.IsNullOrEmpty(Password.Text))
            {
                if (string.IsNullOrEmpty(Email.Text))
                {
                    Frame1.BorderColor = Color.Red;
                }

                if (string.IsNullOrEmpty(Password.Text))
                {
                    Frame2.BorderColor = Color.Red;
                }

                await DisplayAlert("Error", "Missing fields.", "Okay");
                Email.Text = string.Empty;
                Password.Text = string.Empty;
            }
            else
            {
                ToggleIndicator(true);

                FirebaseAuthResponseModel res = new FirebaseAuthResponseModel() { };
                res = await DependencyService.Get<iFirebaseAuth>().LoginWithEmailPassword(Email.Text, Password.Text);

                if (res.Status == true)
                {
                    Application.Current.MainPage = new NavigationPage(new MainPage());
                }
                else
                {
                    bool retryBool = await DisplayAlert("Error", res.Response + " Retry?", "Yes", "No");
                    if (retryBool)
                    {
                        Email.Text = string.Empty;
                        Password.Text = string.Empty;
                        Email.Focus();
                    }
                }

                ToggleIndicator(false);
            }
        }
    }
}