using ContactApp.ContactClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ContactApp
{
    public partial class Contact : Form
    {
        public Contact()
        {
            InitializeComponent();
            
        }
        ContactClass c = new ContactClass();
        private void Contact_Load(object sender, EventArgs e)
        {
            //Load data on dataGirdView :
            DataTable dt = c.Select();
            dgvContactList.DataSource = dt;   
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {
             
        }

        private void UpdateDataGridView()
        {
        
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            // Get the value from the input fields
                c.firstName = txtBoxFirstName.Text;
                c.lastName = txtBoxLastName.Text;
                c.contactNo = txtBoxContactNo.Text;
                c.address = txtBoxAdress.Text;
                c.gender = cmbBoxGender.Text;

                // Inserting data to the database using the methods in the class
                bool success = c.Insert(c);

                if (success==true)
                {
                    MessageBox.Show("New Contact Successfully added");
                //Using Clear method to clear data from textBoxes after insertion:
                Clear();

                }
                else
                {
                    MessageBox.Show("Failed to add a new Contact. Try Again!!");
                }
            //Load data on dataGirdView after inserting the data :
            DataTable dt = c.Select();
            dgvContactList.DataSource = dt;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //Method to clear all fields after inserting data : 
        public void Clear()
        {
            txtBoxContactID.Text = "";
            txtBoxFirstName.Text = "";
            txtBoxLastName.Text = "";
            txtBoxContactNo.Text = "";
            txtBoxAdress.Text = "";
            cmbBoxGender.Text = "";
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            //Get the data from TextB:
            c.contactID = int.Parse(txtBoxContactID.Text);
            c.firstName = txtBoxFirstName.Text;
            c.lastName = txtBoxLastName.Text;   
            c.contactNo = txtBoxContactNo.Text;
            c.address = txtBoxAdress.Text;
            c.gender = cmbBoxGender.Text;
            //Update data in database:
            bool success = c.Update(c);
            if(success==true)
            {
                MessageBox.Show("Contact has been Successfully Updated.");
                //Load data on dataGirdView After Update , it see the data:
                DataTable dt = c.Select();
                dgvContactList.DataSource = dt;
                //Clear the texte Boxes after Update :
                Clear();
            }
            else
            {
                MessageBox.Show("Failed to Update Contact. TRY AGAIN PLAEASE !!");
            }
            
        }

        private void dgvContactList_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //We have to get the data from DataGird Using DataGridViewCellMouseEventArgs selector
            //And render it into textBoxes.
            int rowIndex = e.RowIndex;
            txtBoxContactID.Text = dgvContactList.Rows[rowIndex].Cells[0].Value.ToString();
            txtBoxFirstName.Text = dgvContactList.Rows[rowIndex].Cells[1].Value.ToString();
            txtBoxLastName.Text = dgvContactList.Rows[rowIndex].Cells[2].Value.ToString();
            txtBoxContactNo.Text = dgvContactList.Rows[rowIndex].Cells[3].Value.ToString();
            txtBoxAdress.Text = dgvContactList.Rows[rowIndex].Cells[4].Value.ToString();
            cmbBoxGender.Text = dgvContactList.Rows[rowIndex].Cells[5].Value.ToString();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            //Calling Clear method to let the button Clear Functionnal:
            Clear();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            c.contactID = Convert.ToInt32(txtBoxContactID.Text);
            bool success = c.Delete(c);
            if (success)
            {
                MessageBox.Show("Your Contact is successfuly deleted.");
                //Resreshing the DataGird : 
                //Load data on dataGirdView :
                DataTable dt = c.Select();
                dgvContactList.DataSource = dt;
                //Clearing texteBoxes
                Clear();
            }
            else {
                MessageBox.Show("Cannot delete this contact .Try agin !!");
                  }
        }

        static string myconnstrng = ConfigurationManager.ConnectionStrings["connstrng"].ConnectionString;
        private void txtBoxSearch_TextChanged(object sender, EventArgs e)
        {
            //Get the value from the textBox search:
            string keyword = txtBoxSearch.Text;
            SqlConnection conn = new SqlConnection(myconnstrng);
            SqlDataAdapter sda = new SqlDataAdapter("SELECT * FROM tbl_contact WHERE firstName LIKE '%" + keyword + "%' OR lastName LIKE '%" + keyword + "%' OR address LIKE '%" + keyword + "%' OR contactNo LIKE '%" + keyword+"%'",conn);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            dgvContactList.DataSource = dt;
        }
    }
}
