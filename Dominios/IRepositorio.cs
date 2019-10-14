using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio
{
    public interface IRepositorio<T>
    {
        bool Add(T item);
        bool Update(T item);
        bool Remove(Object id);
        T FindById(Object id);
        IEnumerable<T> FindAll();
    }
}
