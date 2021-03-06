﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ManagementLayer
{ // klasa reprezentująca konsole 
    sealed class CMLConsole
    {
        private String ConsoleInput;
        private String helloMessage = "****************************************************************************** \n*\n* Hello in Network Manager. To exit type 'q' \n*\n*********************************************************************** ";
        private String exitMessage = "Exiting...";
        private String commandPrompt = "Network Manager$";
        private readonly static CMLConsole instance = new CMLConsole();
        private readonly Dictionary<string, string> cmdList = new Dictionary<string, string>() {
        
        {"show netCfg", "Wyświetla konfigurację połaczeń w sieci"},
        {"show nodes","Wyświetla węzły wraz z ich rolą w sieci"},
        {"getConnections [args]","Pobiera tablicę komutacji z węzła i ja wyświetla"},
        {"addConnections [args]","Dodaje połączenie w tablicy komutacji"},
        {"?","Wyswietla listę poleceń dostępnych w konsoli"}
        
        };

        private CMLConsole()
        {
        }

        public static CMLConsole Instance
        {
            get
            {
                return instance;
            }
        }

        public void consoleInit()
        {
            //CPortManager cpm = CPortManager.Instance;
            Console.WriteLine(helloMessage);
            CNetworkConfiguration cnc = CNetworkConfiguration.Instance;
            ConnectionsManager cm = ConnectionsManager.Instance;



            cnc.readConfig();
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
                else if(ConsoleInput.Equals("?")) 
                {
                    // pokazuje listę dostępnych poleceń
                    Console.WriteLine("***\t Dostępne polecenia: \t***");
                    foreach (KeyValuePair<String, String> pair in cmdList)
                    {
                        
                        Console.WriteLine("{0} \t {1}",
                        pair.Key,
                        pair.Value);
                    }

                }
                else if(ConsoleInput.StartsWith("show")) 
                {
                    String[] alt = ConsoleInput.Split(new char[] {' '});
                    if (alt.Count() > 1)
                    {
                        if (alt[1].Equals("netCfg"))
                        {
                            // pokazuje konfigurację połączeń w sieci
                            cnc.showNetworkConfiguration();
                        }
                        else if (alt[1].Equals("nodes"))
                        {
                            //pokazuje listę nodów które zgłosiły sie do ML
                            cnc.showNodes();
                        }
                        else
                        {
                            Console.WriteLine("ERROR : Błędny argument");
                            continue;
                        }
                    }
                    else
                    {
                        Console.WriteLine("ERROR : Brak podanego argumentu...");
                        continue;
                    }

                }
                else if (ConsoleInput.StartsWith("addconnection")) 
                {
                    String[] alt = ConsoleInput.Split(new char[] { ' ' });
                    List<int> args = new List<int>();
                    if (alt.Count() > 1)
                    {
                        if (alt.Count() == 8)
                        {
                            try
                            {
                                for (int i = 1; i < alt.Count(); i++)
                                {
                                    args.Add(Convert.ToInt32(alt[i]));
                                }
                                // node number, port in port out
                                cm.addConnection(args[0], args[1], args[2], args[3], args[4], args[5], args[6]);
                                //CShortestPathCalculatorWrapper.Instance.getShortestPath(args[0], args[1]);

                                }
                            catch (Exception e)
                            {
                                Console.WriteLine("ERROR : Błędny argument " + e.Message);
                            }
                        }
                        else { Console.WriteLine("ERROR : Błęda liczba argumentów"); }
                    }
                    else { Console.WriteLine("ERROR : Błęda liczba argumentów"); }
                    
                }

                else if (ConsoleInput.StartsWith("removeconnection"))
                {
                    String[] alt = ConsoleInput.Split(new char[] { ' ' });
                    List<int> args = new List<int>();
                    if (alt.Count() > 1)
                    {
                        if (alt.Count() == 8)
                        {
                            try
                            {
                                for (int i = 1; i < alt.Count(); i++)
                                {
                                    args.Add(Convert.ToInt32(alt[i]));
                                }
                                // node number, port in port out
                                cm.removeConnection(args[0], args[1], args[2], args[3], args[4], args[5], args[6]);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("ERROR : Błędny argument " + e.Message);
                            }
                        }
                        else { Console.WriteLine("ERROR : Błęda liczba argumentów"); }
                    }
                    else { Console.WriteLine("ERROR : Błęda liczba argumentów"); }

                }
                else if (ConsoleInput.StartsWith("getConnections"))
                {
                    String[] alt = ConsoleInput.Split(new char[] { ' ' });
                    List<int> args = new List<int>();
                    if (alt.Count() > 1)
                    {
                        try
                            {
                                for (int i = 1; i < alt.Count(); i++)
                                {
                                    args.Add(Convert.ToInt16(alt[i]));
                                }
                            }
                        catch (Exception e) {
                            Console.WriteLine("ERROR : Błędny argument " + e.Message);
                            }
                        // usuwam powtarzające sie argumenty
                        int[] var = args.Distinct().ToArray();

                        // dla każdego argumentu sprawdza warunek i wysyła request do noda
                        foreach (int a in var)
                        {
                            if (cnc.checkFormula(a))
                            {
                                Console.WriteLine("<-- Pobierasz dane z noda: " + args[0]);
                                cm.getNodeCommutationTable(args[0]);
                            }
                        }
                                
                    }
                    else { Console.WriteLine("ERROR : Nie podałeś numeru węzła"); continue; }
                }

             else Console.WriteLine(ConsoleInput);

            }
        }

        

    }
}
