﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace  NetworkNode
{
    // Klasa portu wyjściowego dziedzicząca po CClientPort

    class CClientPortOut : CClientPort
    {
        private static TcpClient client;
        private NetworkStream stream;
        private StreamWriter clientStream;


        //konstruktor
        public CClientPortOut(int i, bool p)
            : base(i, p)
        {
            base.PORTTYPE = "OUT";
            base.PORTCLASS = "ClientPort";
        }

        public void init(int systemPortNumber) // metoda łącząca socket z portNumber
        {

            base.PORTNUMBER = systemPortNumber;
            try
            {
                client = new TcpClient();
                client.Connect(CConstrains.ipAddress, base.PORTNUMBER);
                stream = client.GetStream();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error.... " + e.StackTrace);
            }
        }

        public void shutdown()
        {
            client.Close();
        }

        // metoda nawiązująca połączenie z węzłem sieciowym i nadająca do niego TO DO
        public void send(String str)
        {
            Console.WriteLine("nadaje " + str);
            clientStream = new StreamWriter(stream);
            clientStream.WriteLine(str);
            clientStream.Flush();
        }
    }
}
