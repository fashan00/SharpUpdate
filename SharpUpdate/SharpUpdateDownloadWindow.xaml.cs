using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.IO;

namespace SharpUpdate
{
    /// <summary>
    /// SharpUpdateDownloadWindow.xaml 的互動邏輯
    /// </summary>
    internal partial class SharpUpdateDownloadWindow : Window
    {
        private WebClient webClient;

        private BackgroundWorker bgWorker;

        private string tempFile;

        /// <summary>
        /// MD5算法常常被用來驗證文件的完整性，防止文件被人篡改。
        /// MD5全稱是報文摘要算法（Message-Digest Algorithm 5），此算法對任意長度的信息逐位進行計算，產生一個二進制長度為128位（十六進制長度就是32位）的"指紋"（或稱"報文摘要")。
        /// 不同的文件產生相同的報文摘要的可能性是非常非常之小的
        /// </summary>
        private string md5;

        /// <summary>
        /// 是否使用MD5來檢核檔案是否正確。 
        /// 因為每次更換檔案都需產生一組MD5 File Checksum(http://onlinemd5.com/)，目前考慮更新到xml步驟較多，且無安全性需求，所以暫時先不用 by hashan00 2015.07.21
        /// </summary>
        public bool IsUseMD5 { get; set; }

        internal string TempFilePath
        {
            get { return tempFile; }
        }

        internal SharpUpdateDownloadWindow(Uri location, string md5, Image programIcon)
        {
            InitializeComponent();

            if (programIcon != null)
                this.Icon = programIcon.Source;

            tempFile = Path.GetTempFileName();

            this.md5 = md5; 

            webClient = new WebClient();
            webClient.DownloadProgressChanged += webClient_DownloadProgressChanged;
            webClient.DownloadFileCompleted += webClient_DownloadFileCompleted;
         
            bgWorker = new BackgroundWorker();
            bgWorker.DoWork += bgWorker_DoWork;
            bgWorker.RunWorkerCompleted += bgWorker_RunWorkerCompleted;

            try
            {
                webClient.DownloadFileAsync(location, this.tempFile);
            }
            catch (Exception)
            {
                DialogResult = null; // null為發生錯誤
            }

        }

        private void webClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            this.ProgressBar.Value = e.ProgressPercentage;
            this.lblProgress.Content = string.Format("Downloaded {0} of {1}", FormatBytes(e.BytesReceived), FormatBytes(e.TotalBytesToReceive)); 
        }

        private string FormatBytes(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB" };
            double len = bytes;
            int order = 0;
            while (len >= 1024 && order + 1 < sizes.Length)
            {
                order++;
                len = len / 1024;
            }

            // Adjust the format string to your preferences. For example "{0:0.#}{1}" would
            // show a single decimal place, and no space.
            string result = String.Format("{0:0.0}{1}", len, sizes[order]);

            return result;
        }

        private void webClient_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                DialogResult = null; // null為發生錯誤
            }
            else if (e.Cancelled)
            {
                DialogResult = false;
            }
            else
            {
                lblProgress.Content = "Verifying Download...";
                ProgressBar.IsIndeterminate = true;
                bgWorker.RunWorkerAsync(new string[] { this.tempFile, this.md5});
            }

        }

        private void bgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            string file = ((string[]) e.Argument)[0];
            string updateMd5 = ((string[]) e.Argument)[1];

            if (IsUseMD5)
            {
                if (Hasher.HashFile(file, HashType.MD5) != updateMd5)
                    e.Result = false;
                else
                    e.Result = true;
            }
            else
                e.Result = true;
        }

        private void bgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.DialogResult = (bool?)  e.Result;
        }

        private void SharpUpdateDownloadWindow_OnClosed(object sender, EventArgs e)
        {
            if (webClient.IsBusy)
            {
                webClient.CancelAsync();
                DialogResult = false;
            }

            if (bgWorker.IsBusy)
            {
                bgWorker.CancelAsync();
                DialogResult = false;
            }
        }


        

    }
}
