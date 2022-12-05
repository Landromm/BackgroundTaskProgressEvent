using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using PastebinAPI;


namespace BackgroundTaskProgressEvent
{
    public partial class Form1 : Form
    {
        private BackgroundWorker backgroundWorker;

        public Form1()
        {
            InitializeComponent();
            //InitControl();
            this.button1.Click += new EventHandler(this.startAsyncButton_Click);
        }

        //private void InitControl()
        //{
        //    //this.backgroundWorker = new BackgroundWorker();
        //    //this.SuspendLayout();
        //    //this.button1.Click += new EventHandler(this.startAsyncButton_Click);

        //    //this.backgroundWorker.WorkerReportsProgress = true;
        //    //this.backgroundWorker.WorkerSupportsCancellation = true;
        //    //backgroundWorker.DoWork += new DoWorkEventHandler(backgroundWorker_DoWork);
        //    //backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker_RunWorkerCompleted);
        //    //backgroundWorker.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker_ProgressChanged);

        //    //this.ResumeLayout(false);
        //}

        private void startAsyncButton_Click(object sender, EventArgs e)
        {
            //backgroundWorker.RunWorkerAsync();
            testTask();
        }

        private async void testTask()
        {

            var list = Enumerable.Range(1, 100).Select(i => i.ToString()).ToArray();
            var progress = new Progress<ProgressInfo>(TimeSpan.FromMilliseconds(100), UpdateProgressAction);

            await PastebinExample(progress);
        }

        private void UpdateProgressAction(ProgressInfo obj)
        {
            progressBar1.Value = (int)obj.CompletedPercentage;
            //OperationStatus.Text = obj.ProgressStatusText;
            //ProgressBarPercentage.Text = string.Format("{0:P}", obj.CompletedPercentage);
        }

        
        public async Task LongMethod(IReadOnlyList<string> something, IProgress<ProgressInfo> progress)
        {
            await Task.Factory.StartNew(() =>
            {
                progress.Report(new ProgressInfo((double)(25), "25"));
                progress.Report(new ProgressInfo((double)(50), "50"));
                progress.Report(new ProgressInfo((double)(75), "75"));
                progress.Report(new ProgressInfo((double)(100), "100"));
            });
        }

        //private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        //{
        //    prograssBar.Value = e.ProgressPercentage;
        //}

        //private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    if (e.Error != null)
        //    {
        //        MessageBox.Show(e.Error.Message);
        //    }
        //    else if (e.Cancelled)
        //    {
        //        // Next, handle the case where the user canceled
        //        // the operation.
        //        // Note that due to a race condition in
        //        // the DoWork event handler, the Cancelled
        //        // flag may not have been set, even though
        //        // CancelAsync was called.
        //        Console.WriteLine("Cancelled");

        //    }
        //    else
        //    {
        //        // Finally, handle the case where the operation
        //        // succeeded.
        //        Console.WriteLine("GOOG FINALY");
        //    }
        //}

        //private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    BackgroundWorker worker = sender as BackgroundWorker;

        //    // Assign the result of the computation
        //    // to the Result property of the DoWorkEventArgs
        //    // object. This is will be available to the
        //    // RunWorkerCompleted eventhandler.
        //    e.Result = PastebinExample(worker, e).GetAwaiter();

        //}

        //int MethodDeploy(BackgroundWorker worker, DoWorkEventArgs e)
        //{
        //    if (worker.CancellationPending)
        //    {
        //        e.Cancel = true;
        //    }
        //    else
        //    {
        //        // Report progress as a percentage of the total task.
        //        int percentComplete = 10;
        //        for (int i = 0; i < 100; i+=10)
        //        {
        //            Thread.Sleep(500);
        //            Console.WriteLine(percentComplete);
        //            percentComplete += 10;
        //            if(percentComplete <= 100)
        //                worker.ReportProgress(percentComplete);
        //        }
        //    }
        //    return 100;
        //}

