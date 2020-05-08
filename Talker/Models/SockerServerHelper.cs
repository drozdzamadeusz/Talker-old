using System;
using System.IO;
using System.Net;
using System.Text;
using System.Net.Sockets;
using System.Threading;

namespace Talker.Models
{
    public class SockerServerHelper
    {

        const string socketServerIpAddress = "talkerapi-env.eba-banadszz.eu-west-1.elasticbeanstalk.com";
        const int socketServerPort = 6789;


        public int UserId;

        //public SimpleTcpClient Client;

        public TcpClient Client;

        public SockerServerHelper(int userId)
        {
            UserId = userId;
            Client = null;
        }


        public bool EstablishConnection()
        {
            /*if (Client != null && Client.TcpClient.Connected) return true;

            Client = new SimpleTcpClient().Connect(socketServerIpAddress, socketServerPort);

            Client.StringEncoder = System.Text.ASCIIEncoding.UTF8;

            return Client.TcpClient.Connected;*/




            /*if (Client != null && Client.Connected) return true;

            Client = new TcpClient(socketServerIpAddress, socketServerPort);*/


            if (Client != null && Client.Connected) return true;


            Client = new TcpClient();


            var result = Client.BeginConnect(socketServerIpAddress, socketServerPort, null, null);

            var success = result.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(1));

            if (!success)
            {
                throw new Exception("Failed to connect.");
            }


            return Client.Connected; 
        }


        public string CheckNewChanges()
        {
            var textToSend = $"status;{UserId};{DateTime.Now.ToFileTime()};\r\n";

            NetworkStream nwStream = Client.GetStream();
            byte[] bytesToSend = ASCIIEncoding.UTF8.GetBytes(textToSend);

            //---send the text---
            //Console.WriteLine("Sending : " + textToSend);

            nwStream.WriteTimeout = 1000;
            nwStream.Write(bytesToSend, 0, bytesToSend.Length);


            //---read back the text---
            byte[] bytesToRead = new byte[Client.ReceiveBufferSize];
            int bytesRead = nwStream.Read(bytesToRead, 0, Client.ReceiveBufferSize);
            //Console.WriteLine("Received : " + Encoding.UTF8.GetString(bytesToRead, 0, bytesRead));

            return Encoding.ASCII.GetString(bytesToRead, 0, bytesRead);
        }



        public void Disconnect()
        {
            Client.Close();
            Client = null;
        }


    }
}