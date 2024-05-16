using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using Newtonsoft.Json;
using System.Runtime.InteropServices;
using System.Net;
using System.ComponentModel;


public class UrStateReceiver
{
    private readonly string HOST;
    private readonly int PORT;
    private readonly List<State> Output = new List<State>();

    public UrStateReceiver(string host, int port)
    {
        HOST = host;
        PORT = port;

        string jsonText = "";
        try 
        {
            jsonText = File.ReadAllText("states.json"); 
        }
        catch (Exception e) 
        {
            Console.WriteLine(e.Message);
        }

        var states = JsonConvert.DeserializeObject<dynamic>(jsonText);

        foreach (var state in states["states"])
        {
            Output.Add(new State(
                state["description"].ToString(),
                (int)state["length"],
                (bool)state["radian"],
                (int)state["start"],
                state["valuetype"].ToString(),
                (bool)state["visible"]
            ));
        }
    }

    public List<State> PollDataFromSocket()
    {
        List<State> output_return = new List<State>();
        Socket s = new Socket((new IPEndPoint(IPAddress.Parse(HOST), PORT)).AddressFamily,
            SocketType.Stream, ProtocolType.Tcp);

        s.Connect(HOST, PORT);
        
        byte[] receivedData = new byte[2048];
        
        s.Receive(receivedData);

        foreach (var state in Output)
        {
            var byteData = new byte[state.Length];
            Array.Copy(receivedData, state.Start, byteData, 0, state.Length);

            byte[] reversed = byteData.Reverse().ToArray();

            if (state.ValueType == typeof(int))
            {
                state.Value = BitConverter.ToInt32(reversed, 0);
            }
            else if (state.ValueType == typeof(double))
            {
                state.Value = BitConverter.ToDouble(reversed, 0);
            }
            else
            {
                throw new ArgumentException("Unsupported value type: {0}", state.ValueType.ToString());
            }
            output_return.Add(state);
        }
        return output_return;
    }
}

