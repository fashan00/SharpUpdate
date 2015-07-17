using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
using SharpUpdate;

namespace TestApp
{
    /// <summary>
    /// MainUpdateUserControl.xaml 的互動邏輯
    /// </summary>
    public partial class MainUpdateUserControl : UserControl, ISharpUpdatable
    {
        public MainUpdateUserControl()
        {
            InitializeComponent();

            this.Label.Content = this.ApplicationAssembly.GetName().Version.ToString();
        }

        private void CheckUpdateBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        public string ApplicationName
        {
            get
            {
                return "TestApp";
            }
        }

        public string ApplicationID
        {
            get
            {
                return "testApp";
            }
        }

        public Assembly ApplicationAssembly
        {
            get
            {
                return Assembly.GetExecutingAssembly();
            }
        }

        private Image icon;
        public Image ApplicationIcon
        {
            get
            {
                return this.icon;
            }
            set
            {
                this.icon = value;
            }
        }

        public Uri UpdateXmlLocation
        {
            get
            {
                return new Uri("");
            }
        }

        public UserControl Context
        {
            get
            {
                return this;
            }
        }
    }
}
