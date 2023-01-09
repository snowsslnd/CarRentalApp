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
    public partial class AddEditVehicle : Form
    {
        private readonly CarRentalEntities1 _db;
        private ManageVehicleListing _manageVehicleListing;
        private bool IsEditMode;
        public AddEditVehicle(ManageVehicleListing manageVehicleListing = null)
        {
            InitializeComponent();
            lblTitle.Text = "Add New Vehicle";
            IsEditMode = false;
            _manageVehicleListing= manageVehicleListing;
            _db = new CarRentalEntities1();
        }

        public AddEditVehicle(TypesOfCar carToEdit, ManageVehicleListing manageVehicleListing= null)
        {
            InitializeComponent();
            lblTitle.Text = "Edit Vehicle";
            _manageVehicleListing = manageVehicleListing;
            PopulateFields(carToEdit);
            _db = new CarRentalEntities1();
            IsEditMode= true;
        }

        //gonna display the objects coming from db to text boxes
        private void PopulateFields(TypesOfCar car)
        {
            lblId.Text = car.id.ToString();
            tbMake.Text = car.Make;
            tbModel.Text = car.Model;
            tbVIN.Text = car.VIN;
            tbYear.Text = car.Year.ToString();
            tbLPN.Text = car.LicensePlateNumber;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (IsEditMode)
            {
                var Id= int.Parse(lblId.Text);
                var car = _db.TypesOfCars.FirstOrDefault(q => q.id == Id);
                car.Model = tbModel.Text;
                car.Make= tbMake.Text;
                car.VIN= tbVIN.Text;
                car.Year = int.Parse(tbYear.Text);
                car.LicensePlateNumber= tbLPN.Text;

                _db.SaveChanges();
                _manageVehicleListing.GridLoading();
            }
            else 
            {
                var newCar = new TypesOfCar
                {
                    LicensePlateNumber = tbLPN.Text,
                    Make = tbMake.Text,
                    Model = tbModel.Text,
                    Year = int.Parse(tbYear.Text),
                    VIN = tbVIN.Text

                };
                _db.TypesOfCars.Add(newCar);
                _db.SaveChanges();
                _manageVehicleListing.GridLoading();
                
                
            }
            this.Close();
            
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
