using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace myClient
{
    public partial class formClient : Form
    {
        Socket cliSocket;
        byte[] receivedBytes = new byte[2048];
        byte[] fileData;

        public formClient()
        {
            InitializeComponent();
            TextBox.CheckForIllegalCrossThreadCalls = false;
        }
        
        private void clientConnect_Click(object sender, EventArgs e)
        {
            cliSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            cliSocket.Connect(clientIP.Text, Convert.ToInt32(clientPort.Text));
        }

        private void clientBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog fDialog = new OpenFileDialog();
            if (fDialog.ShowDialog() == DialogResult.OK)
            {
                MessageBox.Show(fDialog.FileName.ToString());
                clientText.Text = fDialog.FileName.ToString();
            }
            fDialog.AddExtension = true;
            fDialog.CheckFileExists = true;
            fDialog.CheckPathExists = true;  
        }

        private void clientSend_Click(object sender, EventArgs e)
        {
            ASCIIEncoding aEncoder = new ASCIIEncoding();
            fileData = File.ReadAllBytes(clientText.Text);
            string username = clientUsername.Text;
            string filename = Path.GetFileName(clientText.Text);
            Int32 filesize = fileData.Length;
            byte[] callback = new byte[4];
            cliSocket.Receive(callback);
            Console.WriteLine(aEncoder.GetString(callback));
            if (BitConverter.ToInt32(callback, 0) == -1)
            {
                MessageBox.Show("Your username already exists in the server. Connection request denied.");
                //TODO: DO smthng
                return;
            }
            byte[] usernameInBytes = aEncoder.GetBytes(username);
            byte[] filenameInBytes = aEncoder.GetBytes(filename);
            byte[] usernameInfo = new byte[4 + username.Length];
            byte[] fileInfo = new byte[8 + filename.Length];

            //Filling usernameData byte array
            Buffer.BlockCopy(BitConverter.GetBytes(usernameInBytes.Length), 0, usernameInfo, 0, 4);
            Buffer.BlockCopy(usernameInBytes, 0, usernameInfo, 4, usernameInBytes.Length);
            cliSocket.Send(usernameInfo);

            //Filling fileInfo byte array
            Buffer.BlockCopy(BitConverter.GetBytes(filenameInBytes.Length), 0, fileInfo, 0, 4);
            Buffer.BlockCopy(filenameInBytes, 0, fileInfo, 4, filenameInBytes.Length);
            Buffer.BlockCopy(BitConverter.GetBytes(filesize), 0, fileInfo, 4 + filenameInBytes.Length, 4);
            cliSocket.Send(fileInfo);
            Thread.Sleep(1000);
            int size = cliSocket.Send(fileData);
            Console.WriteLine(size);
        }

        private void formClient_Load(object sender, EventArgs e)
        {
            clientConnect.Enabled = false;
        }

        private void clientConnect_Click_1(object sender, EventArgs e)
        {
            if (clientConnect.Text == "Connect")   
            {
                try
                {
                    cliSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    cliSocket.Connect(clientIP.Text, Convert.ToInt32(clientPort.Text));
                    clientConnect.Text = "Disconnect!";
                    clientConnect.BackColor = Color.Orange;
                }
                catch
                {
                    MessageBox.Show("ERROR: UNABLE TO CONNECT");
                }
            }
            else      // For disconnection   
            {
                clientConnect.Text = "Connect";
                clientConnect.BackColor = DefaultBackColor;
                clientSend.Enabled = false;
                cliSocket.Shutdown(SocketShutdown.Both);
                cliSocket.Close();
            }
        }
        //  Enable Connect button if the username box is not empty
        private void clientUsername_TextChanged(object sender, EventArgs e)  
        {
            clientConnect.Enabled = !string.IsNullOrEmpty(clientUsername.Text);
        }
    }
}
