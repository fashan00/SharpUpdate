using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// SharpUpdateInfo.xaml 的互動邏輯
    /// </summary>
    public partial class SharpUpdateInfoWindow : Window
    {
    
        public void SharpUpdateInfo(ISharpUpdatable applicationInfo, SharpUpdateXml updateInfo)
        {
            InitializeComponent();

            if (applicationInfo.ApplicationIcon != null)
                this.Icon = applicationInfo.ApplicationIcon.Source;

            this.Title = applicationInfo.ApplicationName + " - Update Info";

            this.lblDescription.Content = string.Format("Current Version: {0}\nUpdate Version: {1}",
                applicationInfo.ApplicationAssembly.GetName().Version, updateInfo.Version);

            this.txtDescription.Document.Blocks.Clear();
            this.txtDescription.Document.Blocks.Add(new Paragraph(new Run(updateInfo.Description)));
            
        }
        
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

    }
}
