using MySql.Data.MySqlClient;

namespace WinFormsAppDEMO4
{
    public partial class Form1 : Form
    {
        private DatabaseManager databaseManager;

        public Form1()
        {
            InitializeComponent();
            databaseManager = new DatabaseManager();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text;
            string password = textBox2.Text;

            string query = "SELECT UserType FROM Users WHERE Username = @username AND Password = @password";
            MySqlCommand cmd = new MySqlCommand(query, databaseManager.GetConnection());

            cmd.Parameters.AddWithValue("username", username);
            cmd.Parameters.AddWithValue("password", password);

            object userType = cmd.ExecuteScalar();

            if (userType !=  null)
            {
                MainForm mainForm = new MainForm(userType.ToString());
                this.Hide();
                mainForm.Show();
            }
            else
            {
                MessageBox.Show("пароль или логин неправилен");
            }
        }

    }
}
