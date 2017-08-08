using System;
using System.Threading;
using System.Threading.Tasks;
using Alladin.Models;

namespace Alladin.Bots
{
    /*
     * Example from .NET sample
     */
    class RandomBot
    {
        private ServerStuff serverStuff;

        public RandomBot(ServerStuff serverStuff)
        {
            this.serverStuff = serverStuff;
        }

        //starts everything
        public async Task run()
        {
            Console.Out.WriteLine("random bot running");

            await serverStuff.CreateGame();

            if (serverStuff.errored == false)
            {
                Console.WriteLine(serverStuff.viewURL);
            }

            Random random = new Random();
            while (serverStuff.finished == false && serverStuff.errored == false)
            {
                var dir = string.Empty;
                switch (random.Next(0, 6))
                {
                    case 0:
                        dir = Direction.East;
                        break;
                    case 1:
                        dir = Direction.North;
                        break;
                    case 2:
                        dir = Direction.South;
                        break;
                    case 3:
                        dir = Direction.Stay;
                        break;
                    case 4:
                        dir = Direction.West;
                        break;
                }
                await serverStuff.moveHero(dir);

                Console.Out.WriteLine("completed turn " + serverStuff.currentTurn);
            }

            if (serverStuff.errored)
            {
                Console.Out.WriteLine("error: " + serverStuff.errorText);
            }

            Console.Out.WriteLine("random bot finished");
        }
    }
}