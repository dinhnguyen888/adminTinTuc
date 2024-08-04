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
using Newtonsoft.Json;
using static adminTinTuc.Form3;

namespace adminTinTuc
{
    public partial class Form4 : Form
    {
        private static readonly HttpClient client = new HttpClient();

        public Form4()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            LoadNewsData();
        }

        private async void LoadNewsData()
        {
            try
            {
                var response = await client.GetAsync("https://localhost:7161/api/News");
                response.EnsureSuccessStatusCode();

                var responseBody = await response.Content.ReadAsStringAsync();
                var newsList = JsonConvert.DeserializeObject<List<NewsDTO>>(responseBody);

                dataGridView1.DataSource = newsList;
            }
            catch (HttpRequestException e)
            {
                MessageBox.Show($"Request error: {e.Message}");
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var selectedRow = dataGridView1.SelectedRows[0];
                txtTitle.Text = selectedRow.Cells["Title"].Value.ToString();
                txtDescription.Text = selectedRow.Cells["Description"].Value.ToString();
                txtContent.Text = selectedRow.Cells["Content"].Value.ToString();
            }
        }

        public class NewsDTO
        {
            public string Id { get; set; }
            public string Title { get; set; }
            public string LinkDetail { get; set; }
            public string ImageUrl { get; set; }
            public string Description { get; set; }
            public string Content { get; set; }
            public string Type { get; set; }
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var selectedRow = dataGridView1.SelectedRows[0];
                var id = selectedRow.Cells["Id"].Value.ToString();

                var confirmation = MessageBox.Show("Are you sure you want to delete this news item?", "Confirm Delete", MessageBoxButtons.YesNo);

                if (confirmation == DialogResult.Yes)
                {
                    try
                    {
                        string apiUrl = $"https://localhost:7161/api/News/{id}";
                        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GlobalVariables.JwtToken);

                        HttpResponseMessage response = await client.DeleteAsync(apiUrl);

                        if (response.IsSuccessStatusCode)
                        {
                            MessageBox.Show("News deleted successfully!");
                            LoadNewsData(); // Làm mới dữ liệu trong dataGridView1
                        }
                        else
                        {
                            MessageBox.Show("Failed to delete news. Status Code: " + response.StatusCode);
                        }
                    }
                    catch (HttpRequestException ex)
                    {
                        MessageBox.Show($"Request error: {ex.Message}");
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a news item to delete.");
            }
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    var selectedRow = dataGridView1.SelectedRows[0];
                    var id = selectedRow.Cells["Id"].Value.ToString();

                    var confirmation = MessageBox.Show("Are you sure you want to update this news item?", "Confirm Update", MessageBoxButtons.YesNo);

                    if (confirmation == DialogResult.Yes)
                    {
                        // Lấy thông tin hiện tại của tin tức để giữ nguyên các trường không thay đổi
                        var currentNews = (NewsDTO)dataGridView1.SelectedRows[0].DataBoundItem;

                        var updatedNews = new NewsDTO
                        {
                            Id = currentNews.Id, // Giữ nguyên Id
                            Title = txtTitle.Text, // Cập nhật Title
                            LinkDetail = currentNews.LinkDetail, // Giữ nguyên LinkDetail
                            ImageUrl = currentNews.ImageUrl, // Giữ nguyên ImageUrl
                            Description = txtDescription.Text, // Cập nhật Description
                            Content = txtContent.Text, // Cập nhật Content
                            Type = currentNews.Type // Giữ nguyên Type
                        };

                        var json = System.Text.Json.JsonSerializer.Serialize(updatedNews);
                        var content = new StringContent(json, Encoding.UTF8, "application/json");

                        try
                        {
                            string apiUrl = $"https://localhost:7161/api/News/{id}";
                            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GlobalVariables.JwtToken);

                            HttpResponseMessage response = await client.PutAsync(apiUrl, content);

                            if (response.IsSuccessStatusCode)
                            {
                                MessageBox.Show("News updated successfully!");
                                LoadNewsData(); // Làm mới dữ liệu trong dataGridView1
                            }
                            else
                            {
                                // Hiển thị nội dung phản hồi để gỡ lỗi
                                var responseContent = await response.Content.ReadAsStringAsync();
                                MessageBox.Show($"Failed to update news. Status Code: {response.StatusCode}\nResponse: {responseContent}");
                            }
                        }
                        catch (HttpRequestException ex)
                        {
                            MessageBox.Show($"Request error: {ex.Message}");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please select a news item to update.");
                }
            }
        }
    }
}
