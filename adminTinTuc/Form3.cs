using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace adminTinTuc
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private async void Form3_Load(object sender, EventArgs e)
        {
            var accounts = await GetAccountsAsync();
            dataGridView1.DataSource = accounts;
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void addButton_Click(object sender, EventArgs e)
        {
            var addAccountForm = new AddAccountForm();
            addAccountForm.ShowDialog();
        }

        private async Task<List<Account>> GetAccountsAsync()
        {
            string apiUrl = "https://localhost:7161/api/Account"; // URL của API lấy tất cả tài khoản
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Thiết lập token trong header nếu cần
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GlobalVariables.JwtToken);

                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        var accounts = JsonConvert.DeserializeObject<List<Account>>(responseBody);
                        return accounts;
                    }
                    else
                    {
                        MessageBox.Show("Failed to retrieve accounts. Status Code: " + response.StatusCode);
                        return new List<Account>();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                    return new List<Account>();
                }
            }
        }

        public class Account
        {
            public string Id { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
            public string Name { get; set; }
            public DateTime CreatedAt { get; set; }
            public DateTime UpdatedAt { get; set; }
            public string Roles { get; set; }
        }
    }
}
