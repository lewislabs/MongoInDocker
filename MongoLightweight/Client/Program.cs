using System;
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
            // Read
            await Task.Factory.StartNew(async () =>
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
                        Console.WriteLine("exception:", ex.Message.ToString());
                        continue;
                    }

                }
            });
            // DB Admin
            await Task.Factory.StartNew(async () =>
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
                        Console.WriteLine("exception:", ex.Message.ToString());
                        continue;
                    }

                }
            });

            // Write
            await Task.Factory.StartNew(async () =>
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
                        Console.WriteLine("exception:", ex.Message.ToString());
                        continue;
                    }

                }
            });
            Console.ReadLine();
        }
    }
}
