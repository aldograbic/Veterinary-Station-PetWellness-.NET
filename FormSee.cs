using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace Veterinarska_stanica__.NET_
{
    public partial class FormSee : Form
    {
        public FormSee()
        {
            InitializeComponent();
        }

        private void FormSee_Load(object sender, System.EventArgs e)
        {
            LoadData();
            this.visitsTableAdapter.Fill(this.veterinaryStationDBDataSet1.Visits);
            this.visitsTableAdapter.Fill(this.veterinaryStationDBDataSet.Visits);
        }

        private void LoadData()
        {
            dataGridView1.Rows.Clear();

            string connStr = ConfigurationManager.ConnectionStrings["konekcija"].ConnectionString;
            using (SqlConnection sqlConnection = new SqlConnection(connStr))
            {
                sqlConnection.Open();
                SqlCommand command = new SqlCommand("SELECT visits.Date, owners.FirstName, owners.LastName, pets.Name, pets.Type, visits.Reason, visits.Opinion " +
                                                     "FROM visits " +
                                                     "INNER JOIN pets ON visits.petId = pets.Id " +
                                                     "INNER JOIN owners ON pets.ownerId = owners.Id", sqlConnection);

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string visitOpinion = reader["Opinion"].ToString();
                    string visitReason = reader["Reason"].ToString();
                    string petName = reader["Name"].ToString();
                    string petType = reader["Type"].ToString();
                    string ownerFirstName = reader["FirstName"].ToString();
                    string ownerLastName = reader["LastName"].ToString();
                    DateTime visitDateTime = (DateTime)reader["Date"];
                    string visitDate = visitDateTime.Date.ToString("dd.MM.yyyy");

                    string ownerName = ownerFirstName + " " + ownerLastName;
                    string petFull = petName + " (" + petType + ")";

                    dataGridView1.Rows.Add(visitDate, ownerName, petFull, visitReason, visitOpinion);
                }

                reader.Close();
                sqlConnection.Close();
            }
        }
    }
}
