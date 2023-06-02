using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Lifetime;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Veterinarska_stanica__.NET_
{
    public partial class FormAddPet : Form
    {
        private string firstName;
        private string lastName;
        private string address;
        private string petName;
        private string petType;
        private string petBreed;
        private int petAge;
        private DateTime visitDate;
        private string visitReason;
        private string visitOpinion;
        private int ownerId;
        private int petId;


        public FormAddPet()
        {
            InitializeComponent();
        }

        private void btnPetNext_Click(object sender, EventArgs e)
        {
            if (panel1.Controls.OfType<TextBox>().All(tb => !string.IsNullOrWhiteSpace(tb.Text)))
            {
                firstName = txtFirstName.Text;
                lastName = txtLastName.Text;
                address = txtAddress.Text;

                panel2.Visible = true;

                if (txtAddress.TextLength == 1)
                {
                    txtPetName.Focus();
                }
            }
            else
            {
                MessageBox.Show("Please fill in all fields.");
            }
        }


        private void btnPetNext2_Click(object sender, EventArgs e)
        {
            if (panel2.Controls.OfType<TextBox>().All(tb => !string.IsNullOrWhiteSpace(tb.Text)))
            {
                petName = txtPetName.Text;
                petType = txtPetType.Text;
                petBreed = txtPetBreed.Text;

                if (!int.TryParse(txtPetAge.Text, out petAge))
                {
                    MessageBox.Show("Please enter a valid integer value for pet age.");
                    return;
                }

                panel3.Visible = true;

                if (txtPetAge.TextLength == 1)
                {
                    txtReason.Focus();
                }
            }
            else
            {
                MessageBox.Show("Please fill in all fields.");
            }

        }

        private void FormAddPet_Load(object sender, EventArgs e)
        {
            panel1.Visible = true;
            panel2.Visible = false;
            panel3.Visible = false;

            dateTimePicker.Value = DateTime.Now;
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            panel2.Visible = false;
            panel1.Visible = true;
        }

        private void btnBack2_Click(object sender, EventArgs e)
        {
            panel3.Visible = false;
            panel2.Visible = true;
        }

        private void btnFinish_Click(object sender, EventArgs e)
        {
            if (panel3.Controls.OfType<TextBox>().All(tb => !string.IsNullOrWhiteSpace(tb.Text)))
            {
                visitDate = dateTimePicker.Value;
                visitReason = txtReason.Text;
                visitOpinion = txtOpinion.Text;

                string connStr = ConfigurationManager.ConnectionStrings["konekcija"].ConnectionString;
                using (SqlConnection sqlConnection = new SqlConnection(connStr))
                {
                    try
                    {
                        sqlConnection.Open();

                        using (SqlCommand command = new SqlCommand("INSERT INTO Owners(FirstName, LastName, Address) VALUES(@firstName, @lastName, @address); SELECT SCOPE_IDENTITY()", sqlConnection))
                        {
                            command.Parameters.AddWithValue("@firstName", firstName);
                            command.Parameters.AddWithValue("@lastName", lastName);
                            command.Parameters.AddWithValue("@address", address);
                            ownerId = Convert.ToInt32(command.ExecuteScalar());
                        }

                        using (SqlCommand petCommand = new SqlCommand("INSERT INTO Pets(Name, Type, Breed, Age, OwnerId) VALUES(@petName, @petType, @petBreed, @petAge, @ownerId); SELECT SCOPE_IDENTITY()", sqlConnection))
                        {
                            petCommand.Parameters.AddWithValue("@petName", petName);
                            petCommand.Parameters.AddWithValue("@petType", petType);
                            petCommand.Parameters.AddWithValue("@petBreed", petBreed);
                            petCommand.Parameters.AddWithValue("@petAge", petAge);
                            petCommand.Parameters.AddWithValue("@ownerId", ownerId);
                            petId = Convert.ToInt32(petCommand.ExecuteScalar());
                        }

                        using (SqlCommand visitCommand = new SqlCommand("INSERT INTO Visits(Date, Reason, Opinion, PetId) VALUES(@visitDate, @visitReason, @visitOpinion, @petId)", sqlConnection))
                        {
                            visitCommand.Parameters.AddWithValue("@visitDate", visitDate);
                            visitCommand.Parameters.AddWithValue("@visitReason", visitReason);
                            visitCommand.Parameters.AddWithValue("@visitOpinion", visitOpinion);
                            visitCommand.Parameters.AddWithValue("@petId", petId);
                            visitCommand.ExecuteNonQuery();
                        }

                        MessageBox.Show("Visit added successfully!");
                        this.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("An error occurred while attempting to add the pet and owner to the database. Error message: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please fill in all fields.");
            }
        }
    }
}
