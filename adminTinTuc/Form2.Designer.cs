namespace adminTinTuc
{
    partial class Form2
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.commentManagement = new System.Windows.Forms.Button();
            this.newsManagement = new System.Windows.Forms.Button();
            this.accountManagement = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.commentManagement);
            this.panel1.Controls.Add(this.newsManagement);
            this.panel1.Controls.Add(this.accountManagement);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(931, 450);
            this.panel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft New Tai Lue", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.label1.Location = new System.Drawing.Point(363, 88);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(225, 40);
            this.label1.TabIndex = 3;
            this.label1.Text = "ADMIN PANEL";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // commentManagement
            // 
            this.commentManagement.Font = new System.Drawing.Font("Microsoft Tai Le", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.commentManagement.Location = new System.Drawing.Point(636, 182);
            this.commentManagement.Name = "commentManagement";
            this.commentManagement.Size = new System.Drawing.Size(257, 64);
            this.commentManagement.TabIndex = 2;
            this.commentManagement.Text = "COMMENT MANAGEMENT";
            this.commentManagement.UseVisualStyleBackColor = true;
            // 
            // newsManagement
            // 
            this.newsManagement.Font = new System.Drawing.Font("Microsoft Tai Le", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.newsManagement.Location = new System.Drawing.Point(345, 182);
            this.newsManagement.Name = "newsManagement";
            this.newsManagement.Size = new System.Drawing.Size(257, 64);
            this.newsManagement.TabIndex = 1;
            this.newsManagement.Text = "NEWS MANAGEMENT";
            this.newsManagement.UseVisualStyleBackColor = true;
            this.newsManagement.Click += new System.EventHandler(this.newsManagement_Click);
            // 
            // accountManagement
            // 
            this.accountManagement.Font = new System.Drawing.Font("Microsoft Tai Le", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.accountManagement.Location = new System.Drawing.Point(35, 182);
            this.accountManagement.Name = "accountManagement";
            this.accountManagement.Size = new System.Drawing.Size(273, 64);
            this.accountManagement.TabIndex = 0;
            this.accountManagement.Text = "ACCOUNT MANAGEMENT";
            this.accountManagement.UseVisualStyleBackColor = true;
            this.accountManagement.Click += new System.EventHandler(this.accountManagement_Click);
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(931, 450);
            this.Controls.Add(this.panel1);
            this.MaximizeBox = false;
            this.Name = "Form2";
            this.Text = "Form2";
            this.Load += new System.EventHandler(this.Form2_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button commentManagement;
        private System.Windows.Forms.Button newsManagement;
        private System.Windows.Forms.Button accountManagement;
        private System.Windows.Forms.Label label1;
    }
}