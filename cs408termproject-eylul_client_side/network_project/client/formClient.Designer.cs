namespace myClient
{
    partial class formClient
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
            this.clientSend = new System.Windows.Forms.Button();
            this.clientText = new System.Windows.Forms.TextBox();
            this.clientIP = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.clientPort = new System.Windows.Forms.TextBox();
            this.clientBrowse = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.clientUsername = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.clientConnect = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // clientSend
            // 
            this.clientSend.Location = new System.Drawing.Point(467, 345);
            this.clientSend.Name = "clientSend";
            this.clientSend.Size = new System.Drawing.Size(75, 23);
            this.clientSend.TabIndex = 0;
            this.clientSend.Text = "Send";
            this.clientSend.UseVisualStyleBackColor = true;
            this.clientSend.Click += new System.EventHandler(this.clientSend_Click);
            // 
            // clientText
            // 
            this.clientText.Location = new System.Drawing.Point(332, 259);
            this.clientText.Name = "clientText";
            this.clientText.Size = new System.Drawing.Size(210, 22);
            this.clientText.TabIndex = 1;
            // 
            // clientIP
            // 
            this.clientIP.Location = new System.Drawing.Point(123, 35);
            this.clientIP.Name = "clientIP";
            this.clientIP.Size = new System.Drawing.Size(171, 22);
            this.clientIP.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(34, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(28, 17);
            this.label1.TabIndex = 5;
            this.label1.Text = "IP :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(34, 91);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 17);
            this.label2.TabIndex = 6;
            this.label2.Text = "Port :";
            // 
            // clientPort
            // 
            this.clientPort.Location = new System.Drawing.Point(123, 86);
            this.clientPort.Name = "clientPort";
            this.clientPort.Size = new System.Drawing.Size(171, 22);
            this.clientPort.TabIndex = 7;
            // 
            // clientBrowse
            // 
            this.clientBrowse.Location = new System.Drawing.Point(467, 302);
            this.clientBrowse.Name = "clientBrowse";
            this.clientBrowse.Size = new System.Drawing.Size(75, 25);
            this.clientBrowse.TabIndex = 8;
            this.clientBrowse.Text = "Browse";
            this.clientBrowse.UseVisualStyleBackColor = true;
            this.clientBrowse.Click += new System.EventHandler(this.clientBrowse_Click);
            // 
            // folderBrowserDialog1
            // 
           
            // 
            // clientUsername
            // 
            this.clientUsername.Location = new System.Drawing.Point(123, 133);
            this.clientUsername.Name = "clientUsername";
            this.clientUsername.Size = new System.Drawing.Size(171, 22);
            this.clientUsername.TabIndex = 9;
            this.clientUsername.TextChanged += new System.EventHandler(this.clientUsername_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(30, 138);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(81, 17);
            this.label3.TabIndex = 10;
            this.label3.Text = "Username :";
            // 
            // clientConnect
            // 
            this.clientConnect.Location = new System.Drawing.Point(166, 173);
            this.clientConnect.Name = "clientConnect";
            this.clientConnect.Size = new System.Drawing.Size(128, 23);
            this.clientConnect.TabIndex = 11;
            this.clientConnect.Text = "Connect";
            this.clientConnect.UseVisualStyleBackColor = true;
            this.clientConnect.Click += new System.EventHandler(this.clientConnect_Click_1);
            // 
            // formClient
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(647, 445);
            this.Controls.Add(this.clientConnect);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.clientUsername);
            this.Controls.Add(this.clientBrowse);
            this.Controls.Add(this.clientPort);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.clientIP);
            this.Controls.Add(this.clientText);
            this.Controls.Add(this.clientSend);
            this.Name = "formClient";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.formClient_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button clientSend;
        private System.Windows.Forms.TextBox clientText;
        private System.Windows.Forms.TextBox clientIP;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox clientPort;
        private System.Windows.Forms.Button clientBrowse;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.TextBox clientUsername;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button clientConnect;
    }
}

