using System;
using System.Linq;
using Aladdin.Common;
using Aladdin.DAL;
using Aladdin.DAL.Interfaces;
using Aladdin.DAL.Models;
using Aladdin.Game;
using Aladdin.Core;
using Aladdin.Common.Interfaces;
using System.Collections.Generic;

namespace Aladdin
{
    class Program
    {
        static void Main(string[] args)
        {
            string serverURL = args.Length == 4 ? args[3] : "";

            var sp = Container.Create()
                    .RegisterDAL()
                    .RegisterCore();
            Container.Build(sp);

            Console.Out.WriteLine("starting server");

            var coach = Container.GetService<ICoach>();

            var accProvider = Container.GetService<IAccountDataProvider>();

            var trainee = Tuple.Create(accProvider.GetMainAccount().Result, new GenomeEntity());

            coach.Run(new List<Tuple<AccountEntity,GenomeEntity>>{trainee}, 1);

            Console.Read();
        }
    }
}
