﻿using System.Linq;
using System.Threading.Tasks;

namespace Rad3.Models.Domian
{
    public interface IRepository<T>
    {
        IQueryable<T> GetAll();
        Task<T> GetById(object id);
    }
}
