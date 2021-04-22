using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DnsClient;

namespace Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Starting...");
            var dbContext = new DbContext();
            var names = new string[]
            {
                "Bob",
                "Mark",
                "Sarah",
            };
            var random = new Random();
            var work = new List<Task>();

            work.Add(Task.Factory.StartNew(async () =>
            {
                while (true)
                {
                    Console.Clear();
                    Console.WriteLine($"Server Status:");
                    var counts = dbContext.GetCounts();
                    foreach(var s in dbContext.ClientContext.Cluster.Description.Servers)
                    {
                        (string Host, int Count) = counts.FirstOrDefault(c => s.EndPoint.ToString() == c.Host);
                        if (Host != null)
                        {
                            Console.WriteLine($"    {s.EndPoint} ({s.Type}) ({Count}) {s.State}");
                        }
                        else
                        {
                            Console.WriteLine($"    {s.EndPoint} ({s.Type}) (??) {s.State}");
                        }
                    }
                    Console.WriteLine("\nPress ctrl+c to exit...");
                    Thread.Sleep(100);
                }
            }));


            // Read
            work.Add(Task.Factory.StartNew(async () =>
            {
                while (true)
                {
                    try
                    {
                        await dbContext.FindPerson(names[random.Next(0,2)]);
                        Thread.Sleep(100);
                    }
                    catch (Exception ex)
                    {
                        continue;
                    }

                }
            }));

            // DB Admin
            work.Add(Task.Factory.StartNew(async () =>
            {
                while (true)
                {
                    try
                    {
                        await dbContext.ListDatabases();
                        Thread.Sleep(100);
                    }
                    catch (Exception ex)
                    {
                        continue;
                    }

                }
            }));

            // Write
            work.Add(Task.Factory.StartNew(async () =>
            {
                while (true)
                {
                    try
                    {
                        await dbContext.CreatePerson(names[random.Next(0, 2)], random.Next(5,55));
                        Thread.Sleep(150);
                    }
                    catch (Exception ex)
                    {
                        continue;
                    }

                }
            }));
            await Task.WhenAll(work);
            Console.ReadLine();
        }
    }
}
