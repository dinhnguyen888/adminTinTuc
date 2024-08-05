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
using static adminTinTuc.Form4;

namespace adminTinTuc
{
    public partial class CreateNewsForm : Form
    {
        private static readonly HttpClient client = new HttpClient();

        public CreateNewsForm()
        {
            InitializeComponent();
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            var newsDTO = new NewsDTO
            {
                Title = txtTitle.Text,
                LinkDetail = txtLinkDetail.Text,
                ImageUrl = txtImageUrl.Text,
                Description = txtDescription.Text,
                Content = txtContent.Text,
                Type = txtType.Text
            };

            string json = System.Text.Json.JsonSerializer.Serialize(newsDTO);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GlobalVariables.JwtToken);

            try
            {
                HttpResponseMessage response = await client.PostAsync("https://localhost:7161/api/News", content);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("News created successfully!");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Failed to create news. Status Code: " + response.StatusCode);
                }
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Request error: {ex.Message}");
            }
        }
    }
}
