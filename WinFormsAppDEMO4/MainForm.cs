using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsAppDEMO4
{
    public partial class MainForm : Form
    {
        private DatabaseManager databaseManager;
        System.Data.DataTable dataTable;

        public MainForm(string userType)
        {
            InitializeComponent();
            databaseManager = new DatabaseManager();

            if (userType != "Administrator")
            {
                button1.Visible = false;
            }

            LoadComboBoxData();
        }

        private void LoadComboBoxData()
        {
            string query1 = "SELECT DISTINCT UserType FROM Users";
            MySqlCommand command1 = new MySqlCommand(query1, databaseManager.GetConnection());
            using (MySqlDataReader reader = command1.ExecuteReader())
            {
                while (reader.Read())
                {
                    comboBox1.Items.Add(reader.GetString("UserType"));
                }
            }

            string query2 = "SELECT DISTINCT UserStatus FROM Users";
            MySqlCommand command2 = new MySqlCommand(query2, databaseManager.GetConnection());
            using (MySqlDataReader reader = command2.ExecuteReader())
            {
                while (reader.Read())
                {
                    comboBox2.Items.Add(reader.GetString("UserStatus"));
                }
            }
        }

        private void LoadUserData()
        {
            dataGridView1.DataSource = null;
            string query = "SELECT * FROM users";
            MySqlCommand cmd = new MySqlCommand(query, databaseManager.GetConnection());

            MySqlDataAdapter dataAdapter = new MySqlDataAdapter(cmd);
            dataTable = new System.Data.DataTable();
            dataAdapter.Fill(dataTable);


            DataGridViewComboBoxColumn status = new DataGridViewComboBoxColumn();
            var list = new List<string>() { "Уволен", "Активен" };
            status.DataSource = list;
            status.HeaderText = "Статус";
            status.DataPropertyName = "UserStatus";

            dataGridView1.DataSource = dataTable;

            dataGridView1.Columns[1].ReadOnly = true;
            dataGridView1.Columns[2].ReadOnly = true;

            dataGridView1.Columns.AddRange(status);
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text;
            string password = textBox2.Text;
            string userType = comboBox1.SelectedItem.ToString();
            string userStatus = comboBox2.SelectedItem.ToString();

            string query = "INSERT INTO Users (Username, Password, UserType, UserStatus) VALUES (@username, @password, @userType, @userStatus)";
            MySqlCommand cmd = new MySqlCommand(query, databaseManager.GetConnection());

            cmd.Parameters.AddWithValue("@username", username);
            cmd.Parameters.AddWithValue("@password", password);
            cmd.Parameters.AddWithValue("@userType", userType);
            cmd.Parameters.AddWithValue("@userStatus", userStatus);

            cmd.ExecuteNonQuery();

            LoadUserData();
        }

        private void LogoutButton_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
            this.Close();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            LoadUserData();
        }

        void SetStatus(int id, string fired)
        {
            MySqlCommand cmd = new MySqlCommand($"UPDATE users SET UserStatus='{fired}' WHERE UserID={id}", databaseManager.GetConnection());
            cmd.ExecuteNonQuery();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            int i = e.RowIndex;
            SetStatus(Convert.ToInt32(dataTable.Rows[i]["UserID"]), Convert.ToString(dataTable.Rows[i]["UserStatus"]));
            LoadUserData();
        }
    }
}
