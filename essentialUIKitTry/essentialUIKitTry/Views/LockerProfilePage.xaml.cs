﻿using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;
using CounterFunctions;
using Xamarin.Forms;
using Plugin.LocalNotification;
using System;
using Plugin.LocalNotifications;
using System.Threading.Tasks;

namespace essentialUIKitTry.Views
{
    /// <summary>
    /// Page to show chat profile page
    /// </summary>
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LockerProfilePage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LockerProfilePage" /> class.
        /// </summary>

        Locker locker;
        bool photoTaken = false;
        public LockerProfilePage(int lockerId)
        {
            this.InitializeComponent();
            InitializeLocker(lockerId);
            initializePageComponent();
            SetLocker(lockerId);
        }

        private void initializePageComponent()
        {
            int btnTakephotoFontSize = 12;
            int btn_width = 160;
            int btn_height = 45;
            LockUnlockBtn.Text = this.locker.locked ? "Unlock" : "Lock";
            LockUnlockBtn.WidthRequest = btn_width;
            LockUnlockBtn.HeightRequest = btn_height;
            LockUnlockBtn.BackgroundColor = Color.LightSteelBlue;
            LockUnlockBtn.Padding = new Xamarin.Forms.Thickness(5, 2);
            LockUnlockBtn.FontSize = btnTakephotoFontSize;
            LockUnlockBtn.Clicked += HandleLockUnlockBtn;

            TakeAPhotoBtn.Text = "Take Photo :)";
            TakeAPhotoBtn.WidthRequest = btn_width;
            TakeAPhotoBtn.HeightRequest = btn_height;
            TakeAPhotoBtn.BackgroundColor = Color.LightSteelBlue;
            TakeAPhotoBtn.Padding = new Xamarin.Forms.Thickness(5, 2);
            TakeAPhotoBtn.FontSize = btnTakephotoFontSize;
            TakeAPhotoBtn.Clicked += HandleTakeAPhotoBtn;

            ReleaseBtn.Text = "Release Locker";
            ReleaseBtn.WidthRequest = btn_width;
            ReleaseBtn.HeightRequest = btn_height;
            ReleaseBtn.BackgroundColor = Color.LightSteelBlue;
            ReleaseBtn.Padding = new Xamarin.Forms.Thickness(5, 2);
            ReleaseBtn.FontSize = btnTakephotoFontSize;
            ReleaseBtn.Clicked += HandleReleaseBtn;


        }

        private void InitializeLocker(int lockerId)
        {
            this.locker = AzureApi.GetLocker(lockerId);
        }

        private void SetLocker(int lockerId)
        {

            LastPhotoLinkLabel.Text = "No Photo Available";
            if (photoTaken)
            {
                LastPhotoLinkLabel.Text = "Last photo of the locker";
            }


            string status = "not set";

            LockerIdLbl.Text = "Locker Id: " + lockerId;


            string timeRemainingStr = AzureApi.GetRemainingTime(locker);
            TimeRemainingLbl.Text = timeRemainingStr;


            if (this.locker.locked) status = "Locked";
            else status = "Unlocked";
            StatusLbl.Text = "Status: " + status;


            LockUnlockBtn.Text = locker.locked ? "Unlock" : "Lock";

        }
        void HandleTakeAPhotoBtn(object sender, System.EventArgs e)
        {
            photoTaken = true;
            SetLocker(this.locker.Id);
            AzureApi.TakeLockerPhoto(this.locker.Id + "");
        }
       void HandleLockUnlockBtn(object sender, System.EventArgs e)
        {

            if (!this.locker.locked)
            {
                AzureApi.SetLock(this.locker);
                this.locker.locked = true;
            }
            else
            { 
                AzureApi.SetUnlock(this.locker);
                this.locker.locked = false;
            }
            SetLocker(this.locker.Id);
        }
        async void HandleReleaseBtn(object sender, System.EventArgs e)
        {
            bool answer = await DisplayAlert("You will not be refunded for the remaining time on the clock", "are you sure you want to release the locker", "Yes", "No");
            if (answer)
            {
                AzureApi.SetAvailable(this.locker);
                this.locker.available = true;
                await Navigation.PopAsync();
            }

        }

        void Navigate_To_Photo(object sender, System.EventArgs e)
        {
            if(photoTaken)
                Navigation.PushAsync(new InsideALockerImage(this.locker.Id + ""));
        }
    }
}