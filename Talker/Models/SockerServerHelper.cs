using System;
using System.Text;
using System.Net.Sockets;

namespace Talker.Models
{

    /// <summary>
    /// 
    /// Stores the methods responsible for connecting the program to socket server in program
    /// 
    /// </summary>
    public class SockerServerHelper
    {
        /// <summary>
        /// 
        /// Internet address of tcp heardbeart sever
        /// 
        /// </summary>
        private const string SocketServerIpAddress = "talkerapi-env.eba-banadszz.eu-west-1.elasticbeanstalk.com";

        /// <summary>
        /// 
        /// Port number of tcp heardbeart sever
        /// 
        /// </summary>
        private const int SocketServerPort = 6789;

        /// <summary>
        /// 
        /// Name of the user whose information about we want to get
        /// 
        /// </summary>
        public int UserId;

        /// <summary>
        /// 
        /// Instance of System Net cliecnt to allow the porgram to estabish tcp connections
        /// 
        /// </summary>
        public TcpClient Client;

        /// <summary>
        /// 
        /// The constructor initializes the variables required to send tcp requerst.
        /// 
        /// </summary>
        /// <param name="userId">Specify the user to whom the request applies</param>
        public SockerServerHelper(int userId)
        {
            UserId = userId;
            Client = null;
        }

        /// <summary>
        /// 
        /// Method establishes connection between client and tcp heartbeat server
        /// 
        /// </summary>
        public bool EstablishConnection()
        {

            if (Client != null && Client.Connected) return true;
            Client = new TcpClient();

            var result = Client.BeginConnect(SocketServerIpAddress, SocketServerPort, null, null);

            var success = result.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(1));

            if (!success)
            {
                throw new Exception("Failed to connect.");
            }

            return Client.Connected; 
        }


        /// <summary>
        /// Sends request to tcp echo server to get informtions related with user using tcp connection
        /// - such as new messages or changes of friends status.
        /// These messages don’t  need to be encrypted because they don't contain any crucial data 
        /// An error will be thrown after one second withou response
        /// </summary>
        public string CheckNewChanges()
        {
            var textToSend = $"status;{UserId};{DateTime.Now.ToFileTime()};\r\n";

            NetworkStream nwStream = Client.GetStream();
            byte[] bytesToSend = ASCIIEncoding.UTF8.GetBytes(textToSend);

            //---send the text to the tcp server ---
            nwStream.WriteTimeout = 1000;
            nwStream.Write(bytesToSend, 0, bytesToSend.Length);

            //---read back the text---
            byte[] bytesToRead = new byte[Client.ReceiveBufferSize];
            int bytesRead = nwStream.Read(bytesToRead, 0, Client.ReceiveBufferSize);

            return Encoding.ASCII.GetString(bytesToRead, 0, bytesRead);
        }


        /// <summary>
        /// Disconects from tcp heartbeat server. 
        /// </summary>
        public void Disconnect()
        {
            Client.Close();
            Client = null;
        }


    }
}