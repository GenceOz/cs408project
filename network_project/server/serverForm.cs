/*
 * Author: Gence Özer 
 * Date: 11/19/2016
 */
//Server disconnect client notify, stop listening
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

namespace server
{
    public partial class serverForm : Form
    {
        Socket socket;
        EndPoint epLocal;
        List<string> connectedUsers = new List<String>();
        Thread dispatcherThread;
        Boolean isStoped = true;

        public serverForm()
        {
            InitializeComponent();
        }

        /*
         * This function prints the given message in the 
         * Logger rich text box in the GUI
         */
        private void printLogger(string message)
        {
            string now = DateTime.Now.ToString(@"MM\/dd\/yyyy h\:mm tt");
            RTextBox_Logs.AppendText(now + "--> " + message + "\n");
        }

        /*
         * This function returns local ip address of the server
         * If the server is not connected to internet, it returns 
         * local host ip.
         */
        private string getLocalIP()
        {
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    printLogger("Server ip: " + ip.ToString());
                    return ip.ToString();
                }
            }
            printLogger("Server ip: 127.0.0.1 ");
            return "127.0.0.1";
        }

        /*
         * This function checks if the directory in the given 
         * path exists, if it doesn't it creates a directory
         */
        private void createDirectoryInPath(String path)
        {
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                    printLogger("Creating the directory in the path: " + path);
                }
                else
                {
                    printLogger("Directory already exists in the path: " + path);
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show("Exception occured on directory creation:" + exc.Message);
                return;
            }
        }

        /*
         * This function retrieves path and port information from GUI
         * First, it creates a directory, if not exists, 
         * then it binds a socket and puts it in a listening state to
         * given port info.
         */
        private void initalizeListening()
        {
            //Binding the socket to ip and given port name, puts the socket in listening state
            int portNumber = (int)Numeric_Port.Value;
            epLocal = new IPEndPoint(IPAddress.Parse(getLocalIP()), portNumber);
            try
            {
                socket.Bind(epLocal);
                socket.Listen(portNumber);
            }
            catch (Exception exc)
            {
                MessageBox.Show("Exception occured: " + exc.Message);
            }

            // Sets up the thread which will perform the handshake connection with the client 
            try
            {
                dispatcherThread = new Thread(new ThreadStart(dispatchFileTransferOperations));
                dispatcherThread.IsBackground = true;
                dispatcherThread.Start();
                printLogger("Dispatcher thread starts working.");
                printLogger("Server is now listening on port: " + portNumber);
            }
            catch (Exception exc)
            {
                MessageBox.Show("Thread failed to start: " + exc.Message);
            }
        }

        /*
         * This function continously listens to given port
         * when a connection comes, it transfers the connection
         * to a new socket that will be handled in a seperate thread.
         * After, thread runs the socket keeps on listening for 
         * new incoming connections.
         */
        private void dispatchFileTransferOperations()
        {
            while (true && isStoped)
            {
                try
                {
                    Socket dataTransferSocket = socket.Accept();
                    printLogger("A new incomming connection accepted");
                    Thread clientConnectionThread = new Thread(new ParameterizedThreadStart(handleUserConnection));
                    clientConnectionThread.IsBackground = true;
                    clientConnectionThread.Start(dataTransferSocket);
                }
                catch (Exception exc)
                {
                    MessageBox.Show("Exception occured while listening: " + exc.Message);
                }
            }
        }

        /*
         * This function allows users to make multiple file
         * transfer throughout their connection
         */
        private void handleUserConnection(Object socketObj)
        {
            Socket socket = (Socket)socketObj;
            //Recieves the username, filename and filesize to establish the connection
            byte[] handshakeInfo = new byte[128];
            try
            {
                int recievedData = socket.Receive(handshakeInfo);
                //If 0 bytes are recieved connection is dropped
                if (recievedData == 0)
                {
                    MessageBox.Show("Client disconnected");
                    return;
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show("Socket exception occured: " + exc.Message);
                return;
            }

            UTF8Encoding encoder = new UTF8Encoding();
            //Parsing username 
            int usernameSize = BitConverter.ToInt32(handshakeInfo.Take(4).ToArray(), 0);
            string username = encoder.GetString(handshakeInfo.Skip(4).Take(usernameSize).ToArray());
            printLogger("An incoming connection request from user: " + username);

            if (checkUserList(username))
            {
                //Username already has a active connection, terminate
                printLogger("user " + username + "already has an active connection. Terminating connection.");
                termianteUserConnection(socket, username);
                return;
            }

            //Add the username in the list
            connectedUsers.Add(username);
            sendResultToClient(socket, 0);

            printLogger("Checking for existing directory of user: " + username);
            createDirectoryInPath(serverPath.Text + "\\" + username);

            while (true)
            {
                byte[] operationInfo = new byte[128];
                try
                {
                    int recievedData = socket.Receive(operationInfo);
                    //If 0 bytes are recieved connection is dropped
                    if (recievedData == 0)
                    {
                        MessageBox.Show("Client disconnected");
                        return;
                    }
                }
                catch (Exception exc)
                {
                    MessageBox.Show("Socket exception occured: " + exc.Message);
                    return;
                }

                //Parsing operation type
                //BRW,DEL,RNM,DWN,UPL are possible operation types
                //Parsing username  
                string operation = encoder.GetString(operationInfo.Take(3).ToArray());

                switch (operation)
                {
                    //File upload operation
                    case "UPL":
                        transferData(socketObj, username);
                        break;
                    //File list browse operation
                    case "BRW":
                        requestFileList(socketObj, username);
                        break;
                    //File delete operation
                    case "DEL":
                        //Parse the file to be deleted
                        int filenameSize = BitConverter.ToInt32(operationInfo.Skip(3).Take(4).ToArray(), 0);
                        string filename = encoder.GetString(operationInfo.Skip(3 + 4).Take(filenameSize).ToArray());
                        deleteFile(socketObj, username, filename);
                        break;
                    //File rename operation
                    case "RNM":
                        //Parse the file to be renamed
                        filenameSize = BitConverter.ToInt32(operationInfo.Skip(3).Take(4).ToArray(), 0);
                        filename = encoder.GetString(operationInfo.Skip(3 + 4).Take(filenameSize).ToArray());
                        int newfilenameSize = BitConverter.ToInt32(operationInfo.Skip(3 + 4 + filenameSize).Take(4).ToArray(), 0);
                        string newFilename = encoder.GetString(operationInfo.Skip(3 + 4 + filenameSize + 4).Take(newfilenameSize).ToArray());
                        renameFile(socketObj, username, filename, newFilename);
                        break;
                    //File download operation
                    case "DWN":
                        //Parse the file to be downloaded
                        filenameSize = BitConverter.ToInt32(operationInfo.Skip(3).Take(4).ToArray(), 0);
                        filename = encoder.GetString(operationInfo.Skip(3 + 4).Take(filenameSize).ToArray());
                        dataTransferToClient(socketObj, username, filename);
                        break;
                    default:
                        printLogger("Unknown operation type");
                        break;
                }
            }
        }

        /*
         * This function sends a file list to the client 
         * The list contains name, size, upload time of each file
         */
        private void requestFileList(Object socketObj, string username)
        {
            Socket socket = (Socket)socketObj;
            string directoryPath = serverPath.Text + "\\" + username;
            string[] files = Directory.GetFiles(directoryPath, "*", SearchOption.AllDirectories);

            //Fill the list with name,size,upload time of the files
            string list = "";
            foreach (string filename in files)
            {
                try
                {
                    FileInfo fileInfo = new System.IO.FileInfo(filename);
                    list += filename.Remove(0, directoryPath.Length + 1) + " " + fileInfo.Length + " " + fileInfo.LastWriteTime + "\n";
                }
                catch (Exception exc)
                {
                    MessageBox.Show("Exception occured during view file operation for user: " + username + exc.Message);
                    MessageBox.Show(directoryPath + "\\" + filename);
                }
                
            }
            //Convert the list into a byte array, get the size of it
            UTF8Encoding encoder = new UTF8Encoding();
            byte[] listInBytes = encoder.GetBytes(list);
            byte[] result = new byte[1024];
            //Send the list to the client
            printLogger("Server is sending the requested list view...");
            socket.Send(listInBytes);
            printLogger("List view is sent succesfully.");
        }

        /*
         * This function deletes the file of the given user
         * It notifies the client about the completion of the operation
         */
        private void deleteFile(Object socketObj, string username, string filename)
        {
            Socket socket = (Socket)socketObj;
            string directoryPath = serverPath.Text + "\\" + username;
            string filePath = directoryPath + "\\" + filename;

            try
            {
                if (File.Exists(filePath))
                {           
                    File.Delete(filePath);
                    sendResultToClient(socket, 0);
                }else
                { 
                    //File deletion is successful
                    sendResultToClient(socket, 1);
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show("Exception occured during delete file operation for user: " + username + exc.Message);
                sendResultToClient(socket, 1);
            }
        }

        /*
         * This function changes the name of the file with the given filename
         * to newFilename. It notifies the client about the result of the operation
         */
        private void renameFile(Object socketObj, string username, string filename, string newFilename)
        {
            Socket socket = (Socket)socketObj;
            string filePath = "C:\\Users\\eylulyurdakul\\Desktop" + "\\" + username + "\\" + filename;
            string newFilePath = "C:\\Users\\eylulyurdakul\\Desktop" + "\\" + username + "\\" + newFilename;

            try
            {
                if (File.Exists(filePath))
                {
                    File.Move(filePath, newFilePath);
                    //File rename is successful
                    sendResultToClient(socket, 0);
                }
                else
                {
                    //File not exist notify client
                    sendResultToClient(socket, 1);
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show("Exception occured during rename file operation for user: " + username + exc.Message);
                sendResultToClient(socket, 1);
                return;
            }  
        }

        /*
         * This function sends the file with the given filename to the client
         * on the end of the socketObj by first sending the filesize, then transfering
         * the data.
         */
        private void dataTransferToClient(Object socketObj, string username, string filename)
        {
            Socket socket = (Socket)socketObj;
            string directoryPath = serverPath.Text + "\\" + username;
            string filePath = directoryPath + "\\" + filename;
            printLogger("Server is sending the file " + filename + " for user: " + username);

            //Sending file size beforehand
            long filesize = new FileInfo(filePath).Length;
            byte[] filesizeInBytes = BitConverter.GetBytes(filesize);
            socket.Send(filesizeInBytes);
           
            //Sending the file
            try
            {
                socket.BeginSendFile(filePath, new AsyncCallback(FileSendCallback), socket);
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
            Socket clientSocket = (Socket)ar.AsyncState;
            try
            {
                clientSocket.EndSendFile(ar);
                // Complete sending the data to the remote device.
                printLogger("File transfer complete");
            }
            catch (Exception exc)
            {
                MessageBox.Show("Socket exception occured.");
                clientSocket.Shutdown(SocketShutdown.Both);
                clientSocket.Close();
            }
        }

        /*
         * This function takes a socket object as a parameter. 
         * First, it recieves the handshake information from the 
         * client, which contains username size,username, filename size,
         * filename, filesize. Then, it verifies that this is a unique client
         * by checking it on the namelist. If it verifies, it initiates data transfer
         * over TCP sockets.
         */
        private bool transferData(Object socketObj, string username)
        {
            Socket socket = (Socket)socketObj;

            byte[] fileInfo = new byte[128];
            try
            {
                int recievedData = socket.Receive(fileInfo);
                //If 0 bytes recieved socket connection is lost
                if (recievedData == 0)
                {
                    MessageBox.Show("Client disconnected");
                    termianteUserConnection(socket, username);
                    return false;
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show("Socket exception occured: " + exc.Message);
                return false;
            }

            UTF8Encoding encoder = new UTF8Encoding();
            //Parsing filename and fileSize
            int filenameSize = BitConverter.ToInt32(fileInfo.Take(4).ToArray(), 0);
            string filename = encoder.GetString(fileInfo.Skip(4).Take(filenameSize).ToArray());
            long fileSize = BitConverter.ToInt64(fileInfo.Skip(4 + filenameSize).Take(sizeof(long)).ToArray(), 0);

            printLogger("File transfer started for user: " + username + " \n" + "Filesize: " + fileSize);

            /*
             * Recieves the file in chunks of 2KB, it continues to recieve until 
             * the file transfer is completed.
             */
            byte[] data = new byte[8 * 1024];
            FileStream stream = File.Create(serverPath.Text + "\\" + username + "\\" + filename);
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
                File.Delete(serverPath.Text + "\\" + username + "\\" + filename);
                printLogger("Corrupted filed is being deleted ");
                termianteUserConnection(socket, username);
                return false;
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
                termianteUserConnection(socket, username);
                return false;
            }
            printLogger("File transfer finished for user:" + username);
            return true;
        }


        /*
         *  This method terminates the sockets 
         */
        private void terminateSocket(Socket sck)
        {
            sck.Shutdown(SocketShutdown.Both);
            sck.Close();
        }

        private void Button_Start_Click(object sender, EventArgs e)
        {
            initalizeListening();
            Button_Start.Enabled = false;
            //Button_Stop.Enabled = true;
        }

        private void serverForm_Load(object sender, EventArgs e)
        {
            TextBox.CheckForIllegalCrossThreadCalls = false;
            Button_Stop.Enabled = false;

            //Creating a socket to listen the incoming requests
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
        }

        private void Button_Stop_Click(object sender, EventArgs e)
        {
            terminateSocket(socket);
            isStoped = false;
            Button_Stop.Enabled = false;
            Button_Start.Enabled = true;
        }

        /*
         * This method iterates over the user list to find out 
         * whether the username already exists on the list
         */
        private bool checkUserList(string nameToCheck)
        {
            foreach (string clientName in connectedUsers)
            {
                if (clientName.Equals(nameToCheck))
                {
                    // User name exists in the list return true
                    return true;
                }
            }
            // Iterated over all of the list, username is not in the list
            return false;
        }

        private void removeFromUserList(string username)
        {
            connectedUsers.Remove(username);
        }

        private void termianteUserConnection(Socket socket, string username)
        {
            sendResultToClient(socket, 1);
            terminateSocket(socket);
            removeFromUserList(username);
        }

        private void sendResultToClient(Socket socket, Int32 result)
        {
            socket.Send(BitConverter.GetBytes(result));
        }

        private void serverBrowse_Click(object sender, EventArgs e)
        {

            string folderPath = "";
            FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                folderPath = folderBrowserDialog1.SelectedPath;
                serverPath.Text = folderPath;
                printLogger("You chose the path " + folderPath + " to upload items.");
            }
        }
    }
}