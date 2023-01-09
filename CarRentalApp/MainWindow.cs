using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarRentalApp
{
    public partial class MainWindow : Form
    {
        private Login _login;
        public string _roleName;
        public User _user;
        public MainWindow()
        {
            InitializeComponent();
        }

        public MainWindow(Login login, User user)
        {
            InitializeComponent();
            _login = login;
            _user = user;
            _roleName = user.UserRoles.FirstOrDefault().Role.shortname;
        }

        private void addRentalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var addRentalRecord = new AddEditRentalRecord();
            addRentalRecord.ShowDialog();
            addRentalRecord.MdiParent= this;
            
        }

        private void manageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //to get a list of windows forms in the datatype form
            var OpenForms = Application.OpenForms.Cast<Form>();
            var isOpen = OpenForms.Any(q => q.Name == "ManageVehicleListing");
            if (!isOpen) 
            {
                var vehicalListing = new ManageVehicleListing();
                vehicalListing.MdiParent = this;
                vehicalListing.Show();
            }
            
        }

        private void viewArchieveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var OpenForms = Application.OpenForms.Cast<Form>();
            var isOpen = OpenForms.Any(q => q.Name == "ManageRentalRecords");
            if (!isOpen) 
            {
                var manageRentalRecords = new ManageRentalRecords();
                manageRentalRecords.MdiParent = this;
                manageRentalRecords.Show();
            }

            
        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            _login.Close();
        }

        private void manageUsersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var OpenForms = Application.OpenForms.Cast<Form>();
            var isOpen = OpenForms.Any(q => q.Name == "ManageUsers");
            if (!isOpen)
            {
                var manageUsers = new ManageUsers();
                manageUsers.MdiParent = this;
                manageUsers.Show();
            }
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            if (_user.password == Utils.DefaultHashPassword())
            {
                var resetPassword = new ResetPassword(_user);
                resetPassword.ShowDialog();
            }

            var username = _user.username;
            tsiLoginText.Text = $"Logged In As: {username}";
            if (_roleName != "admin") 
            {
                manageUsersToolStripMenuItem.Visible = false;
            }
        }
    }
}
