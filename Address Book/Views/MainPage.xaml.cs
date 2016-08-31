using Address_Book.Model;
using Address_Book.ViewModel;
using Address_Book.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Phone.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace Address_Book
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private List<ContactBook> listContactBook;

        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;

            //  MySQLiteHelper Db_Helper = new MySQLiteHelper();
            // Db_Helper.onCreate();
            // MySQLiteHelper.Instance.Insert(new ContactBook("Igor Vilar", "61 8300-5524", "Rua 666"));
            //   ContactBook contactBook = MySQLiteHelper.Instance.ReadContact(1);
            //   System.Diagnostics.Debug.WriteLine("contactBook nome: "+ contactBook.Name);

        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.

            GetContactBookData();

        }

        private void GetContactBookData()
        {
            listViewContacts.Items.Clear();
            listContactBook = new List<ContactBook>();
            listContactBook = MySQLiteHelper.Instance.ReadContacts();

            foreach (ContactBook value in listContactBook)
            {
                listViewContacts.Items.Add(value);
            }
        }


        private void AddContact(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(AddContactPage), null);
        }

        void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                e.Handled = true;
                Frame.GoBack();
            }
        }

        private void OnItemClickListView(object sender, ItemClickEventArgs e)
        {
            var data = (ContactBook)e.ClickedItem;
            Frame.Navigate(typeof(DetailsContactPage), data);

        }
    }
}
