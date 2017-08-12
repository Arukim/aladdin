using System;
using System.Linq;
using System.Threading.Tasks;
using Aladdin.Common;
using Aladdin.Common.Interfaces;
using Aladdin.DAL.Interfaces;
using Aladdin.DAL.Models;
using Aladdin.Game;

namespace Aladdin.Core
{
    public class Coach : ICoach
    {
        private const string _serverUrl = "http://vindinium.org";
        private const int _turns = 400;

        private readonly IAccountDataProvider _accDataProvider;

        public Coach()
        {
            _accDataProvider = Container.GetService<IAccountDataProvider>();
        }

        public void Run(bool isTraining)
        {
            while (true)
            {
                Console.WriteLine("Starting game session");
                var accs = _accDataProvider.GetAll().ToList();

                Parallel.ForEach(accs, acc => RunSession(acc));
                Console.WriteLine("Game sesion ended");
                return;
            }
        }

        protected void RunSession(AccountEntity acc)
        {
            var gamesCount = 0;
            while (gamesCount < 1)
            {
                try
                {
                    // Connect to server
                    var server = new ServerStuff(acc.Token, false, _turns, _serverUrl, null);
                    // Play game
                    var player = new Player(acc.Name, server);

                    var result = player.Run().Result;
                    // Save results
                    gamesCount++;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Unhandled exception " + ex);
                }
            }
        }
    }
}