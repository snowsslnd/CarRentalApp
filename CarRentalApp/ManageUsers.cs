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
    
    public partial class ManageUsers : Form
    {
        private readonly CarRentalEntities1 _db;
        public ManageUsers()
        {
            InitializeComponent();
            _db = new CarRentalEntities1();
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            //var OpenForms = Application.OpenForms.Cast<Form>();
            //var isOpen = OpenForms.Any(q => q.Name == "AddUsers");
            //if (!isOpen)
            //{
                var addUser = new AddUser(this);
                addUser.MdiParent = this.MdiParent;
                addUser.Show();
            //}
            
        }

        private void btnResetPassword_Click(object sender, EventArgs e)
        {
            try
            {
                //get id of the selected row
                var Id = (int)gvUserList.SelectedRows[0].Cells["id"].Value;
                //query database for record
                var user = _db.Users.FirstOrDefault(q => q.id == Id);
                //var genericPassword = "Password@123";
                var hashed_password = Utils.DefaultHashPassword();
                //gonna change the password from the database
                user.password = hashed_password;
                _db.SaveChanges();

                MessageBox.Show($"{user.username}'s Password has been reset!");
            }
            catch (Exception ex)
            { MessageBox.Show($"Error: {ex.Message}"); }

        }

        private void btnDeactivateUser_Click(object sender, EventArgs e)
        {
            try
            {
                //get id of the selected row
                var Id = (int)gvUserList.SelectedRows[0].Cells["id"].Value;
                //query database for record
                var user = _db.Users.FirstOrDefault(q => q.id == Id);

                //if the user is active in database, set it to false, otherwise true
                user.isActive = user.isActive == true ? false : true;
                _db.SaveChanges();

                MessageBox.Show($"{user.username}'s active status has been changed!");
                PopulateGrid();
            }
            catch (Exception ex)
            { MessageBox.Show($"Error: {ex.Message}"); }
        }

        private void ManageUsers_Load(object sender, EventArgs e)
        {
            PopulateGrid();
        }

        public void PopulateGrid() 
        {
            var users  = _db.Users
                .Select(q => new
                {
                    q.id,
                    q.username,
                    q.UserRoles.FirstOrDefault().Role.name,
                    q.isActive
                })
                .ToList();
            gvUserList.DataSource = users;
            gvUserList.Columns["username"].HeaderText = "Username";
            gvUserList.Columns["name"].HeaderText = "Role Name";
            gvUserList.Columns["isActive"].HeaderText = "Active";
            gvUserList.Columns["id"].Visible = false;
        }
    }

        
    
}
