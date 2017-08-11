﻿using System;
using System.Linq;
using Aladdin.Config;
using Aladdin.DAL;
using Aladdin.DAL.Interfaces;
using Aladdin.DAL.Models;
using Alladin.Bots;

namespace Alladin
{
    class Program
    {
        static void Main(string[] args)
        {
            string serverURL = args.Length == 4 ? args[3] : "http://vindinium.org";

            var accDataProvider = Config.GetService<IAccountDataProvider>();
            
            Console.WriteLine(string.Join("",accDataProvider.GetAll().Select(x => $"{x.Name}:{x.Token}")));

            while (true)
            {
                //create the server stuff, when not in training mode, it doesnt matter
                //what you use as the number of turns
                ServerStuff serverStuff = new ServerStuff(args[0], args[1] != "arena", uint.Parse(args[2]), serverURL, null);

                //create the random bot, replace this with your own bot
                var bot = new Bot(serverStuff);

                //now kick it all off by running the bot.
                bot.Run().GetAwaiter().GetResult();
            }
            Console.Out.WriteLine("done");

            Console.Read();
        }
    }
}
