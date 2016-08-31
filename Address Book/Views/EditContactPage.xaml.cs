using Address_Book.Model;
using Address_Book.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Address_Book.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EditContactPage : Page
    {
        private ContactBook contactBook = new ContactBook(); 
        public EditContactPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            contactBook = e.Parameter as ContactBook;
            if (contactBook != null)
            {
                //Do your stuff
                textBoxName.Text = contactBook.Name;
                textBoxPhone.Text = contactBook.Phone;
                textBoxAddress.Text = contactBook.Address;

            }
        }

        private async void SaveContact(object sender, RoutedEventArgs e)
        {
            contactBook.Name = textBoxName.Text;
            contactBook.Phone = textBoxPhone.Text;
            contactBook.Address = textBoxAddress.Text;
            if (textBoxName.Text.Equals(""))
            {
                MessageDialog msgbox = new MessageDialog("Informe o nome do contato", "Alert");
                await msgbox.ShowAsync();
                return;
            }
            MySQLiteHelper.Instance.UpdateContact(contactBook);
            Frame.GoBack();
            Frame.GoBack();
        }

        private void RemoveContact(object sender, RoutedEventArgs e)
        {

        }
    }
}
