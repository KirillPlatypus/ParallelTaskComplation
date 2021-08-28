using System;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;

namespace ParallelTaskComplation
{
    static class Program
    {
        static async Task Main(string[] args)
        {
            var tokenSource = new CancellationTokenSource();
            tokenSource.CancelAfter(3000);
            var token = tokenSource.Token;

            var name = Console.ReadLine();

            string path = "test.txt";

            if(!File.Exists(path))
            {
                using(FileStream stream = File.Create(path))
                {
                    await stream.WriteStringAsync("hello ");
                }
            }
           
            using(FileStream stream = File.OpenWrite(path))
            {
                List <Task> task = new List<Task>();
                try
                {
                    
                    for (int i = 0; i < 10000; i++)
                    {
                        task.Add(stream.WriteStringAsync($"{i+1} hello {name}\n"));
                        //await Task.Run(async () => await stream.WriteStringAsync($"{i+1} hello \n"), token);
                    } 
                    Console.WriteLine(name);

                    await Task.WhenAll(task);
                }
                catch (Exception e)
                {                        
                    Console.WriteLine($"{e.Message}");
                }
            }
            
        }
    }

    static class StreamPlus
    {
        public static async Task WriteStringAsync(this FileStream stream, string value)
        {
            var buffer = new UTF8Encoding().GetBytes(value);
            await stream.WriteAsync(buffer);
            Console.WriteLine("value");
        }
    }

}
