using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BrowserTabControl
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Add one tab when the window starts
            this.browserTabControl.AddTab("New Tab", null);

            // Add a new tab when the plus button is clicked
            this.browserTabControl.NewTabButtonClick += (object sender, EventArgs e) =>
            {
                this.browserTabControl.AddTab("New Tab", null);
            };
        }
    }
}
