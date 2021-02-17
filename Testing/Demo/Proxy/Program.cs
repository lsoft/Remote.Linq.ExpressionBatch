using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Proxy
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Proxy is running");
            var listener = new TcpListener(new IPAddress(new byte[]{ 127, 0, 0, 1 }), 10050);
            listener.Start();
            var client = listener.AcceptTcpClient();
            Console.WriteLine("+++++ Client connected +++++");
            var buffer = new byte[1024];
            using (var server = new TcpClient())
            {
                server.Connect("localhost", 10040);
                using (var clientStream = client.GetStream())
                using (var serverStream = server.GetStream())
                {
                    while (client.Connected)
                    {
                        var haveData = false;
                        while (client.Available > 0)
                        {
                            if (!haveData)
                            {
                                Console.WriteLine();
                                Console.WriteLine("+++++ Request Client => Server +++++");
                                haveData = true;
                            }
                            var len = clientStream.Read(buffer, 0, 1024);
                            if (len > 0)
                            {
                                Console.WriteLine();
                                Console.WriteLine(" ASCII:");
                                for (int i = 0; i < len; i++)
                                {
                                    var b = buffer[i];
                                    var c = Encoding.ASCII.GetString(new byte[] { b })[0];
                                    if (b > 127 || (b < 32 && c != '\r' && c != '\n'))
                                    {
                                        Console.Write("?");
                                    }
                                    else
                                    {
                                        Console.Write(c);
                                    }
                                }
                                Console.WriteLine();
                                Console.WriteLine(" HEX:");
                                for (int i = 0; i < len; i++)
                                {
                                    Console.Write(buffer[i].ToString("X2"));
                                }
                                serverStream.Write(buffer, 0, len);
                            }
                        }
                        if (haveData)
                        {
                            serverStream.Flush();
                            Console.WriteLine();
                        }
                        haveData = false;
                        while (server.Available > 0)
                        {
                            if (!haveData)
                            {
                                Console.WriteLine();
                                Console.WriteLine("+++++ Response Server => Client +++++");
                                haveData = true;
                            }
                            var len = serverStream.Read(buffer, 0, 1024);
                            if (len > 0)
                            {
                                Console.WriteLine();
                                Console.WriteLine(" ASCII:");
                                for (int i = 0; i < len; i++)
                                {
                                    var b = buffer[i];
                                    var c = Encoding.ASCII.GetString(new byte[] { b })[0];
                                    if (b > 127 || (b < 32 && c != '\r' && c != '\n'))
                                    {
                                        Console.Write("?");
                                    }
                                    else
                                    {
                                        Console.Write(c);
                                    }
                                }
                                Console.WriteLine();
                                Console.WriteLine(" HEX:");
                                for (int i = 0; i < len; i++)
                                {
                                    Console.Write(buffer[i].ToString("X2"));
                                }
                                clientStream.Write(buffer, 0, len);
                            }
                        }
                        if (haveData)
                        {
                            clientStream.Flush();
                            Console.WriteLine();
                        }
                        Thread.Sleep(100);
                    }
                }
            }
        }
    }
}
