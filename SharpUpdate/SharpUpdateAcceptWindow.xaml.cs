using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SharpUpdate
{
    /// <summary>
    /// SharpUpdateAcceptWindow.xaml 的互動邏輯
    /// </summary>
    internal partial class SharpUpdateAcceptWindow : Window
    {
        private ISharpUpdatable applicationInfo;

        private SharpUpdateXml updateInfo;

        private SharpUpdateInfoWindow updateInfoWindow;

        internal SharpUpdateAcceptWindow(ISharpUpdatable applicationInfo, SharpUpdateXml updateInfo)
        {
            InitializeComponent();

            this.applicationInfo = applicationInfo;
            this.updateInfo = updateInfo;

            this.Title = applicationInfo.ApplicationName + " - Update Available";
            if (applicationInfo.ApplicationIcon != null)
                this.Icon = applicationInfo.ApplicationIcon.Source;

            lblNewVersion.Content = string.Format("New Version {0}", updateInfo.Version);
            
        }

        private void YesBtn_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void NoBtn_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void DetailsBtn_OnClick(object sender, RoutedEventArgs e)
        {
            if (this.updateInfoWindow == null)
            {
                this.updateInfoWindow = new SharpUpdateInfoWindow(this.applicationInfo, this.updateInfo);
                updateInfoWindow.Owner = this;

                updateInfoWindow.ShowDialog();

                updateInfoWindow = null;
            }

            
        }

    }
}
