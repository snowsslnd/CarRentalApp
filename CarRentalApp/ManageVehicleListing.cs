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
    public partial class ManageVehicleListing : Form
    {
        private readonly CarRentalEntities1 _db;
        public ManageVehicleListing()
        {
            InitializeComponent();
            _db= new CarRentalEntities1();
        }

        private void ManageVehicleListing_Load(object sender, EventArgs e)
        {
            GridLoading();
           
        }

        public void GridLoading() 
        {
            //Select * From TypeOfCars
            // var cars = _db.TypesOfCars.ToList();
            //Select Id as CarId, name as CarName from Types of Cars
            //var cars = _db.TypesOfCars
            //    .Select(q => new { CarId = q.id, CarName = q.Make })
            //    .ToList();


            //returning data from database==>
            var cars = _db.TypesOfCars
                .Select(q => new
                {
                    Make = q.Make,
                    Model = q.Model,
                    VIN = q.VIN,
                    Year = q.Year,
                    LiscensePlateNumber = q.LicensePlateNumber,
                    q.id //want to use this but doesn't want to display it
                }).ToList();
            gvVehicalList.DataSource = cars;
            gvVehicalList.Columns[4].HeaderText = "License Plate Number";
            gvVehicalList.Columns[5].Visible = false;

        }

        private void btnAddCar_Click(object sender, EventArgs e)
        {
            var addEditVehicle = new AddEditVehicle(this);
            addEditVehicle.MdiParent = this.MdiParent;
            addEditVehicle.Show();
        }

        private void btnEditCar_Click(object sender, EventArgs e)
        {
            
            try
            { 
                //get id of the selected row
                var Id = (int)gvVehicalList.SelectedRows[0].Cells["id"].Value;
                //query database for record
                var car = _db.TypesOfCars.FirstOrDefault(q => q.id == Id);

                //lauch AddEditVehicle window with data 
                var addEditVehicle = new AddEditVehicle(car, this);
                addEditVehicle.MdiParent = this.MdiParent;
                addEditVehicle.Show();

                //the above one will go to the edit constructor
            }
            catch (Exception)
            { MessageBox.Show("Select the row to edit!"); }
   
        }

    
        private void btnDeleteCar_Click(object sender, EventArgs e)
        {
            //get id of the selected row
            var Id = (int)gvVehicalList.SelectedRows[0].Cells["id"].Value;

            //query database for record
            var car = _db.TypesOfCars.FirstOrDefault(q => q.id == Id);

            DialogResult dr = MessageBox.Show("Are you sure you want to delete this record?",
                "Delete", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);

            if (dr == DialogResult.Yes)
            {
                //delete vehicle from the table
                _db.TypesOfCars.Remove(car);
  
            }
            try
            {
                _db.SaveChanges();
            }
            catch (Exception)
            {

                MessageBox.Show("Cannot delete data used in other form!");
            }
            GridLoading();
        }

        //private void btnRefresh_Click(object sender, EventArgs e)
        //{
        //    GridLoading();
        //}

        
    }
}
