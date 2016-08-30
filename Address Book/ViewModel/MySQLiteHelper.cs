using Address_Book.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Address_Book.ViewModel
{

    //This class for perform all database CRUID operations 
    class MySQLiteHelper
    {

        SQLiteConnection dbConn;
        private static MySQLiteHelper instance;

        private MySQLiteHelper() { }

        public static MySQLiteHelper Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MySQLiteHelper();
                    instance.onCreate();
                }
                return instance;
            }
        }

        //Create Tabble 
        public bool onCreate()
        {
            try
            {
                if (!CheckFileExists(App.DB_PATH).Result)
                {
                    using (dbConn = new SQLiteConnection(App.DB_PATH))
                    {
                        dbConn.CreateTable<ContactBook>();
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        private async Task<bool> CheckFileExists(string fileName)
        {
            try
            {
                var store = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFileAsync(fileName);
                return true;
            }
            catch
            {
                return false;
            }
        }

        // Retrieve the specific contact from the database. 
        public ContactBook ReadContact(int contactid)
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                var existingconact = dbConn.Query<ContactBook>("select * from ContactBook where IdContact =" + contactid).FirstOrDefault();
                return existingconact;
            }
        }
        // Retrieve the all contact list from the database. 
        public ObservableCollection<ContactBook> ReadContacts()
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                List<ContactBook> myCollection = dbConn.Table<ContactBook>().ToList<ContactBook>();
                ObservableCollection<ContactBook> ContactsList = new ObservableCollection<ContactBook>(myCollection);
                return ContactsList;
            }
        }

        //Update existing conatct 
        public void UpdateContact(ContactBook contact)
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                var existingconact = dbConn.Query<ContactBook>("select * from ContactBook where IdContact =" + contact.IdContact).FirstOrDefault();
                if (existingconact != null)
                {
                    existingconact.Name = contact.Name;
                    existingconact.Phone = contact.Phone;
                    existingconact.Address = contact.Address;
                    dbConn.RunInTransaction(() =>
                    {
                        dbConn.Update(existingconact);
                    });
                }
            }
        }
        // Insert the new contact in the Contacts table. 
        public void Insert(ContactBook newcontact)
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                dbConn.RunInTransaction(() =>
                {
                    dbConn.Insert(newcontact);
                });
            }
        }

        //Delete specific contact 
        public void DeleteContact(int Id)
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                var existingconact = dbConn.Query<ContactBook>("select * from ContactBook where IdContact =" + Id).FirstOrDefault();
                if (existingconact != null)
                {
                    dbConn.RunInTransaction(() =>
                    {
                        dbConn.Delete(existingconact);
                    });
                }
            }
        }
        //Delete all contactlist or delete Contacts table 
        public void DeleteAllContact()
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                //dbConn.RunInTransaction(() => 
                //   { 
                dbConn.DropTable<ContactBook>();
                dbConn.CreateTable<ContactBook>();
                dbConn.Dispose();
                dbConn.Close();
                //}); 
            }
        }
    }
}
