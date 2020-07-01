using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HZY.EFCore.Repository
{
    using HZY.EFCore.Repository.Interface;

    public class DefaultRepository<T> : HZYRepositoryBase<T, EFCoreContext>
        where T : class, new()
    {

        public DefaultRepository(EFCoreContext _context) : base(_context) { }


    }
}
