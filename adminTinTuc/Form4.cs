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

                //dataGridView1.Columns["Title"].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                //dataGridView1.Columns["Description"].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                //dataGridView1.Columns["Content"].DefaultCellStyle.WrapMode = DataGridViewTriState.True;

                //dataGridView1.Columns["Content"].Width = 700;

                // Auto-size rows to fit the content
                //dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
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
                        //var response = await client.DeleteAsync($"https://localhost:7161/api/News/{id}");
                        //response.EnsureSuccessStatusCode();
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

                        // Refresh the DataGridView
                        //LoadNewsData();
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
    }
}
