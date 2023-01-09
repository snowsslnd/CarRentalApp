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
    public partial class AddEditRentalRecord : Form
    {
        private readonly CarRentalEntities1 _db;
        private ManageRentalRecords _manageRentalRecords;
        private bool IsEditMode;
        public AddEditRentalRecord(ManageRentalRecords manageRentalRecords = null)
        {
            InitializeComponent();

            lblTitle.Text = "Add New Rental Record";
            this.Text = "Add New Rental";
            IsEditMode = false;
            _manageRentalRecords = manageRentalRecords;
            _db = new CarRentalEntities1();
        }

        public AddEditRentalRecord(CarRentalRecord recordToEdit, ManageRentalRecords manageRentalRecords=null)
        {
            InitializeComponent();
            lblTitle.Text = "Edit Rental Record";
            this.Text = "Edit Rental Record";
            _manageRentalRecords = manageRentalRecords;

            if (recordToEdit == null)
            {
                MessageBox.Show("Please ensure that you selected a valid record to edit");
                Close();
            }
            else
            {
                PopulateFields(recordToEdit);
                _db = new CarRentalEntities1();
                IsEditMode = true;
            }
        }

        private void PopulateFields(CarRentalRecord recordToEdit)
        {
            tbCustomerName.Text = recordToEdit.CustomerName;
            dtpDateRented.Value = (DateTime)recordToEdit.DateRented;
            dtpDateReturned.Value = (DateTime)recordToEdit.DateReturned;         
            tbCost.Text = recordToEdit.Cost.ToString();
            lblRecordId.Text = recordToEdit.id.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string customerName = tbCustomerName.Text;
                var dateOut = dtpDateRented.Value;
                var dateIn = dtpDateReturned.Value;
                var carType = cmbTypeOfCar.SelectedItem.ToString();
                double cost = Convert.ToDouble(tbCost.Text);
                var isValid = true;
                var errorMessage = "";

                if (string.IsNullOrWhiteSpace(customerName) || string.IsNullOrWhiteSpace(carType))
                {
                    isValid = false;
                    errorMessage += "Error: Please enter missing data.\n\r";
                }

                if (dateOut > dateIn)
                {
                    isValid = false;
                    MessageBox.Show("illegal Data Selection");
                }

                if (isValid == true)
                {

                    if (IsEditMode)
                    {
                        var id = int.Parse(lblRecordId.Text);
                        var rentalRecord = _db.CarRentalRecords.FirstOrDefault(q => q.id == id);

                        rentalRecord.CustomerName = customerName;
                        rentalRecord.DateRented = dateOut;
                        rentalRecord.DateReturned = dateIn;
                        rentalRecord.Cost = (decimal)cost;
                        rentalRecord.TypeOfCarId = (int)cmbTypeOfCar.SelectedValue;
                        _db.SaveChanges();
                        _manageRentalRecords.PopulateGrid();
                    }

                    else {

                        var rentalRecord = new CarRentalRecord();
                        rentalRecord.CustomerName = customerName;
                        rentalRecord.DateRented = dateOut;
                        rentalRecord.DateReturned = dateIn;
                        rentalRecord.Cost = (decimal)cost;
                        rentalRecord.TypeOfCarId = (int)cmbTypeOfCar.SelectedValue;

                        _db.CarRentalRecords.Add(rentalRecord);
                        _db.SaveChanges();
                        _manageRentalRecords.PopulateGrid();
                    }


                    MessageBox.Show($"Customer Name: {customerName}\n\r" +
                    $"Date Rented: {dateOut}\n\r" + $"Date Returned: {dateIn}\n\r" +
                    $"Cost: {cost}\n\r" +
                    $"Car Type: {carType}\n\r" +
                    $"Thank you for your business");
                    Close();
                }
                else
                {
                    MessageBox.Show(errorMessage);
                }   


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }

        }
        private void Form1_Load(object sender, EventArgs e)
        {
            //var cars = carRentalEntities.TypesOfCars.ToList();

            var cars = _db.TypesOfCars
                .Select(q => new { Id = q.id, Name = q.Make + " " + q.Model }).ToList();
            cmbTypeOfCar.DisplayMember = "Name";
            cmbTypeOfCar.ValueMember = "id";
            cmbTypeOfCar.DataSource = cars;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }
    }
    
}