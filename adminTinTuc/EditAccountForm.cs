using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static adminTinTuc.Form3;

namespace adminTinTuc
{
    public partial class EditAccountForm : Form
    {
        public delegate void AccountUpdatedHandler();
        public event AccountUpdatedHandler AccountUpdated;

        private string _accountId;
        private string _originalEmail;
        private string _originalRole;
        private string _originalName;
        private string _originalPassword;
        private DateTime _originalCreatedAt;
        private DateTime _originalUpdatedAt;

        public EditAccountForm(string accountId, string email, string password, string name, string role, DateTime createdAt, DateTime updatedAt)
        {
            InitializeComponent();
            _accountId = accountId;
            _originalEmail = email;
            _originalRole = role;
            _originalName = name;
            _originalPassword = password;
            _originalCreatedAt = createdAt;
            _originalUpdatedAt = updatedAt;

            emailBox.Text = email;
            nameBox.Text = name;
            passwordBox.Text = password;

            if (role == "Admin")
            {
                adminBtn.Checked = true;
            }
            else
            {
                userBtn.Checked = true;
            }
        }

        private async void saveBtn_Click(object sender, EventArgs e)
        {
            string email = emailBox.Text;
            string name = nameBox.Text;
            string password = passwordBox.Text;
            string role = adminBtn.Checked ? "Admin" : "User";

            var updatedAccount = new Account
            {
                Id = _accountId,
                Email = email,
                Password = password,
                Name = name,
                CreatedAt = _originalCreatedAt, // Giữ nguyên ngày tạo
                UpdatedAt = DateTime.UtcNow, // Cập nhật ngày sửa
                Roles = role
            };

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string apiUrl = $"https://localhost:7161/api/Account/{_accountId}";
                    var jsonContent = new StringContent(JsonConvert.SerializeObject(updatedAccount), Encoding.UTF8, "application/json");

                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GlobalVariables.JwtToken);
                    HttpResponseMessage response = await client.PutAsync(apiUrl, jsonContent);

                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Account updated successfully!");
                        AccountUpdated?.Invoke(); // Gọi sự kiện thông báo cập nhật
                        this.Close(); // Đóng form sau khi cập nhật thành công
                    }
                    else
                    {
                        MessageBox.Show("Failed to update account. Status Code: " + response.StatusCode);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }
    }
}
