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

        private SQLiteConnection dbConn;
        private static MySQLiteHelper instance;

        private MySQLiteHelper() { }

        public static MySQLiteHelper Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MySQLiteHelper();
                    if (instance.onCreate()== true)
                    {
                        instance.dbConn = new SQLiteConnection(App.DB_PATH);
                    }
                }
                return instance;
            }
        }

        //Create Tabble 
        private bool onCreate()
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
                var existingconact = instance.dbConn.Query<ContactBook>("select * from ContactBook where IdContact =" + contactid).FirstOrDefault();
                return existingconact;
        }
        // Retrieve the all contact list from the database. 
        public List<ContactBook> ReadContacts()
        {
                List<ContactBook> ContactsList = instance.dbConn.Table<ContactBook>().ToList<ContactBook>();
                return ContactsList;
        }

        //Update existing conatct 
        public void UpdateContact(ContactBook contact)
        {

            System.Diagnostics.Debug.WriteLine(contact.IdContact);

            var existingconact = instance.dbConn.Query<ContactBook>("select * from ContactBook where IdContact =" + contact.IdContact).FirstOrDefault();
                if (existingconact != null)
                {
                    existingconact.Name = contact.Name;
                    existingconact.Phone = contact.Phone;
                    existingconact.Address = contact.Address;
                instance.dbConn.RunInTransaction(() =>
                    {
                        instance.dbConn.Update(existingconact);
                    });
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
                var existingconact = instance.dbConn.Query<ContactBook>("select * from ContactBook where IdContact =" + Id).FirstOrDefault();
                if (existingconact != null)
                {
                instance.dbConn.RunInTransaction(() =>
                    {
                        instance.dbConn.Delete(existingconact);
                    });
                }
            
        }
        //Delete all contactlist or delete Contacts table 
        public void DeleteAllContact()
        {

            //dbConn.RunInTransaction(() => 
            //   { 
            instance.dbConn.DropTable<ContactBook>();
            instance.dbConn.CreateTable<ContactBook>();
            instance.dbConn.Dispose();
            instance.dbConn.Close();
                //}); 
            }
        
    }
}
