using Oak.Classes;
using Oak.Classes.Enums;
using Oak.Classes.Messages;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Oak.ViewModels
{
    #region StartViewModel
    public class StartViewModel : OakViewModel
    {
        public StartViewModel() : base()
        {
            this.StartConnectionCommand = new VisualCommand(this.StartConnection);
            this.AlcoholCommand = new VisualCommand(this.Alcohol);
            this.OilCommand = new VisualCommand(this.Oil);
            this.MilkCommand = new VisualCommand(this.Milk);
            this.WaterCommand = new VisualCommand(this.Water);
            this.TakePhotoCommand = new VisualCommand(this.TakePhoto);
            this.ScanCommand = new VisualCommand(this.Scan);
            this.StoreCommand = new VisualCommand(this.Store);
            this.TestCommand = new VisualCommand(this.Test);
            this.KeepInPhoneCommand = new VisualCommand(this.KeepInPhone);
            this.TransferFileCommand = new VisualCommand(this.TransferFile);
            this.Program1Command = new VisualCommand(this.Program1);
            this.Program2Command = new VisualCommand(this.Program2);
            this.Program3Command = new VisualCommand(this.Program3);
            this.Program4Command = new VisualCommand(this.Program4);
            this.Program5Command = new VisualCommand(this.Program5);
            this.Program6Command = new VisualCommand(this.Program6);
            this.Program7Command = new VisualCommand(this.Program7);
            this.Program8Command = new VisualCommand(this.Program8);
            this.RescanCommand = new VisualCommand(this.Rescan);
            this.CloseCommand = new VisualCommand(this.Close);
            this.CompareCommand = new VisualCommand(this.Compare);
            this.CheckCommand = new VisualCommand(this.Check);
            this.NextCommand = new VisualCommand(this.Next);
        }

        public override void Initialize(params object[] parameters)
        {
            base.Initialize(parameters);
        }

        public override void Appering()
        {
            base.Appering();
        }

        public override void Disappering()
        {
            base.Disappering();
        }

        public override bool HandleBackButton()
        {
            var result = base.HandleBackButton();


            return result;
        }

        protected override void DoPropertyChanged(string propertyName)
        {
            if (propertyName == "State")
            {
                this.StartConnectionCommand.IsEnabled = (this.State == StartPageStates.WaitConnection);
            }

            base.DoPropertyChanged(propertyName);
        }

        private void StartConnection(object parameter)
        {
            Task.Run(() => {
                this.State = StartPageStates.Connecting;
                try
                {
                    Task.Delay(5000).Wait();
                    this.State = StartPageStates.Connected;
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.Message);
//                    ShowToastMessage.Send(exception.Message);
                }
                finally
                {
                }
            });
        }

        private void SetProductCategory(ProductCategories productCategory)
        {
            this.ProductCategory = productCategory;
            this.State = StartPageStates.CameraHelp;
        }

        private void Alcohol(object parameter)
        {
            this.SetProductCategory(ProductCategories.Alcohol);
        }

        private void Oil(object parameter)
        {
            this.SetProductCategory(ProductCategories.Oil);
        }

        private void Milk(object parameter)
        {
            this.SetProductCategory(ProductCategories.Water);
        }

        private void Water(object parameter)
        {
            this.SetProductCategory(ProductCategories.Oil);
        }

        private void TakePhoto(object parameter)
        {
            if (this.State == StartPageStates.CameraHelp)
                this.State = StartPageStates.Camera;
            else if (State == StartPageStates.Camera)
                this.State = StartPageStates.Scan;
        }

        private void Scan(object parameter)
        {
            this.State = StartPageStates.Store;
        }

        private void Store(object parameter)
        {
            this.State = StartPageStates.Keep;
        }

        private void Test(object parameter)
        {
            this.State = StartPageStates.Compare;
        }

        private void KeepInPhone(object parameter)
        {
            this.State = StartPageStates.Programs;
        }

        private void TransferFile(object parameter)
        {
            this.State = StartPageStates.Rescan;
        }

        private void Program1(object parameter)
        {
            this.State = StartPageStates.Rescan;
        }

        private void Program2(object parameter)
        {
            this.State = StartPageStates.Rescan;
        }

        private void Program3(object parameter)
        {
            this.State = StartPageStates.Rescan;
        }

        private void Program4(object parameter)
        {
            this.State = StartPageStates.Rescan;
        }

        private void Program5(object parameter)
        {
            this.State = StartPageStates.Rescan;
        }

        private void Program6(object parameter)
        {
            this.State = StartPageStates.Rescan;
        }

        private void Program7(object parameter)
        {
            this.State = StartPageStates.Rescan;
        }

        private void Program8(object parameter)
        {
            this.State = StartPageStates.Rescan;
        }

        private void Rescan(object parameter)
        {
            this.State = StartPageStates.Store;
        }

        private void Close(object parameter)
        {
            CloseAppMessage.Send();
        }

        private void Compare(object parameter)
        {
            this.State = StartPageStates.Check;
        }

        private void Check(object parameter)
        {
        }

        private void Next(object parameter)
        {
        }

        public StartPageStates State
        {
            get { return (StartPageStates)this.GetValue("State", StartPageStates.Starting); }
            set { this.SetValue("State", value); }
        }

        public ProductCategories ProductCategory
        {
            get { return (ProductCategories)this.GetValue("ProductCategory", ProductCategories.Alcohol); }
            set { this.SetValue("ProductCategory", value); }
        }

        public VisualCommand StartConnectionCommand { get; private set; }

        public VisualCommand AlcoholCommand { get; private set; }

        public VisualCommand OilCommand { get; private set; }

        public VisualCommand MilkCommand { get; private set; }

        public VisualCommand WaterCommand { get; private set; }

        public VisualCommand TakePhotoCommand { get; private set; }

        public VisualCommand ScanCommand { get; private set; }

        public VisualCommand StoreCommand { get; private set; }

        public VisualCommand TestCommand { get; private set; }

        public VisualCommand KeepInPhoneCommand { get; private set; }

        public VisualCommand TransferFileCommand { get; private set; }

        public VisualCommand Program1Command { get; private set; }

        public VisualCommand Program2Command { get; private set; }

        public VisualCommand Program3Command { get; private set; }

        public VisualCommand Program4Command { get; private set; }

        public VisualCommand Program5Command { get; private set; }

        public VisualCommand Program6Command { get; private set; }

        public VisualCommand Program7Command { get; private set; }

        public VisualCommand Program8Command { get; private set; }

        public VisualCommand RescanCommand { get; private set; }

        public VisualCommand CloseCommand { get; private set; }

        public VisualCommand CompareCommand { get; private set; }

        public VisualCommand CheckCommand { get; private set; }

        public VisualCommand NextCommand { get; private set; }
    }
    #endregion
}
