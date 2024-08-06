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


using System.IdentityModel.Tokens.Jwt;

namespace adminTinTuc
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private async void loginBtn_Click(object sender, EventArgs e)
        {
            // Lấy email và password từ TextBox
            string email = emailBox.Text;
            string password = passwordBox.Text;

            // Tạo đối tượng HttpClient
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Thiết lập URL API và dữ liệu JSON cần gửi
                    string apiUrl = "https://localhost:7161/api/Login/login";
                    var loginData = new { email = email, password = password };
                    var jsonContent = new StringContent(JsonConvert.SerializeObject(loginData), Encoding.UTF8, "application/json");

                    // Gửi yêu cầu POST tới API
                    HttpResponseMessage response = await client.PostAsync(apiUrl, jsonContent);

                    // Kiểm tra kết quả
                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();

                        // Giả sử API trả về một đối tượng JSON chứa JWT trong trường "token"
                        var jsonResponse = JsonConvert.DeserializeObject<dynamic>(responseBody);
                        string token = jsonResponse?.token;

                        if (string.IsNullOrEmpty(token))
                        {
                            MessageBox.Show("Token is empty or null.");
                            return;
                        }

                        // Lưu token vào biến toàn cục
                        GlobalVariables.JwtToken = token;

                        var handler = new JwtSecurityTokenHandler();

                        if (!handler.CanReadToken(token))
                        {
                            MessageBox.Show("The token is not in a correct format.");
                            return;
                        }

                        var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
                        var role = jsonToken.Claims.First(claim => claim.Type == "role").Value;
                        var userId = jsonToken.Claims.First(claim => claim.Type == "unique_name").Value;

                        GlobalVariables.CurrentUserId = userId;

                        if (role == "Admin")
                        {
                            MessageBox.Show("Login Successful! You are an Admin.");
                            Form2 form2 = new Form2();
                            form2.Show();
                            this.Hide(); // Ẩn Form1
                        }
                        else
                        {
                            MessageBox.Show("Login Successful! But you are not an Admin.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Login Failed! Status Code: " + response.StatusCode);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

    }
    public static class GlobalVariables
    {
        public static string JwtToken { get; set; }
        public static string CurrentUserId { get; set; }
    }
}