        public async Task PastebinExample(IProgress<ProgressInfo> progress)
        {
            //before using any class in the api you must enter your api dev key
            Pastebin.DevKey = "iCv_LuM23pCxy1OUT-OgWkYx7ofrWx4z";
            //you can see yours here: https://pastebin.com/api#1
            try
            {
                progress.Report(new ProgressInfo((double)(10), "10"));

                // login and get user object
                User me = await Pastebin.LoginAsync("Landromm", "Pastebin_21!03@1993#");
                // user contains information like e-mail, location etc...
                Console.WriteLine("{0}({1}) lives in {2}", me, me.Email, me.Location);
                // lists all pastes for this user
                progress.Report(new ProgressInfo((double)(25), "25"));

                string code = "<your fancy &code#() goes here>";
                ////creates a new paste and get paste object
                //Paste newPaste = await me.CreatePasteAsync(code, "MyPasteTitle", Language.HTML5, Visibility.Public, Expiration.TenMinutes);
                ////newPaste now contains the link returned from the server
                //Console.WriteLine("URL: {0}", newPaste.Url);
                //Console.WriteLine("Paste key: {0}", newPaste.Key);
                //Console.WriteLine("Content: {0}", newPaste.Text);


                foreach (Paste paste in await me.ListPastesAsync(10)) // we limmit the results to 3
                {
                    if (paste.Title.Equals("MyPasteTitle"))
                    {
                        Console.WriteLine(@paste.GetRawAsync().Result);
                        progress.Report(new ProgressInfo((double)(75), "75"));
                    }
                }
                progress.Report(new ProgressInfo((double)(100), "100"));

                //if (percentComplete < 100)
                //    worker.ReportProgress(0);

                //deletes the paste we just created
                //await me.DeletePasteAsync(newPaste);

                //lists all currently trending pastes(similar to me.ListPastes())
                //foreach (Paste paste in await Pastebin.ListTrendingPastesAsync())
                //{
                //    Console.WriteLine("{0} - {1}", paste.Title, paste.Url);
                //}
                //you can create pastes directly from Pastebin static class but they are created as guests and you have a limited number of guest uploads
                //Paste anotherPaste = await Paste.CreateAsync("another paste", "MyPasteTitle2", Language.CSharp, Visibility.Unlisted, Expiration.OneHour);
                //Console.WriteLine(anotherPaste.Title);
            }
            catch (PastebinException ex) //api throws PastebinException
            {
                //in the Parameter property you can see what invalid parameter was sent
                //here we check if the exeption is thrown because of invalid login details
                if (ex.Parameter == PastebinException.ParameterType.Login)
                {
                    Console.Error.WriteLine("Invalid username/password");
                }
                else
                {
                    throw; //all other types are rethrown and not swalowed!
                }
            }
        }
    }

    public class ProgressInfo : IProgressInfo
    {
        public ProgressInfo(double completedPercentage, string progressStatusText)
        {
            CompletedPercentage = completedPercentage;
            ProgressStatusText = progressStatusText;
        }

        public double CompletedPercentage { get; private set; }

        public string ProgressStatusText { get; private set; }

        public bool IsCompleted
        {
            get { return CompletedPercentage >= 1; }
        }
    }
    public class Progress<T> : IProgress<T> where T : class, IProgressInfo
    {
        private T _previousProgressInfo;
        private volatile T _progressInfo;
        private readonly Action<T> _updateProgressAction;
        private readonly System.Threading.Timer _timer;
        private readonly SynchronizationContext _synchronizationContext;

        public Progress(TimeSpan pollingInterval, Action<T> updateProgressAction)
        {
            _synchronizationContext = SynchronizationContext.Current ?? new SynchronizationContext();
            _updateProgressAction = updateProgressAction;
            _timer = new System.Threading.Timer(TimerCallback, null, pollingInterval, pollingInterval);
        }

        private void TimerCallback(object state)
        {
            ProcessUpdate();
        }

        private void ProcessUpdate()
        {
            var progressInfo = _progressInfo;
            if (_previousProgressInfo != progressInfo)
            {
                _synchronizationContext.Send(state => _updateProgressAction((T)state), progressInfo);
            }
            _previousProgressInfo = progressInfo;
        }

        public void Report(T value)
        {
            _progressInfo = value;
            if (value.IsCompleted)
            {
                _timer.Dispose();
                ProcessUpdate();
            }
        }
    }

}
