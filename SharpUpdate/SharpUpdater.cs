using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;

namespace SharpUpdate
{
    public class SharpUpdater
    {
        private ISharpUpdatable applicationInfo;
        private BackgroundWorker bgWorker;

        public SharpUpdater(ISharpUpdatable applicationInfo)
        {
            this.applicationInfo = applicationInfo;

            this.bgWorker = new BackgroundWorker();
            this.bgWorker.DoWork += bgWorker_DoWork;
            this.bgWorker.RunWorkerCompleted += bgWorker_RunWorkerCompleted;
        }

        public void DoUpdate()
        {
            if(!this.bgWorker.IsBusy)
                this.bgWorker.RunWorkerAsync(this.applicationInfo);
        }

        private void bgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            var application = (ISharpUpdatable) e.Argument;

            if (!SharpUpdateXml.ExistOnServer(application.UpdateXmlLocation))
                e.Cancel = true;
            else
                e.Result = SharpUpdateXml.Parse(application.UpdateXmlLocation, application.ApplicationID);

        }

        private void bgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!e.Cancelled)
            {
                var update = (SharpUpdateXml) e.Result;

                if (update != null && update.IsNewerThan(this.applicationInfo.ApplicationAssembly.GetName().Version))
                {
                    var updateAcceptWindow = new SharpUpdateAcceptWindow(this.applicationInfo, update);
                    updateAcceptWindow.Owner = this.applicationInfo.Context;
                    if (updateAcceptWindow.ShowDialog() == true)
                    {
                        this.DownloadUpdate(update);
                    }
                }
                else
                    MessageBox.Show("You software is up to date.", "Update", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void DownloadUpdate(SharpUpdateXml update)
        {
            var updateDownloadWindow = new SharpUpdateDownloadWindow(update.Uri, update.MD5, this.applicationInfo.ApplicationIcon);
            updateDownloadWindow.Owner = this.applicationInfo.Context;
            updateDownloadWindow.IsUseMD5 = false;
            bool? result = updateDownloadWindow.ShowDialog();
            if (result == true)
            {
                string currentPath = this.applicationInfo.ApplicationAssembly.Location;
                string newPath = Path.GetDirectoryName(currentPath) + "\\" + update.FileName;

                UpdateApplication(updateDownloadWindow.TempFilePath, currentPath, newPath, update.LunchArgs);

                //Environment.Exit(0);// 强制退出，即使有其他的執行緒没有结束

                Application.Current.Shutdown(); // 關閉當前程序，如果有其他執行緒没有结束，不會關閉

            }
            else if (result == false)
            {
                MessageBox.Show("The update download was cancelled.\nThis program has not been modified.",
                    "Update Download Cancelled", MessageBoxButton.OK,MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("There was a problem downloading the update.\nPlease try again later.",
                    "Update Download Error", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void UpdateApplication(string tempFilePath, string currentPath, string newPath, string lunchArgs)
        {
            string exe = Path.GetDirectoryName(tempFilePath) + "\\" + Path.GetFileName(newPath);
            
            try
            {
                File.Move(tempFilePath, exe);
                ProcessStartInfo info = new ProcessStartInfo();
                //info.WindowStyle = ProcessWindowStyle.Hidden;
                //info.CreateNoWindow = false;
                //info.UseShellExecute = true;
                //info.Verb = "runas";
                //info.WorkingDirectory = Path.GetDirectoryName(tempFilePath);
                //info.FileName = Path.GetFileName(exe);
                info.FileName = exe;
                info.Arguments = "/S";

                var process = new Process();
                process.StartInfo = info;
                process.Start();
                process.WaitForExit();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(),
                    "Install Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                File.Delete(exe);
            }

        }

    }
}
