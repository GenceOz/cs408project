
/*
 * Author: Gence Özer 
 * Date: 11/19/2016
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

namespace server
{
    public partial class serverForm : Form
    {
        Socket socket;
        EndPoint epLocal;
        List<string> connectedUsers = new List<String>();
        Thread dispatcherThread;

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
            RTextBox_Logs.AppendText(now + "--> "+ message + "\n");
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
         * This function retrieves path and port information from GUI
         * First, it creates a directory, if not exists, 
         * then it binds a socket and puts it in a listening state to
         * given port info.
         */
        private void initalizeListening()
        {
            string path = TextBox_Path.Text;

            //Create a folder in the given path
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                    printLogger("Creating the directory in the path: " + path);
                }
                else
                {
                    printLogger("Directory alread exist in the path: " + path);
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine("Exception : " + exc.ToString());
                MessageBox.Show("Exception occured on directory creation.");
                return;
            }

            //Binding the socket to ip and given port name, puts the socket in listening state
            int portNumber =  (int)Numeric_Port.Value;
            epLocal = new IPEndPoint(IPAddress.Parse(getLocalIP()), portNumber);
            socket.Bind(epLocal);
            socket.Listen(portNumber);

            /* Sets up the thread which will perform the handshake connection
             * with the client 
             */
            try
            {
                dispatcherThread = new Thread(new ThreadStart(dispatchFileTransferOperations));
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
         * 
         */
        private void dispatchFileTransferOperations() 
        {
            while (true)
            {
                Socket dataTransferSocket = socket.Accept();
                Thread clientConnectionThread = new Thread(new ParameterizedThreadStart(transferData));
                clientConnectionThread.Start(dataTransferSocket);
            }
        }

        private void transferData(Object socketObj)
        {
            Socket socket = (Socket)socketObj;
            
            //Recieves the username, filename and filesize to establish the connection
            byte[] handshakeInfo = new byte[128];
            socket.Receive(handshakeInfo);
         
            ASCIIEncoding encoder = new ASCIIEncoding();
            //Parsing username 
            int usernameSize = BitConverter.ToInt32(handshakeInfo.Take(4).ToArray(),0);
            string username = encoder.GetString(handshakeInfo.Skip(4).Take(usernameSize).ToArray());

            printLogger("An incoming connection from user: " + username);
            
            if (checkUserList(username))
            {
                //Username already has a active connection, terminate
                printLogger("user " + username + "already has an active connection. Terminating connection.");
            }
            else
            {
                //Add the username in the list
                connectedUsers.Add(username);

                //Parsing filename and fileSize
                int filenameSize = BitConverter.ToInt32(handshakeInfo.Skip(4 + usernameSize).Take(4).ToArray(), 0);
                string filename = encoder.GetString(handshakeInfo.Skip(8 + usernameSize).Take(filenameSize).ToArray());
                int fileSize = BitConverter.ToInt32(handshakeInfo.Skip(4 + usernameSize + 4 + filenameSize).Take(4).ToArray(), 0);
                printLogger("Recieving file");
                
                byte[] data = new byte[2048];
                printLogger(fileSize.ToString());
                try
                {
                    var stream = File.Create(TextBox_Path.Text + "\\" + "Simple.txt");

                    int bytesLeftToTransfer = fileSize;    
                    while (bytesLeftToTransfer > 0)
                    {
                        int noOfBytes = socket.Receive(data);
                        int bytesToCopy = Math.Min(noOfBytes, bytesLeftToTransfer);
                        stream.Write(data, 0, bytesToCopy);

                        bytesLeftToTransfer -= bytesToCopy;
                        printLogger("RecievedBytes " + noOfBytes + " " + bytesLeftToTransfer);
                    }
                    stream.Close();
                    socket.Close();
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.Message);
                }

                try
                {
                    printLogger("Writing file " + filenameSize +" "+ filename + " " + username );
                    BinaryWriter bWrite = new BinaryWriter(File.Open(TextBox_Path.Text + "\\" + "Gence_Özer_CV.pdf", FileMode.Append));
                    bWrite.Write(data, 0, fileSize);
                    bWrite.Close();
                }
                catch (Exception exc) {
                    MessageBox.Show(exc.Message);
                }
            }
        }

        /*
         *  This method destroys the sockets
         */
        private void terminateSocket()
        {
            //TODO: Not sure about how to terminate socket connection
            socket.Close();
        }

        private void Button_Start_Click(object sender, EventArgs e)
        {
            initalizeListening();
            Button_Start.Enabled = false;
            Button_Stop.Enabled = true;
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
            terminateSocket();
            Button_Stop.Enabled = false;
            Button_Start.Enabled = true;
        }

        private bool checkUserList(string nameToCheck)
        {
            foreach(string clientName in connectedUsers)
            {
                if (clientName.Equals(nameToCheck)) 
                {
                    // User name exists in the list
                    return true;
                }
            }

            // Iterated over all of the list, username is not in the list
            return false;
        }
        byte[] fileData;
        private void test_Click(object sender, EventArgs e)
        {
            Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            clientSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

            EndPoint ipLocal = new IPEndPoint(IPAddress.Parse(getLocalIP()), 8889);
            EndPoint ipEnd = new IPEndPoint(IPAddress.Parse(getLocalIP()), 8888);
            clientSocket.Bind(ipLocal);
            clientSocket.Connect(ipEnd);

            fileData = File.ReadAllBytes("D:\\Users\\SUUSER\\Desktop" + "\\Simple.txt");

            string username = "asdfghjklo";
            string filename = "Simple.txt";
            Int32 filesize = fileData.Length;

            ASCIIEncoding aEncoder = new ASCIIEncoding();
            byte[] usernameInBytes = aEncoder.GetBytes(username);
            byte[] filenameInBytes = aEncoder.GetBytes(filename);
            byte[] data = new byte[12 + username.Length + filename.Length];

            System.Buffer.BlockCopy(BitConverter.GetBytes(usernameInBytes.Length), 0, data, 0, 4);
            System.Buffer.BlockCopy(usernameInBytes, 0, data, 4, usernameInBytes.Length);
            System.Buffer.BlockCopy(BitConverter.GetBytes(filenameInBytes.Length), 0, data, 4 + usernameInBytes.Length, 4);
            System.Buffer.BlockCopy(filenameInBytes, 0, data, 8 + usernameInBytes.Length, filenameInBytes.Length);
            System.Buffer.BlockCopy(BitConverter.GetBytes(filesize), 0, data, 8 + usernameInBytes.Length + filenameInBytes.Length, 4);

            printLogger(data.Length.ToString());
            clientSocket.Send(data);
            int sentData = clientSocket.Send(fileData);
            printLogger("Sent bytes -> " + sentData.ToString());
        }
    }
}
