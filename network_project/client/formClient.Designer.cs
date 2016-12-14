namespace myClient
{
    partial class clientDownloadPath
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
            this.clientBox = new System.Windows.Forms.RichTextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.clientView = new System.Windows.Forms.Button();
            this.clientOldFile = new System.Windows.Forms.TextBox();
            this.clientNewFile = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.clientRename = new System.Windows.Forms.Button();
            this.clientDosya = new System.Windows.Forms.TextBox();
            this.clientDownload = new System.Windows.Forms.Button();
            this.clientDelete = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.clientDownPath = new System.Windows.Forms.TextBox();
            this.clientDownBrowse = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // clientSend
            // 
            this.clientSend.Location = new System.Drawing.Point(218, 270);
            this.clientSend.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.clientSend.Name = "clientSend";
            this.clientSend.Size = new System.Drawing.Size(75, 23);
            this.clientSend.TabIndex = 0;
            this.clientSend.Text = "Send";
            this.clientSend.UseVisualStyleBackColor = true;
            this.clientSend.Click += new System.EventHandler(this.clientSend_Click);
            // 
            // clientText
            // 
            this.clientText.Location = new System.Drawing.Point(15, 196);
            this.clientText.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.clientText.Name = "clientText";
            this.clientText.Size = new System.Drawing.Size(278, 22);
            this.clientText.TabIndex = 1;
            this.clientText.TextChanged += new System.EventHandler(this.clientText_TextChanged);
            // 
            // clientIP
            // 
            this.clientIP.Location = new System.Drawing.Point(122, 13);
            this.clientIP.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.clientIP.Name = "clientIP";
            this.clientIP.Size = new System.Drawing.Size(171, 22);
            this.clientIP.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(28, 17);
            this.label1.TabIndex = 5;
            this.label1.Text = "IP :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 17);
            this.label2.TabIndex = 6;
            this.label2.Text = "Port :";
            // 
            // clientPort
            // 
            this.clientPort.Location = new System.Drawing.Point(123, 50);
            this.clientPort.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.clientPort.Name = "clientPort";
            this.clientPort.Size = new System.Drawing.Size(171, 22);
            this.clientPort.TabIndex = 7;
            // 
            // clientBrowse
            // 
            this.clientBrowse.Location = new System.Drawing.Point(218, 233);
            this.clientBrowse.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.clientBrowse.Name = "clientBrowse";
            this.clientBrowse.Size = new System.Drawing.Size(75, 25);
            this.clientBrowse.TabIndex = 8;
            this.clientBrowse.Text = "Browse";
            this.clientBrowse.UseVisualStyleBackColor = true;
            this.clientBrowse.Click += new System.EventHandler(this.clientBrowse_Click);
            // 
            // clientUsername
            // 
            this.clientUsername.Location = new System.Drawing.Point(123, 91);
            this.clientUsername.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.clientUsername.Name = "clientUsername";
            this.clientUsername.Size = new System.Drawing.Size(171, 22);
            this.clientUsername.TabIndex = 9;
            this.clientUsername.TextChanged += new System.EventHandler(this.clientUsername_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 91);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(81, 17);
            this.label3.TabIndex = 10;
            this.label3.Text = "Username :";
            // 
            // clientConnect
            // 
            this.clientConnect.Location = new System.Drawing.Point(202, 129);
            this.clientConnect.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.clientConnect.Name = "clientConnect";
            this.clientConnect.Size = new System.Drawing.Size(92, 23);
            this.clientConnect.TabIndex = 11;
            this.clientConnect.Text = "Connect";
            this.clientConnect.UseVisualStyleBackColor = true;
            this.clientConnect.Click += new System.EventHandler(this.clientConnect_Click_1);
            // 
            // clientBox
            // 
            this.clientBox.Location = new System.Drawing.Point(122, 334);
            this.clientBox.Name = "clientBox";
            this.clientBox.Size = new System.Drawing.Size(657, 171);
            this.clientBox.TabIndex = 13;
            this.clientBox.Text = "";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 334);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(43, 17);
            this.label5.TabIndex = 14;
            this.label5.Text = "Logs:";
            // 
            // clientView
            // 
            this.clientView.Location = new System.Drawing.Point(651, 511);
            this.clientView.Name = "clientView";
            this.clientView.Size = new System.Drawing.Size(128, 30);
            this.clientView.TabIndex = 15;
            this.clientView.Text = "View my file list";
            this.clientView.UseVisualStyleBackColor = true;
            this.clientView.Click += new System.EventHandler(this.clientView_Click);
            // 
            // clientOldFile
            // 
            this.clientOldFile.Location = new System.Drawing.Point(541, 196);
            this.clientOldFile.Name = "clientOldFile";
            this.clientOldFile.Size = new System.Drawing.Size(238, 22);
            this.clientOldFile.TabIndex = 16;
            this.clientOldFile.TextChanged += new System.EventHandler(this.clientOldFile_TextChanged);
            // 
            // clientNewFile
            // 
            this.clientNewFile.Location = new System.Drawing.Point(541, 230);
            this.clientNewFile.Name = "clientNewFile";
            this.clientNewFile.Size = new System.Drawing.Size(238, 22);
            this.clientNewFile.TabIndex = 17;
            this.clientNewFile.TextChanged += new System.EventHandler(this.clientNewFile_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(384, 196);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(95, 17);
            this.label6.TabIndex = 19;
            this.label6.Text = "Old file name:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(384, 230);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(100, 17);
            this.label7.TabIndex = 20;
            this.label7.Text = "New file name:";
            // 
            // clientRename
            // 
            this.clientRename.Location = new System.Drawing.Point(706, 267);
            this.clientRename.Name = "clientRename";
            this.clientRename.Size = new System.Drawing.Size(73, 26);
            this.clientRename.TabIndex = 21;
            this.clientRename.Text = "Rename!";
            this.clientRename.UseVisualStyleBackColor = true;
            this.clientRename.Click += new System.EventHandler(this.clientRename_Click);
            // 
            // clientDosya
            // 
            this.clientDosya.Location = new System.Drawing.Point(387, 13);
            this.clientDosya.Name = "clientDosya";
            this.clientDosya.Size = new System.Drawing.Size(392, 22);
            this.clientDosya.TabIndex = 22;
            
            // 
            // clientDownload
            // 
            this.clientDownload.Location = new System.Drawing.Point(692, 88);
            this.clientDownload.Name = "clientDownload";
            this.clientDownload.Size = new System.Drawing.Size(87, 28);
            this.clientDownload.TabIndex = 23;
            this.clientDownload.Text = "Download";
            this.clientDownload.UseVisualStyleBackColor = true;
            this.clientDownload.Click += new System.EventHandler(this.clientDownload_Click);
            // 
            // clientDelete
            // 
            this.clientDelete.Location = new System.Drawing.Point(692, 129);
            this.clientDelete.Name = "clientDelete";
            this.clientDelete.Size = new System.Drawing.Size(87, 26);
            this.clientDelete.TabIndex = 24;
            this.clientDelete.Text = "Delete";
            this.clientDelete.UseVisualStyleBackColor = true;
            this.clientDelete.Click += new System.EventHandler(this.clientDelete_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(384, 50);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(142, 17);
            this.label4.TabIndex = 25;
            this.label4.Text = "Where to download? ";
            // 
            // clientDownPath
            // 
            this.clientDownPath.Location = new System.Drawing.Point(545, 50);
            this.clientDownPath.Name = "clientDownPath";
            this.clientDownPath.Size = new System.Drawing.Size(234, 22);
            this.clientDownPath.TabIndex = 26;
        
            // 
            // clientDownBrowse
            // 
            this.clientDownBrowse.Location = new System.Drawing.Point(545, 88);
            this.clientDownBrowse.Name = "clientDownBrowse";
            this.clientDownBrowse.Size = new System.Drawing.Size(87, 28);
            this.clientDownBrowse.TabIndex = 27;
            this.clientDownBrowse.Text = "Browse";
            this.clientDownBrowse.UseVisualStyleBackColor = true;
            this.clientDownBrowse.Click += new System.EventHandler(this.clientDownBrowse_Click);
            // 
            // clientDownloadPath
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(791, 560);
            this.Controls.Add(this.clientDownBrowse);
            this.Controls.Add(this.clientDownPath);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.clientDelete);
            this.Controls.Add(this.clientDownload);
            this.Controls.Add(this.clientDosya);
            this.Controls.Add(this.clientRename);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.clientNewFile);
            this.Controls.Add(this.clientOldFile);
            this.Controls.Add(this.clientView);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.clientBox);
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
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "clientDownloadPath";
            this.Text = "ClientGUI";
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
        private System.Windows.Forms.RichTextBox clientBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button clientView;
        private System.Windows.Forms.TextBox clientOldFile;
        private System.Windows.Forms.TextBox clientNewFile;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button clientRename;
        private System.Windows.Forms.TextBox clientDosya;
        private System.Windows.Forms.Button clientDownload;
        private System.Windows.Forms.Button clientDelete;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox clientDownPath;
        private System.Windows.Forms.Button clientDownBrowse;
    }
}

