using System;
using System.Threading;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Diagnostics;
using UR_State_Receiver;

class Program
{
    static string HOST = "192.168.204.128";
    static int PORT = 30003;

    static void Main(string[] args)
    {
        UrStateReceiver receiver = new UrStateReceiver(HOST, PORT);
        List<State>? received = null;
        List<string> linesToSave = new List<string>();
        string line;
        var time = $"{DateTime.Now.ToString("MMMM")}-{DateTime.Now.Day} {DateTime.Now.Hour}_{DateTime.Now.Minute}_{DateTime.Now.Second}";
        string fName = "robot_log_";
        while (true)
        {
            try
            {
                received = receiver.PollDataFromSocket();
            }
            catch (SocketException ex)
            {
                Console.WriteLine("[socket-server] {0}. Terminating the program...", ex.Message);
                Environment.Exit(1);
            }
            if (received == null) 
            {
                Console.WriteLine("Couldn't catch data from the Host. Check connection!");
            }

            Console.Clear();
            
            foreach (State state in received)
            {
                if (state.Visible)
                {
                    line = state.ToString();
                    Console.WriteLine(line);
                    linesToSave.Add(line);
                }
            }
            linesToSave.Add("\n ************************ \n");
            DataSaver.SaveToFile(fName+time+".txt", linesToSave);
            linesToSave.Clear();

            Console.WriteLine("\nPress Ctrl+C to quit...\n\n");

            Thread.Sleep(1000);
        }
    }
}
