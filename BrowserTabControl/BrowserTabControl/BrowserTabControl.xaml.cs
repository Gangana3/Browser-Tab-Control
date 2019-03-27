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
    /// Interaction logic for TabControl.xaml
    /// </summary>
    public partial class MyBrowserTabControl : UserControl
    {
        // Private constants
        private const double PlusButtonMarginTop = 5;       // The margin of the + button from top
        private const double PlusButtonMarginLeft = 5;      // The margin of the + button from top

        // Private fields
        private List<TabItem> tabItems;                     // List of all the tab items within the tab control

        // public Properties
        public int TabCount                                 // Counts the number of tabs
        {
            get
            {
                return this.tabItems.Count;
            }
        }                   

        // public events
        public event EventHandler NewTabButtonClick;        // Event for adding a new tab
        public event EventHandler TabClosed;                // Event for closing a tab

        public MyBrowserTabControl()
        {
            InitializeComponent();
            this.tabItems = new List<TabItem>();
            this.tabControl.SizeChanged += this.TabControlSizeChangedHandler;

            // Margin the add new tab button
            this.addNewTabButton.Margin = this.AddNewTabMargin;
        }

        /// <summary>
        /// Adds tab to the tab control
        /// </summary>
        /// <param name="header">tab's header</param>
        /// <param name="content">tab's content</param>
        public void AddTab(string header, UIElement content)
        {
            // Create a tab item and add it to the tab control;
            TabItem item = this.CreateTabItem(header);
            item.Content = content;                         // Put the given content inside the tab item
            this.tabControl.Items.Add(item);                // Add the complete tab to the UI
            this.tabItems.Add(item);

            // Move the + button right
            this.addNewTabButton.Margin = this.AddNewTabMargin;
        }

        #region Private properties
        private double TotalHeadersWidth
        {
            get
            {
                double sum = 0;
                foreach (TabItem item in this.tabItems)
                    sum += item.Width;
                return sum;
            }
           
        }
        /// <summary>
        /// Current margin of the add new tab button
        /// </summary>
        private Thickness AddNewTabMargin
        {
            get
            {
                double sum = this.TotalHeadersWidth;
                return new Thickness(sum + PlusButtonMarginLeft, PlusButtonMarginTop, 0, 0);
            }
        }
        #endregion

        #region Events Functions
        /// <summary>
        /// Should be called when a new tab is created
        /// </summary>
        protected virtual void OnNewTabButtonClick()
        {
            if (this.NewTabButtonClick != null)
                this.NewTabButtonClick(this, EventArgs.Empty);
        }

        /// <summary>
        /// Should be called when a tab is closed
        /// </summary>
        protected virtual void OnTabClosed()
        {
            if (this.TabClosed != null)
                this.TabClosed(this, EventArgs.Empty);
        }
        #endregion

        #region Event Handlers
        private void TabControlSizeChangedHandler(object sender, SizeChangedEventArgs e)
        {
            double addNewTabButtonWidth = PlusButtonMarginLeft * 2 + this.addNewTabButton.ActualWidth;  // Size of the new tab button including the margins

            if (this.TabCount > 0)
            {
                if (this.TabCount * this.tabItems[0].MaxWidth > this.tabControl.ActualWidth - addNewTabButtonWidth)
                {
                    foreach (TabItem item in this.tabItems)
                        item.Width = (this.tabControl.ActualWidth - addNewTabButtonWidth) / this.TabCount;
                    this.addNewTabButton.Margin = this.AddNewTabMargin;
                }
            }
            
        }
        #endregion

        #region private functions
        /// <summary>
        /// Executes when the add new tab button is clicked
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">event args</param>
        private void AddNewTabButton_Click(object sender, RoutedEventArgs e)
        {
            this.OnNewTabButtonClick();
        }

        /// <summary>
        /// Creates a new tab item
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        private TabItem CreateTabItem(string header)
        {
            TabItem item = new TabItem();
            // Create all the UI elements
            var dockPanel = new DockPanel();
            var label = new Label()
            {
                Content = header,
                Style = FindResource("headerLabel") as Style
            };
            var imageWrapper = new Border()
            {
                Style = this.FindResource("xButtonImageWrapper") as Style
            };
            var image = new Image()
            {
                Style = this.FindResource("xImage") as Style
            };

            // Arrange the UI elements together and set the header to the result
            imageWrapper.Child = image;
            label.Name = "title";
            dockPanel.Children.Add(label);
            dockPanel.Children.Add(imageWrapper);
            item.Header = dockPanel;

            // Handle the x button press
            imageWrapper.MouseDown += (object sender, MouseButtonEventArgs e) =>
            {
                this.tabItems.Remove(item);                                     // Decrease the tabCount
                this.tabControl.Items.Remove(item);                             // Remove the tab from the UI
                this.addNewTabButton.Margin = this.AddNewTabMargin;    // Margin the add new tab button
                this.OnTabClosed();
            };

            return item;
        }
        #endregion
    }
}
