using System;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;
using System.Configuration;
using System.Net;
using System.Drawing;

namespace Veterinarska_stanica__.NET_
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            FormAddPet formAddPet = new FormAddPet();
            formAddPet.Show();
        }

        private void btnSee_Click(object sender, EventArgs e)
        {
            FormSee formSee = new FormSee();
            formSee.Show();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string searchInput = txtsearch.Text;
            string connStr = ConfigurationManager.ConnectionStrings["konekcija"].ConnectionString;
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connStr))
                {
                    sqlConnection.Open();

                    if (checkedListBox1.SelectedItem == null)
                    {
                        MessageBox.Show("Please select a search filter.");
                    }
                    else if (checkedListBox1.SelectedItem.ToString() == "Owner's first name")
                    { 
                        using (SqlCommand command = new SqlCommand("SELECT FirstName AS 'First name', LastName as 'Last name', Address FROM Owners WHERE FirstName LIKE '%' + @SearchInput + '%'", sqlConnection))
                        {
                            command.Parameters.AddWithValue("@SearchInput", searchInput);

                            SqlDataAdapter da = new SqlDataAdapter(command);
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            if (dt.Rows.Count > 0)
                            {
                                FormSearchResult formSearchResult = new FormSearchResult(dt);
                                formSearchResult.Show();
                            }
                            else
                            {
                                MessageBox.Show("There is no result found!");
                            }
                        }
                    }
                    else if(checkedListBox1.SelectedItem.ToString() == "Pet's name")
                    {
                        using (SqlCommand command = new SqlCommand("SELECT Name, Type, Breed, Age, FirstName AS Owner FROM Pets INNER JOIN Owners ON Pets.OwnerId = Owners.Id WHERE Name LIKE '%' + @SearchInput + '%'", sqlConnection))
                        {
                            command.Parameters.AddWithValue("@SearchInput", searchInput);

                            SqlDataAdapter da = new SqlDataAdapter(command);
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            if (dt.Rows.Count > 0)
                            {
                                FormSearchResult formSearchResult = new FormSearchResult(dt);
                                formSearchResult.Show();
                            }
                            else
                            {
                                MessageBox.Show("There is no result found!");
                            }
                        }
                    }
                    else if (checkedListBox1.SelectedItem.ToString() == "Pet's type")
                    {
                        using (SqlCommand command = new SqlCommand("SELECT Name, Type, Breed, Age, FirstName AS Owner FROM Pets INNER JOIN Owners ON Pets.OwnerId = Owners.Id WHERE Type LIKE '%' + @SearchInput + '%'", sqlConnection))
                        {
                            command.Parameters.AddWithValue("@SearchInput", searchInput);

                            SqlDataAdapter da = new SqlDataAdapter(command);
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            if (dt.Rows.Count > 0)
                            {
                                FormSearchResult formSearchResult = new FormSearchResult(dt);
                                formSearchResult.Show();
                            }
                            else
                            {
                                MessageBox.Show("There is no result found!");
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("An error occurred while searching the database: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void checkedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            for (int i = 0; i < checkedListBox1.Items.Count; ++i)
            {
                if (i != e.Index)
                {
                    checkedListBox1.SetItemChecked(i, false);
                }
            }
        }
    }
}
