using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace adminTinTuc
{
    public partial class Form5 : Form
    {
        private static readonly HttpClient client = new HttpClient();

        public Form5()
        {
            InitializeComponent();
            LoadCommentsData();
        }

        private async void LoadCommentsData()
        {
            try
            {
                string apiUrl = "https://localhost:7161/api/Comments";
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GlobalVariables.JwtToken);

                HttpResponseMessage response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    var commentsList = System.Text.Json.JsonSerializer.Deserialize<List<CommentDTO>>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    // Chuyển đổi danh sách bình luận thành danh sách các đối tượng hiển thị lên DataGridView
                    var displayComments = new List<CommentDisplay>();

                    foreach (var comment in commentsList)
                    {
                        foreach (var userComment in comment.Comments)
                        {
                            displayComments.Add(new CommentDisplay
                            {
                                CommentId = userComment.CommentId,
                                Id = comment.Id,
                                FromUserId = userComment.FromUserId,
                                FromUserName = userComment.FromUserName,
                                ToUserId = userComment.ToUserId,
                                ToUserName = userComment.ToUserName,
                                Content = userComment.Content,
                                CreateAt = userComment.CreateAt
                            });
                        }
                    }

                    dataGridViewComment.DataSource = displayComments;
                }
                else
                {
                    MessageBox.Show("Failed to load comments. Status Code: " + response.StatusCode);
                }
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Request error: {ex.Message}");
            }
        }

        public class UserCommentDetails
        {
            public string CommentId { get; set; }
            public string FromUserId { get; set; }
            public string FromUserName { get; set; }
            public string ToUserId { get; set; }
            public string ToUserName { get; set; }
            public string Content { get; set; }
            public DateTime CreateAt { get; set; }
        }

        public class CommentDTO
        {
            public string Id { get; set; }
            public List<UserCommentDetails> Comments { get; set; }
        }

        public class CommentDisplay
        {
            public string CommentId { get; set; }
            public string Id { get; set; }
            public string FromUserId { get; set; }
            public string FromUserName { get; set; }
            public string ToUserId { get; set; }
            public string ToUserName { get; set; }
            public string Content { get; set; }
            public DateTime CreateAt { get; set; }
        }

        public class ReplyComment
        {
            public string newsId { get; set; }
            public string FromUserId { get; set; }
            public string ToUserId { get; set; }
            public string Content { get; set; }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridViewComment.SelectedRows.Count > 0)
            {
                var selectedRow = dataGridViewComment.SelectedRows[0];

                txtCommentContent.Text = selectedRow.Cells["Content"].Value?.ToString() ?? string.Empty;
            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            if (dataGridViewComment.SelectedRows.Count > 0)
            {
                var selectedRow = dataGridViewComment.SelectedRows[0];
                var newsId = selectedRow.Cells["Id"].Value?.ToString();
                var toUserID = selectedRow.Cells["FromUserId"].Value?.ToString();

                using (ReplyForm replyForm = new ReplyForm())
                {
                    if (replyForm.ShowDialog() == DialogResult.OK)
                    {
                        var replyContent = replyForm.ReplyContent;

                        var newComment = new ReplyComment
                        {
                            newsId = newsId,
                            FromUserId = GlobalVariables.CurrentUserId,
                            ToUserId = toUserID,
                            Content = replyContent
                        };

                        try
                        {
                            string apiUrl = "https://localhost:7161/api/Comments/add-comment";
                            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GlobalVariables.JwtToken);

                            var json = JsonConvert.SerializeObject(newComment);
                            var content = new StringContent(json, Encoding.UTF8, "application/json");

                            HttpResponseMessage response = await client.PostAsync(apiUrl, content);

                            if (response.IsSuccessStatusCode)
                            {
                                MessageBox.Show("Comment added successfully!");
                                LoadCommentsData(); // Làm mới dữ liệu trong dataGridView1
                            }
                            else
                            {
                                MessageBox.Show("Failed to add comment. Status Code: " + response.StatusCode);
                            }
                        }
                        catch (HttpRequestException ex)
                        {
                            MessageBox.Show($"Request error: {ex.Message}");
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a comment to reply.");
            }
        }

        private async void button2_Click(object sender, EventArgs e)
        {

            if (dataGridViewComment.SelectedRows.Count > 0)
            {
                var confirmation = MessageBox.Show("Are you sure you want to delete the selected news items?", "Confirm Delete", MessageBoxButtons.YesNo);

                if (confirmation == DialogResult.Yes)
                {
                    bool allDeleted = true;
                    List<string> failedDeletes = new List<string>();

                    foreach (DataGridViewRow selectedRow in dataGridViewComment.SelectedRows)
                    {
                        var id = selectedRow.Cells["CommentId"].Value.ToString();

                        try
                        {
                            string apiUrl = $"https://localhost:7161/api/Comments/remove-comment";
                            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GlobalVariables.JwtToken);

                            var jsonContent = new StringContent(JsonConvert.SerializeObject(id), Encoding.UTF8, "application/json");
                            HttpResponseMessage response = await client.PostAsync(apiUrl, jsonContent);

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

                    LoadCommentsData();
                }
            }
            else
            {
                MessageBox.Show("Please select news items to delete.");
            }
        }
    }
}
