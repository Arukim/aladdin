using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Aladdin.DAL.Models;

namespace Aladdin.Common.Interfaces {
    public interface ICoach {
        void Run(IEnumerable<Tuple<AccountEntity, GenomeEntity>> trainee, int rounds);
    }
}