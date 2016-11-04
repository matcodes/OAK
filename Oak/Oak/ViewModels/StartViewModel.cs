using Oak.Classes;
using Oak.Classes.Enums;
using Oak.Classes.Messages;
using Oak.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Oak.ViewModels
{
    #region StartViewModel
    public class StartViewModel : OakViewModel, IScannerServiceListener
    {
        #region Static members
        public static readonly string ALCOHOL_TAG = "ALCOHOL";
        public static readonly string OIL_TAG = "OIL";
        public static readonly string MILK_TAG = "MILK";
        public static readonly string WATER_TAG = "WATER";
        #endregion

        private readonly IScannerService _scannerService = null;
        private readonly IFileService _fileService = null;

        private readonly IOakCrossCorr _oakCrossCorr = null;

        private ScannerData[] _currentData = new ScannerData[] { };

        public StartViewModel() : base()
        {
            _scannerService = DependencyService.Get<IScannerService>();
            _scannerService.Listener = this;
            _fileService = DependencyService.Get<IFileService>();

            _oakCrossCorr = DependencyService.Get<IOakCrossCorr>();

            try
            {
                _scannerService.FindDevice();
            }
            catch (Exception exception)
            {
                ShowToastMessage.Send(exception.Message);
            }

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
            this.RetestCommand = new VisualCommand(this.Retest);
            this.CloseCommand = new VisualCommand(this.Close);
            this.CompareCommand = new VisualCommand(this.Compare);
            this.CheckCommand = new VisualCommand(this.Check);
            this.NextCommand = new VisualCommand(this.Next);
            this.TestResultContinueCommand = new VisualCommand(this.TestResultContinue);
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

        protected override void DoPropertyChanged(string propertyName)
        {
            if (propertyName == "State")
            {
                this.StartConnectionCommand.IsEnabled = (this.State == StartPageStates.WaitConnection);

                if ((this.State == StartPageStates.Programs) ||
                    (this.State == StartPageStates.Check))
                    this.SetProgramsParams();

                if (this.State == StartPageStates.Programs)
                    this.IsRetestVisible = false;

                if (this.State == StartPageStates.Check)
                    this.IsRetestVisible = true;
            }

            base.DoPropertyChanged(propertyName);
        }

        public override bool HandleBackButton()
        {
            var result = base.HandleBackButton();

            if (this.IsTestResultVisible)
            {
                this.TestResultContinue(null);
                result = true;
            }

            return result;
        }

        private string GetFileName(int index)
        {
            var product = "";
            if (this.ProductCategory == ProductCategories.Alcohol)
                product = ALCOHOL_TAG;
            else if (this.ProductCategory == ProductCategories.Milk)
                product = MILK_TAG;
            else if (this.ProductCategory == ProductCategories.Oil)
                product = OIL_TAG;
            else if (this.ProductCategory == ProductCategories.Water)
                product = WATER_TAG;

            var path = _fileService.AppWorkPath;
            var name = String.Format("oak_data_{0}_{1}.csv", product, index);

            return Path.Combine(path, name);
        }

        private string GetFileName()
        {
            var product = "";
            if (this.ProductCategory == ProductCategories.Alcohol)
                product = ALCOHOL_TAG;
            else if (this.ProductCategory == ProductCategories.Milk)
                product = MILK_TAG;
            else if (this.ProductCategory == ProductCategories.Oil)
                product = OIL_TAG;
            else if (this.ProductCategory == ProductCategories.Water)
                product = WATER_TAG;

            var path = _fileService.AppWorkPath;
            var name = String.Format("oak_data_{0}_{1}.csv", product, DateTime.Now.ToString().Replace("/", "_"));

            return Path.Combine(path, name);
        }

        private void SaveDataToCsv(int index)
        {
            Task.Run(() => {
                this.IsBusy = true;
                try
                {
                    var fileName = this.GetFileName(index);

                    this.SaveDataToCsv(fileName);

                    this.State = StartPageStates.Rescan;
                }
                catch (Exception exception)
                {
                    ShowToastMessage.Send(exception.Message);
                }
                finally
                {
                    this.IsBusy = false;
                }
            });
        }

        private void CheckData(int index)
        {
            Task.Run(() => {
                this.IsBusy = true;
                try
                {
                    var storeData = this.ReadDataFromCsv(index);

                    //var currentSample = _currentData.Select(sd => (double)sd.Y).ToArray();
                    //var storeSample = storeData.Select(sd => (double)sd.Y).ToArray();

                    var current = new List<Double>();
                    var store = new List<Double>();

                    foreach (var currentItem in _currentData)
                    {
                        var storeItem = storeData.FirstOrDefault(sd => sd.X == currentItem.X);
                        if (storeItem != null)
                        {
                            current.Add(currentItem.Y);
                            store.Add(storeItem.Y);
                        }
                    }

                    //var adv = _oakCrossCorr.ADV(current.ToArray(), store.ToArray());
                    //var fdav = _oakCrossCorr.FDAV(current.ToArray(), store.ToArray());
                    //var fdls = _oakCrossCorr.FDLS(current.ToArray(), store.ToArray());
                    //var idiff = _oakCrossCorr.IDIFF(current.ToArray(), store.ToArray());
                    //var iis = _oakCrossCorr.IS(current.ToArray(), store.ToArray());
                    //var ls = _oakCrossCorr.LS(current.ToArray(), store.ToArray());
                    //var savg = _oakCrossCorr.SAVG(current.ToArray(), store.ToArray());

                    var coeff = _oakCrossCorr.GetCoeff(current.ToArray(), store.ToArray());

                    var text = "";
                    var result = TestResults.OK;
                    if (coeff > 0.9)
                    {
                        result = TestResults.OK;
                        text = "SAFE ({0})";
                    }
                    else if ((coeff > 0.7) && (coeff <= 0.9))
                    {
                        result = TestResults.Warning;
                        text = "WARNING ({0})";
                    }
                    else
                    {
                        result = TestResults.Danger;
                        text = "DANGER ({0})";
                    }

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        this.TestResultText = String.Format(text, coeff.ToString("0.0000"));
                        this.TestResult = result;
                        this.IsTestResultVisible = true;
                    });
                 }
                catch (Exception exception)
                {
                    ShowToastMessage.Send(exception.Message);
                }
                finally
                {
                    this.IsBusy = false;
                }
            });
        }

        private void SaveDataToCsv(string fileName)
        {
            var text = String.Format("\"X\",\"Y\",\"N\"");
            foreach (var scannerData in _currentData)
            {
                text += Environment.NewLine;
                text += String.Format("\"{0}\",\"{1}\",\"{2}\"", scannerData.X, scannerData.Y, scannerData.N);
            }

            File.WriteAllText(fileName, text);

            this.SetProgramsParams();
        }

        private ScannerData[] ReadDataFromCsv(int index)
        {
            var fileName = this.GetFileName(index);
            if (!File.Exists(fileName))
                throw new Exception(String.Format("Data slot # {0} is empty.", index));

            var scannerDatas = new List<ScannerData>();

            var lines = File.ReadAllLines(fileName);
            var firstLine = true;
            foreach (var line in lines)
            {
                if (!firstLine)
                {
                    var values = line.Split(',');
                    var x = UInt32.Parse(values[0].Replace("\"", ""));
                    var y = UInt32.Parse(values[1].Replace("\"", ""));
                    var n = UInt16.Parse(values[2].Replace("\"", ""));
                    scannerDatas.Add(new ScannerData { X = x, Y = y, N = n });
                }
                else
                    firstLine = false;
            }

            return scannerDatas.ToArray();
        }

        private bool FileExist(int index)
        {
            return File.Exists(this.GetFileName(index));
        }

        private void SetProgramsParams()
        {
            this.Program1Exist = !this.FileExist(1);
            this.Program2Exist = !this.FileExist(2);
            this.Program3Exist = !this.FileExist(3);
            this.Program4Exist = !this.FileExist(4);
            this.Program5Exist = !this.FileExist(5);
            this.Program6Exist = !this.FileExist(6);
            this.Program7Exist = !this.FileExist(7);
            this.Program8Exist = !this.FileExist(8);
        }

        private void StartConnection(object parameter)
        {
            Task.Run(() => {
                try
                {
                    this.State = StartPageStates.Connecting;
                    var result = _scannerService.Connect();
                    //Task.Delay(2000).Wait();
                    this.State = StartPageStates.Connected;
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.Message);
                    ShowToastMessage.Send(exception.Message);
                    this.State = StartPageStates.WaitConnection;
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
            {
                this.IsTakePhoto = true;
                this.State = StartPageStates.Scan;
            }
        }

        private void Scan(object parameter)
        {
            Task.Run(() => {
                this.IsBusy = true;
                try
                {
                    this.SetProgress(0);
                    this.SetProgressVisible(true);
                    var result = _scannerService.Scan();
                    //Task.Delay(3000).Wait();
                    //var result = new ScannerData[] {
                    //    new ScannerData { X = 1, Y = 1, N = 1 },
                    //    new ScannerData { X = 2, Y = 2, N = 2 },
                    //    new ScannerData { X = 3, Y = 3, N = 3 },
                    //    new ScannerData { X = 4, Y = 4, N = 4 },
                    //    new ScannerData { X = 5, Y = 5, N = 5 },
                    //    new ScannerData { X = 6, Y = 6, N = 6 },
                    //    new ScannerData { X = 7, Y = 7, N = 7 },
                    //    new ScannerData { X = 8, Y = 8, N = 8 },
                    //    new ScannerData { X = 9, Y = 9, N = 9 }
                    //};
                    _currentData = result;
                    this.State = StartPageStates.Store;
                }
                catch (Exception exception)
                {
                    ShowToastMessage.Send(exception.Message);
                }
                finally
                {
                    this.SetProgressVisible(false);
                    this.IsBusy = false;
                }
            });
        }

        private void SetProgress(double value)
        {
            Device.BeginInvokeOnMainThread(() => {
                this.Progress = value;
            });
        }

        private void SetProgressVisible(bool visible)
        {
            Device.BeginInvokeOnMainThread(() => {
                this.IsProgressVisible = visible;
            });
        }

        private void Store(object parameter)
        {
            this.State = StartPageStates.Keep;
        }

        private void Test(object parameter)
        {
            this.State = StartPageStates.Check;
        }

        private void KeepInPhone(object parameter)
        {
            this.State = StartPageStates.Programs;
        }

        private void TransferFile(object parameter)
        {
            Task.Run(() => {
                this.IsBusy = true;
                try
                {
                    var fileName = this.GetFileName();
                    this.SaveDataToCsv(fileName);
                    SendFileByEmailMessage.Send(fileName);
                    this.State = StartPageStates.Rescan;
                }
                catch (Exception exception)
                {
                    ShowToastMessage.Send(exception.Message);
                }
                finally
                {
                    this.IsBusy = false;
                }
            });
        }

        private void Program1(object parameter)
        {
            if (this.State == StartPageStates.Programs)
                this.SaveDataToCsv(1);
            else if (this.State == StartPageStates.Check)
                this.CheckData(1);
        }

        private void Program2(object parameter)
        {
            if (this.State == StartPageStates.Programs)
                this.SaveDataToCsv(2);
            else if (this.State == StartPageStates.Check)
                this.CheckData(2);
        }

        private void Program3(object parameter)
        {
            if (this.State == StartPageStates.Programs)
                this.SaveDataToCsv(3);
            else if (this.State == StartPageStates.Check)
                this.CheckData(3);
        }

        private void Program4(object parameter)
        {
            if (this.State == StartPageStates.Programs)
                this.SaveDataToCsv(4);
            else if (this.State == StartPageStates.Check)
                this.CheckData(4);
        }

        private void Program5(object parameter)
        {
            if (this.State == StartPageStates.Programs)
                this.SaveDataToCsv(5);
            else if (this.State == StartPageStates.Check)
                this.CheckData(5);
        }

        private void Program6(object parameter)
        {
            if (this.State == StartPageStates.Programs)
                this.SaveDataToCsv(6);
            else if (this.State == StartPageStates.Check)
                this.CheckData(6);
        }

        private void Program7(object parameter)
        {
            if (this.State == StartPageStates.Programs)
                this.SaveDataToCsv(7);
            else if (this.State == StartPageStates.Check)
                this.CheckData(7);
        }

        private void Program8(object parameter)
        {
            if (this.State == StartPageStates.Programs)
                this.SaveDataToCsv(8);
            else if (this.State == StartPageStates.Check)
                this.CheckData(8);
        }

        private void Rescan(object parameter)
        {
            this.State = StartPageStates.Scan;
        }

        private void Retest(object parameter)
        {
            this.State = StartPageStates.Check;
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

        private void TestResultContinue(object parameter)
        {
            this.IsTestResultVisible = false;
            this.State = StartPageStates.Rescan;
        }

        #region IScannerServiceListener
        public void ScanProgress(double progress)
        {
            this.SetProgress(progress);
        }
        #endregion

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

        public bool Program1Exist
        {
            get { return (bool)this.GetValue("Program1Exist", false); }
            set { this.SetValue("Program1Exist", value); }
        }

        public bool Program2Exist
        {
            get { return (bool)this.GetValue("Program2Exist", false); }
            set { this.SetValue("Program2Exist", value); }
        }

        public bool Program3Exist
        {
            get { return (bool)this.GetValue("Program3Exist", false); }
            set { this.SetValue("Program3Exist", value); }
        }

        public bool Program4Exist
        {
            get { return (bool)this.GetValue("Program4Exist", false); }
            set { this.SetValue("Program4Exist", value); }
        }

        public bool Program5Exist
        {
            get { return (bool)this.GetValue("Program5Exist", false); }
            set { this.SetValue("Program5Exist", value); }
        }

        public bool Program6Exist
        {
            get { return (bool)this.GetValue("Program6Exist", false); }
            set { this.SetValue("Program6Exist", value); }
        }

        public bool Program7Exist
        {
            get { return (bool)this.GetValue("Program7Exist", false); }
            set { this.SetValue("Program7Exist", value); }
        }

        public bool Program8Exist
        {
            get { return (bool)this.GetValue("Program8Exist", false); }
            set { this.SetValue("Program8Exist", value); }
        }

        public bool IsTakePhoto
        {
            get { return (bool)this.GetValue("IsTakePhoto", false); }
            set { this.SetValue("IsTakePhoto", value); }
        }

        public TestResults TestResult
        {
            get { return (TestResults)this.GetValue("TestResult", TestResults.OK); }
            set { this.SetValue("TestResult", value); }
        }

        public string TestResultText
        {
            get { return (string)this.GetValue("TestResultText"); }
            set { this.SetValue("TestResultText", value); }
        }

        public bool IsTestResultVisible
        {
            get { return (bool)this.GetValue("IsTestResultVisible", false); }
            set { this.SetValue("IsTestResultVisible", value); }
        }

        public bool IsProgressVisible
        {
            get { return (bool)this.GetValue("IsProgressVisible", false); }
            set { this.SetValue("IsProgressVisible", value); }
        }

        public bool IsRetestVisible
        {
            get { return (bool)this.GetValue("IsRetestVisible", false); }
            set { this.SetValue("IsRetestVisible", value); }
        }

        public double Progress
        {
            get { return (double)this.GetValue("Progress", (double)0); }
            set { this.SetValue("Progress", value); }
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

        public VisualCommand RetestCommand { get; private set; }

        public VisualCommand CloseCommand { get; private set; }

        public VisualCommand CompareCommand { get; private set; }

        public VisualCommand CheckCommand { get; private set; }

        public VisualCommand NextCommand { get; private set; }

        public VisualCommand TestResultContinueCommand { get; private set; }
    }
    #endregion
}
