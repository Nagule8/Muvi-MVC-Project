using Muvi.Data.Base;
using Muvi.Data.Interfaces;
using Muvi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Muvi.Data.Repositories
{
    public class ActorService : GenericService<Actor>,IActorInterface
    {
            public ActorService(AppDbContext context) : base(context) {  }
    }
}
