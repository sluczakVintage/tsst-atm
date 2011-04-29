﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace NetworkNode
{
    class CNetworkPortIn : CNetworkPort
    {


        private bool status;
        private IPAddress ip = IPAddress.Parse("127.0.0.1");     //adres serwera
        private TcpListener portListener;
        private TcpClient client;
        private NetworkStream clientStream;
        private StreamWriter serwerStream;
        private static String helloMessage = "Welcome to port : " ;
        private int portNumber;

        public CNetworkPortIn(int id, Boolean busy, int systemPortNumber)
            : base(id, busy)
        {
            this.portNumber = systemPortNumber;
            Console.WriteLine("Port sieciowy o id = " + id + " będzie nasłuchiwał na porcie systemowym = " + portNumber);
        }



        public void init() //metoda uruchamiająca nasłuchiwanie na porcie. 
        {
            status = true;
            portListener = new TcpListener(ip, portNumber);  //tworzymy obiekt  nasłuchujący na podanym porcie
            portListener.Start();                      //uruchamiamy serwer

            client = portListener.AcceptTcpClient(); //akceptujemy żądanie połączenia
            clientStream = client.GetStream();  //pobieramy strumień do wymiany danych
            Console.WriteLine("connection accepted ");
            while (status) //uruchamiamy nasłuchiwanie
            {
                StreamReader sr = new StreamReader(clientStream);
                String dane = sr.ReadLine();
                Console.WriteLine(dane);
            }

        }

        public void shutdown()
        {
            status = false;
            client.Close();
            portListener.Stop();
        }

        public void receiveData(Data.CCharacteristicData data) //metoda odbierajace dane 
        {
         //   CCommutationTable.Instance.passOnData(data, this);
        }
    
    } 
}
