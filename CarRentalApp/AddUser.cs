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
    public partial class AddUser : Form
    {
        private readonly CarRentalEntities1 _db;
        private ManageUsers _manageUsers;
        public AddUser(ManageUsers manageUsers)
        {
            _db = new CarRentalEntities1();
            InitializeComponent();
            _manageUsers = manageUsers;
        }

        private void AddUser_Load(object sender, EventArgs e)
        {
            var roles = _db.Roles.ToList();
            cmbRoles.DataSource= roles;
            cmbRoles.ValueMember = "id";
            cmbRoles.DisplayMember= "name";
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                //Collect data from the form first
                var username = tbUserName.Text;
                var roleId = (int)cmbRoles.SelectedValue;
                var password = Utils.DefaultHashPassword();
                // now we put the data back to database
                var user = new User
                {
                    username = username,
                    password = password,
                    isActive = true
                };
                _db.Users.Add(user);
                _db.SaveChanges();

                var userid = user.id;
                var userRole = new UserRole
                {
                    roleid = roleId,
                    userid = userid,
                };

                _db.UserRoles.Add(userRole);
                _db.SaveChanges();
                MessageBox.Show("New User added successfully!");
                _manageUsers.PopulateGrid();
                Close();
            }
            catch (Exception)
            {

                MessageBox.Show("An Error has occured");
            }
        }
    }
}
