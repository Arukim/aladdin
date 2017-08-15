using System;
using System.Collections.Generic;
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

        private readonly IGameDataProvider _gameDataProvider;

        public Coach()
        {
            _gameDataProvider = Container.GetService<IGameDataProvider>();
        }

        public void Run(IEnumerable<Tuple<AccountEntity, GenomeEntity>> trainees, int rounds)
        {
            int played = 0;

            while (played < rounds)
            {
                Console.WriteLine("Starting game session");
                var tasks = new List<Task>();
                foreach (var trainee in trainees)
                {
                    tasks.Add(RunSession(trainee.Item1, trainee.Item2));
                }

                Task.WaitAll(tasks.ToArray());
                Console.WriteLine("Game sesion ended");
                played++;
            }
        }

        protected async Task RunSession(AccountEntity acc, GenomeEntity genome)
        {
            try
            {
                // Connect to server
                var server = new ServerStuff(acc.Token, false, _turns, _serverUrl, null);
                // Play game
                var player = new Player(acc.Name, server);

                await player.Run();

                if (server.finished)
                {
                    var result = new GameEntity{
                        AccoundId = acc.Id,
                        AccountName = acc.Name,
                        GameId = server.Id,
                        GenomeId = genome?.Id,
                        IsWinner = server.heroes.OrderBy(x => x.Gold).First().Name == acc.Name
                    };
                    
                    await _gameDataProvider.Add(result);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unhandled exception " + ex);
            }
        }
    }
}