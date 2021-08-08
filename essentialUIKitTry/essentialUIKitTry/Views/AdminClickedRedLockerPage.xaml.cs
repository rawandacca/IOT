using CounterFunctions;
using System.Drawing;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;
using Xamarin.Forms;
using System;

namespace essentialUIKitTry.Views
{
    /// <summary>
    /// Page to show chat profile page
    /// </summary>
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AdminClickedRedLockerPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AdminClickedRedLockerPage" /> class.
        /// </summary>
        private Locker locker;
        private bool photoTaken = false;
        public AdminClickedRedLockerPage(int lockerId)
        {
            this.InitializeComponent();
            InitializeLocker(lockerId);
            InitializePageComponent();
            SetLocker(lockerId);
        }

        private void InitializePageComponent()
        {
           
            LockUnlockBtn.Clicked += HandleLockUnlockBtn;
            ReleaseBtn.Text = "Release Locker";         
            ReleaseBtn.Clicked += HandleReleaseBtn;
        }

        private void InitializeLocker(int lockerId)
        {
            locker = AzureApi.GetLocker(lockerId);
        }

        void SetLocker(int lockerId)
        {

            string status = "not set";
            
            LockerIdLbl.Text = "Locker Id: " + lockerId;

            this.mailOfUser.Text = $"Mail of Customer: {locker.user_key}";

            string timeRemainingStr = AzureApi.GetRemainingTime(locker);
            TimeRemainingLbl.Text = "Remaing time: " + timeRemainingStr;


            if (this.locker.locked) status = "Locked";
            else status = "Unlocked";
            StatusLbl.Text = "Status: " + status;

            LockUnlockBtn.Text = this.locker.locked ? "Unlock" : "Lock";

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
            this.locker.available = true;
            AzureApi.SetAvailable(this.locker.Id);
            await Navigation.PopAsync();

        }

    }
}