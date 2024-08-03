using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace adminTinTuc
{
    public partial class AddAccountForm : Form
    {
        private readonly AccountService _accountService;

        public AddAccountForm()
        {
            _accountService = new AccountService();
            InitializeComponent();
        }

        private async void addBtn_Click(object sender, EventArgs e)
        {
            var email = emailBox.Text;
            var password = passwordBox.Text;
            var name = nameBox.Text;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Email and Password are required.");
                return;
            }

            var roles = userBtn.Checked ? "User" : adminBtn.Checked ? "Admin" : null;

            var accountDto = new AccountRegistrationDto
            {
                Email = email,
                Password = password,
                Name = name,
                Roles = roles
            };

            var result = await _accountService.RegisterAccountAsync(accountDto);
            MessageBox.Show(result);
            this.Close();
        }
    }
}
