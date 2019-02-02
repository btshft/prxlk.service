using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Serilog;

namespace Prxlk.Gateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                WebHost.CreateDefaultBuilder(args)
                    .UseStartup<Startup>()
                    .UseSerilog()
                    .Build()
                    .Run();
            }
            catch (Exception e)
            {
                Log.Fatal(e, "Host terminated");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}