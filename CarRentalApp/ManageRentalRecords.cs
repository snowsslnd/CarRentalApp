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
    public partial class ManageRentalRecords : Form
    {
        private readonly CarRentalEntities1 _db;
        
        public ManageRentalRecords()
        {
            InitializeComponent();
            _db = new CarRentalEntities1();
        }

        private void btnAddRecord_Click(object sender, EventArgs e)
        {
            var addRentalRecord = new AddEditRentalRecord(this);
            addRentalRecord.MdiParent= this.MdiParent;
            addRentalRecord.Show();
        }

        private void btnEditRecord_Click(object sender, EventArgs e)
        {

            try
            {
                //get id of the selected row
                var Id = (int)gvRecordList.SelectedRows[0].Cells["id"].Value;


                //query database for record
                var record = _db.CarRentalRecords.FirstOrDefault(q => q.id == Id);

                //lauch AddEditVehicle window with data 
                var addEditRentalRecord = new AddEditRentalRecord(record, this);
                addEditRentalRecord.MdiParent = this.MdiParent;
                addEditRentalRecord.Show();
                //the above one will go to the edit constructor
            }
            catch (Exception)
            { MessageBox.Show("Select the row to edit!"); }

        }

        private void btnDeleteRecord_Click(object sender, EventArgs e)
        {
            //get id of the selected row
            var Id = (int)gvRecordList.SelectedRows[0].Cells["id"].Value;

            //query database for record
            var record = _db.CarRentalRecords.FirstOrDefault(q => q.id == Id);

            DialogResult dr = MessageBox.Show("Are you sure you want to delete this record?",
                "Delete", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);

            if (dr == DialogResult.Yes)
            {
                //delete vehicle from the table
                _db.CarRentalRecords.Remove(record);
            }

            _db.SaveChanges();
            PopulateGrid();
        }

        private void ManageRentalRecords_Load(object sender, EventArgs e)
        {
            try
            {
                PopulateGrid();
            }
            catch (Exception ex) 
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        public void PopulateGrid()
        {
            var records = _db.CarRentalRecords.Select(q => new
            {
                Customer = q.CustomerName,
                DateOut = q.DateRented,
                DateIn = q.DateReturned,
                Id = q.id,
                q.Cost,
                Car = q.TypesOfCar.Make + " " + q.TypesOfCar.Model

            }).ToList(); 
            gvRecordList.DataSource = records;
            gvRecordList.Columns["DateIn"].HeaderText = "Date In ";
            gvRecordList.Columns["DateOut"].HeaderText = "Date Out ";
            gvRecordList.Columns["Id"].Visible= false;
        }

        
    }
}
