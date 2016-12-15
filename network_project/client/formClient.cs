/*
 * Author: Eylül Dicle Yurdakul
 * Date: 12/19/2016
 */

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
    public partial class clientDownloadPath : Form
    {
        Socket cliSocket;
        byte[] receivedBytes = new byte[2048];

       

    public clientDownloadPath()
        {
            InitializeComponent();
            TextBox.CheckForIllegalCrossThreadCalls = false;
            clientPort.Text = "8888";
        }

        private void printLogger(string message)
        {
            string now = DateTime.Now.ToString(@"MM\/dd\/yyyy h\:mm tt");
            clientBox.AppendText(now + "--> " + message + "\n");
        }

        private void clientBrowse_Click(object sender, EventArgs e)
        {
            printLogger("You are connected to the system as " + clientUsername.Text + ".");
            OpenFileDialog fDialog = new OpenFileDialog();
            if (fDialog.ShowDialog() == DialogResult.OK)
            {
                MessageBox.Show(fDialog.FileName.ToString());
                clientText.Text = fDialog.FileName.ToString();
                printLogger("You chose " + Path.GetFileName(fDialog.FileName) + " to upload.");
            }
            fDialog.AddExtension = true;
            fDialog.CheckFileExists = true;
            fDialog.CheckPathExists = true;  
        }

        private void clientSend_Click(object sender, EventArgs e)
        {
            string operationCode = "UPL";
            UTF8Encoding aEncoder = new UTF8Encoding();
            byte[] codeInBytes = aEncoder.GetBytes(operationCode);
            cliSocket.Send(codeInBytes);

            printLogger("System is uploading the file/folder... ");
            long filesize = new FileInfo(clientText.Text).Length;
            string filename = Path.GetFileName(clientText.Text);

            byte[] filenameInBytes = aEncoder.GetBytes(filename);
            byte[] fileInfo = new byte[12 + filename.Length];

            //Filling fileInfo byte array
            Buffer.BlockCopy(BitConverter.GetBytes(filenameInBytes.Length), 0, fileInfo, 0, 4);
            Buffer.BlockCopy(filenameInBytes, 0, fileInfo, 4, filenameInBytes.Length);
            Buffer.BlockCopy(BitConverter.GetBytes(filesize), 0, fileInfo, 4 + filenameInBytes.Length, sizeof(long));

            try
            {
                cliSocket.Send(fileInfo);
            }
            catch (Exception exc)
            {
                MessageBox.Show("Exception occured during data transfer: " + exc.Message);
                return;
            }

            Thread.Sleep(1000);

            try
            {
                cliSocket.BeginSendFile(clientText.Text, new AsyncCallback(FileSendCallback), cliSocket);
            }
            catch (Exception exc)
            {
                MessageBox.Show("Exception occured during data transfer: " + exc.Message);
                return;
            }
        }

        private void FileSendCallback(IAsyncResult ar)
        {
            // Retrieve the socket from the state object.
            Socket client = (Socket)ar.AsyncState;

            // Complete sending the data to the remote device.
            printLogger("File transfer complete");
            try
            {
                client.EndSendFile(ar);
            }
            catch (Exception exc)
            {
                MessageBox.Show("Socket exception occured.");
                clientConnect.Text = "Connect";
                clientConnect.BackColor = DefaultBackColor;
                clientSend.Enabled = false;
                cliSocket.Shutdown(SocketShutdown.Both);
                cliSocket.Close();
            }
        }

        private void formClient_Load(object sender, EventArgs e)
        {
            clientConnect.Enabled = false;
            clientDownload.Enabled = false;
            clientDelete.Enabled = false;
            clientSend.Enabled = false;
            clientRename.Enabled = false;
            clientView.Enabled = false;
        }


        private string getLocalIP()
        {
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    printLogger("Client ip: " + ip.ToString());
                    return ip.ToString();
                }
            }
            printLogger("Client ip: 127.0.0.1 ");
            return "127.0.0.1";
        }
        private void clientNotification()
        {
            byte[] notification = new byte[128];
            int receivedData = cliSocket.Receive(notification);
            if (receivedData != 0)
            {
                MessageBox.Show("Server stopped listening.");
            }
        }

        private void clientConnect_Click_1(object sender, EventArgs e)
        {
            if (clientConnect.Text == "Connect")   
            {

                try
                {
                    if (clientIP.Text != getLocalIP())
                    {
                        MessageBox.Show("The IP you entered is not valid in this context.");
                        return;
                    }
                    else
                    { 
                        cliSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                        cliSocket.Connect(clientIP.Text, Convert.ToInt32(clientPort.Text));

                        ASCIIEncoding aEncoder = new ASCIIEncoding();
                        string username = clientUsername.Text;
                        byte[] usernameInBytes = aEncoder.GetBytes(username);
                        byte[] usernameInfo = new byte[4 + username.Length];  //  usernamein sizeı için buradaki 4

                        //Filling usernameData byte array
                        Buffer.BlockCopy(BitConverter.GetBytes(usernameInBytes.Length), 0, usernameInfo, 0, 4);
                        Buffer.BlockCopy(usernameInBytes, 0, usernameInfo, 4, usernameInBytes.Length);

                        try
                        {
                            cliSocket.Send(usernameInfo);
                            byte[] result = new byte[4];
                            cliSocket.Receive(result);
                            if (BitConverter.ToInt32(result, 0).Equals(1))
                            {
                                MessageBox.Show("Connection refused. Your username is not unique");
                                cliSocket.Shutdown(SocketShutdown.Both);
                                cliSocket.Close();
                                return;
                            }

                        }
                        catch (Exception exc)
                        {
                            MessageBox.Show("Exception occured during data transfer: " + exc.Message);
                            return;
                        }
                        clientConnect.Text = "Disconnect!";
                        clientConnect.BackColor = Color.PaleVioletRed;
                        clientSend.Enabled = true;
                    }
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

        // Enable Connect and View buttons if the username box is not empty
        private void clientUsername_TextChanged(object sender, EventArgs e)  
        {
            clientConnect.Enabled = !string.IsNullOrEmpty(clientUsername.Text);
            clientView.Enabled = !string.IsNullOrEmpty(clientUsername.Text);
        }

        // View your files from the server
        private void clientView_Click(object sender, EventArgs e)
        {
            printLogger("You requested to view your file list.");
            UTF8Encoding aEncoder = new UTF8Encoding();
            string operationCode = "BRW";
            byte[] codeInBytes = aEncoder.GetBytes(operationCode);
            try
            {
                cliSocket.Send(codeInBytes);
                byte[] fileListInfo = new byte[1024];
                cliSocket.Receive(fileListInfo);
                string result = Encoding.UTF8.GetString(fileListInfo); 
                printLogger(result);
            }
            catch (Exception exc)
            {
                MessageBox.Show("Exception occured during file viewing: " + exc.Message);
                return;
            }
        }

        // Rename your files in the server
        private void clientRename_Click(object sender, EventArgs e)
        {
            UTF8Encoding encoder = new UTF8Encoding();
            string operationCode = "RNM";
            byte[] codeInBytes = encoder.GetBytes(operationCode);
            string oldFileName = clientOldFile.Text;
            string newFileName = clientNewFile.Text;
            byte[] oldbytes = encoder.GetBytes(oldFileName);
            byte[] newbytes = encoder.GetBytes(newFileName);
            byte[] fileInfo = new byte[3 + 4 + oldFileName.Length + 4 + newFileName.Length];

            //Filling fileInfo byte array
            Buffer.BlockCopy(codeInBytes, 0, fileInfo, 0, 3);
            Buffer.BlockCopy(BitConverter.GetBytes(oldbytes.Length), 0, fileInfo, 3, 4);
            Buffer.BlockCopy(oldbytes, 0, fileInfo, 7, oldFileName.Length);
            Buffer.BlockCopy(BitConverter.GetBytes(newbytes.Length), 0, fileInfo, 7+ oldFileName.Length, 4);
            Buffer.BlockCopy(newbytes, 0, fileInfo, 7+ oldFileName.Length+4, newFileName.Length);
            cliSocket.Send(fileInfo);

            byte[] result = new byte[4];
            cliSocket.Receive(result);

            switch (BitConverter.ToInt32(result, 0))
            {
                case 0:
                    printLogger("Rename operation is successfully performed");
                    break;
                case 1:
                    printLogger("Rename operation is unsuccessful");
                    break;
                default:
                    printLogger("Something went really really wrong");
                    break;
            }
        }

        // Gets the file from server through byte arrays
        private bool getDataFromServer(Object socketObj, string username)
        {
            Socket socket = (Socket)socketObj;
            string filename = clientDosya.Text;
            byte[] fileInfo = new byte[128];

            socket.Receive(fileInfo);
      
            UTF8Encoding encoder = new UTF8Encoding();
            //Parsing filename and fileSize 
            long fileSize = BitConverter.ToInt64(fileInfo.Take(sizeof(long)).ToArray(), 0);
            if (fileSize == 0)
            {
                printLogger("File not found");
                return false;
            }
            printLogger("File transfer started for user: " + username + " \n" + "Filesize: " + fileSize);
            byte[] data = new byte[8 * 1024];
            FileStream stream = File.Create(clientDownPath.Text + "\\" + filename);
            try
            {
                long bytesLeftToTransfer = fileSize;
                printLogger("Filesize: " + fileSize);

                while (bytesLeftToTransfer > 0)
                {
                    long amountOfBytes = socket.Receive(data);
                    //If 0 bytes is recieved socket connection is dropped
                    if (amountOfBytes == 0)
                    {
                        MessageBox.Show("Client disconnected");
                        throw new SocketException();
                    }
                    long bytesToCopy = Math.Min(amountOfBytes, bytesLeftToTransfer);
                    stream.Write(data, 0, (int)bytesToCopy);
                    bytesLeftToTransfer -= bytesToCopy;
                }
                printLogger("Stream closed for " + username);
                stream.Close();
        }
            catch (SocketException exc)
            {
                MessageBox.Show("Socket Exception occured");
                stream.Close();
                File.Delete(clientDownPath.Text + "\\" + filename);
                printLogger("Corruped file deleting ");
                return false;
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
                return false;
            }
            printLogger("File transfer finished for user:" + username);
            return true;
        }

        // Download a file from server
        private void clientDownload_Click(object sender, EventArgs e)
        {
            string username = clientUsername.Text;
            UTF8Encoding encoder = new UTF8Encoding();
            string operationCode = "DWN";
            byte[] codeInBytes = encoder.GetBytes(operationCode);
            string filename = clientDosya.Text;
            byte[] filenameInBytes = encoder.GetBytes(filename);
            byte[] fileInfo = new byte[3 + 4 + filename.Length];

            //Filling fileInfo byte array
            Buffer.BlockCopy(codeInBytes, 0, fileInfo, 0, 3);
            Buffer.BlockCopy(BitConverter.GetBytes(filenameInBytes.Length), 0, fileInfo, 3, 4);
            Buffer.BlockCopy(filenameInBytes, 0, fileInfo, 7, filename.Length);
            cliSocket.Send(fileInfo);
            getDataFromServer(cliSocket,username);
        }

        // Delete a file from server 
        private void clientDelete_Click(object sender, EventArgs e)
        {
            UTF8Encoding encoder = new UTF8Encoding();
            string operationCode = "DEL";
            byte[] codeInBytes = encoder.GetBytes(operationCode);
            string filename = clientDosya.Text;
            byte[] filenameInBytes = encoder.GetBytes(filename);
            byte[] fileInfo = new byte[3 + 4 + filename.Length];

            //Filling fileInfo byte array
            Buffer.BlockCopy(codeInBytes, 0, fileInfo, 0, 3);
            Buffer.BlockCopy(BitConverter.GetBytes(filenameInBytes.Length), 0, fileInfo, 3, 4);
            Buffer.BlockCopy(filenameInBytes, 0, fileInfo, 7, filename.Length);
            cliSocket.Send(fileInfo);
            byte[] result = new byte[4];
            cliSocket.Receive(result);

            switch (BitConverter.ToInt32(result, 0))
            {
                case 0:
                    printLogger("Delete operation is successfully performed");
                    break;
                case 1:
                    printLogger("Delete operation is unsuccessful");
                    break;
                default:
                    printLogger("Something went really really wrong");
                    break;
            }
        }

        // Choose path to download a file from server
        private void clientDownBrowse_Click(object sender, EventArgs e)
        {
            string folderPath = "";
            FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                folderPath = folderBrowserDialog1.SelectedPath;
                clientDownPath.Text = folderPath;
                printLogger("You chose the path " + folderPath + " to upload items.");
            }
        }

        // Enable Send button if the file to send is chosen
        private void clientText_TextChanged(object sender, EventArgs e)
        {
            clientSend.Enabled= !string.IsNullOrEmpty(clientText.Text);
        }

        // Enable Rename button if the box for old file name is not empty
        private void clientNewFile_TextChanged(object sender, EventArgs e)
        {
            clientRename.Enabled = !string.IsNullOrEmpty(clientOldFile.Text);
        }

        // Enable Rename button if the box for new file name is not empty
        private void clientOldFile_TextChanged(object sender, EventArgs e)
        {
            clientRename.Enabled = !string.IsNullOrEmpty(clientOldFile.Text);
            clientRename.Enabled = !string.IsNullOrEmpty(clientNewFile.Text);
        }

        private void clientDownPath_TextChanged(object sender, EventArgs e)
        {
            clientDownload.Enabled = !string.IsNullOrEmpty(clientDownPath.Text);
        }

        private void clientDosya_TextChanged(object sender, EventArgs e)
        {
            clientDelete.Enabled = !string.IsNullOrEmpty(clientDosya.Text);
            clientDownload.Enabled= !string.IsNullOrEmpty(clientDosya.Text);
        }
    }
}
