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
        private string selectedAccountId;

        public Form3()
        {
            InitializeComponent();
            dataGridView1.SelectionChanged += DataGridView1_SelectionChanged;
        }

        private async void Form3_Load(object sender, EventArgs e)
        {
            var accounts = await GetAccountsAsync();
            dataGridView1.DataSource = accounts;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(selectedAccountId))
            {
                GetAccountDetails(selectedAccountId);
            }
            else
            {
                MessageBox.Show("Please select an account to edit.");
            }
        }

        private void DataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var selectedRow = dataGridView1.SelectedRows[0];
                selectedAccountId = selectedRow.Cells["Id"].Value.ToString(); // Thay đổi tên cột nếu cần
            }
            else
            {
                selectedAccountId = null;
            }
        }

        private async void EditForm_AccountUpdated()
        {
            // Làm mới dữ liệu trong dataGridView1 sau khi tài khoản được cập nhật
            await LoadAccountData(); // Gọi phương thức để tải lại dữ liệu
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

        private async void GetAccountDetails(string accountId)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string apiUrl = $"https://localhost:7161/api/Account/{accountId}";
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GlobalVariables.JwtToken);
                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        var account = JsonConvert.DeserializeObject<Account>(responseBody);

                        // Mở EditAccountForm với thông tin của tài khoản
                        EditAccountForm editForm = new EditAccountForm(account.Id, account.Email, account.Password, account.Name, account.Roles, account.CreatedAt, account.UpdatedAt);
                        editForm.AccountUpdated += EditForm_AccountUpdated;
                        editForm.ShowDialog(); // Hiển thị form dưới dạng modal
                    }
                    else
                    {
                        MessageBox.Show("Failed to retrieve account details. Status Code: " + response.StatusCode);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private async Task LoadAccountData()
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string apiUrl = "https://localhost:7161/api/Account";
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GlobalVariables.JwtToken);
                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        var accounts = JsonConvert.DeserializeObject<List<Account>>(responseBody);

                        dataGridView1.DataSource = accounts; // Cập nhật dataGridView1 với dữ liệu mới
                    }
                    else
                    {
                        MessageBox.Show("Failed to load account data. Status Code: " + response.StatusCode);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
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
