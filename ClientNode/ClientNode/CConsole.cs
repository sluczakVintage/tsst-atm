﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
namespace ClientNode
{ // klasa reprezentująca konsole 
    sealed class CConsole 
    {
        private String ConsoleInput;
        private String helloMessage = "****************************************************************************** \n*\n*" 
                                       + " Hello in ClientNode. \n* ID = "+CConstrains.nodeNumber+ " To exit type 'q' " 
                                       +"\n*\n****************************************************************************** ";
        private String exitMessage = "Exiting...";
        private String commandPrompt = "ClientNode$";
        private readonly static  CConsole instance = new CConsole();

        private CConsole()
        {
        }

        public static CConsole Instance
        {
            get
            {
                return instance;
            }
        }

        public void consoleInit()
        {
            CPortManager cpm = CPortManager.Instance;
            CManagementAgent cma = CManagementAgent.Instance;

            foreach (Thread t in CConstrains.threadList)
            {
                t.Start(); 
            }

                Console.Title = "ClientNode ID = " + CConstrains.nodeNumber;
                Console.WriteLine("Wszystkie porty wstały");
                Console.WriteLine(helloMessage);
                while (true)
                {

                    Console.Write(commandPrompt + " ");
                    ConsoleInput = Console.ReadLine();
                    if (ConsoleInput.Equals("q"))
                    {
                        Console.WriteLine(exitMessage);
                        //cpm.shutdownAllPorts();
                        Environment.Exit(1);
                    }
                    else if (ConsoleInput.StartsWith("start"))
                    {
                        // wywołanie metod związanych z nadawaniem 

                        Data.CUserData data = new Data.CUserData();
                        List<byte> temp = new List<byte>();
                        System.Random x = new Random(System.DateTime.Now.Millisecond);
                        for (int i = 0; i < 48; i++)
                        {
                            temp.Add((byte)x.Next(0, 127));        // dodawanie kolejnych bajtow do danych do wyslania
                        }
                        data.setInformation(temp);
                        cpm.sendMsg(data);
                        //cpm.showPorts();
                    }
                    else if (ConsoleInput.StartsWith("stop"))
                    {

                        String[] command = ConsoleInput.Split(' ');
                        try
                        {
                            // wywołanie metod związanych z zaprzestaniem nadawnia
                            int arg = Convert.ToInt32(command[1]);
                            cpm.stopSending(arg);

                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("argument nie jest liczbą");
                            Console.WriteLine(e.StackTrace);
                        }
                    }
                    else if (ConsoleInput.StartsWith("show"))
                    {

                        cpm.getNodePortConfiguration();
                    }
                    else Console.WriteLine(ConsoleInput);

                }
            
        }


    }
}
