using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
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

                txtTitle.Text = selectedRow.Cells["Title"].Value?.ToString() ?? string.Empty;
                txtDescription.Text = selectedRow.Cells["Description"].Value?.ToString() ?? string.Empty;
                txtContent.Text = selectedRow.Cells["Content"].Value?.ToString() ?? string.Empty;
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
                var confirmation = MessageBox.Show("Are you sure you want to delete the selected news items?", "Confirm Delete", MessageBoxButtons.YesNo);

                if (confirmation == DialogResult.Yes)
                {
                    bool allDeleted = true;
                    List<string> failedDeletes = new List<string>();

                    foreach (DataGridViewRow selectedRow in dataGridView1.SelectedRows)
                    {
                        var id = selectedRow.Cells["Id"].Value.ToString();

                        try
                        {
                            string apiUrl = $"https://localhost:7161/api/News/{id}";
                            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GlobalVariables.JwtToken);

                            HttpResponseMessage response = await client.DeleteAsync(apiUrl);

                            if (!response.IsSuccessStatusCode)
                            {
                                allDeleted = false;
                                failedDeletes.Add(id);
                            }
                        }
                        catch (HttpRequestException ex)
                        {
                            MessageBox.Show($"Request error for news ID {id}: {ex.Message}");
                            allDeleted = false;
                            failedDeletes.Add(id);
                        }
                    }

                    if (allDeleted)
                    {
                        MessageBox.Show("All selected news deleted successfully!");
                    }
                    else
                    {
                        MessageBox.Show("Failed to delete some news. Failed IDs: " + string.Join(", ", failedDeletes));
                    }

                    LoadNewsData(); // Làm mới dữ liệu trong dataGridView1
                }
            }
            else
            {
                MessageBox.Show("Please select news items to delete.");
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

        private async void searchBtn_Click(object sender, EventArgs e)
        {
            string searchType = searchBox.Text.Trim();
            if (string.IsNullOrEmpty(searchType))
            {
                MessageBox.Show("Please enter a type to search.");
                return;
            }

            try
            {
                string apiUrl = $"https://localhost:7161/api/News/type/{searchType}";
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GlobalVariables.JwtToken);

                HttpResponseMessage response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    var newsList = JsonConvert.DeserializeObject<List<NewsDTO>>(responseBody);

                    dataGridView1.DataSource = newsList;
                }
                else
                {
                    MessageBox.Show($"Failed to fetch news by type. Status Code: {response.StatusCode}");
                }
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Request error: {ex.Message}");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var form5 = new CreateNewsForm();
            form5.FormClosed += (s, args) => LoadNewsData(); // Refresh the data when the form is closed
            form5.Show();
        }

        private async void button4_Click(object sender, EventArgs e)
        {
            string totalPageStr = "1"; // Default value
            if (InputBox.Show("Start Crawling", "Enter the number of pages to crawl:", ref totalPageStr) == DialogResult.OK)
            {
                if (int.TryParse(totalPageStr, out int totalPage))
                {
                    var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(totalPage), System.Text.Encoding.UTF8, "application/json");

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GlobalVariables.JwtToken);

                    try
                    {
                        HttpResponseMessage response = await client.PostAsync("https://localhost:7161/api/Crawl/start", content);

                        if (response.IsSuccessStatusCode)
                        {
                            MessageBox.Show("Crawling started successfully!");
                        }
                        else
                        {
                            MessageBox.Show("Failed to start crawling. Status Code: " + response.StatusCode);
                        }
                    }
                    catch (HttpRequestException ex)
                    {
                        MessageBox.Show($"Request error: {ex.Message}");
                    }
                }
                else
                {
                    MessageBox.Show("Invalid number of pages entered.");
                }
            }
        }

        private async void button5_Click(object sender, EventArgs e)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GlobalVariables.JwtToken);

            try
            {
                HttpResponseMessage response = await client.PostAsync("https://localhost:7161/api/Crawl/stop", null);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Crawling stopped successfully!");
                }
                else
                {
                    MessageBox.Show("Failed to stop crawling. Status Code: " + response.StatusCode);
                }
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Request error: {ex.Message}");
            }
        }
    }
}
