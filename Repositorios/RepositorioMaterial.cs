using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Dominio;

namespace Repositorios
{
    public class RepositorioMaterial : IRepositorio<Material>
    {
        public bool Add(Material item)
        {
            bool ret = false;
            SqlConnection miCon = new SqlConnection();


            return ret;
        }

        public IEnumerable<Material> FindAll()
        {
            throw new NotImplementedException();
        }

        public Material FindById(object id)
        {
            throw new NotImplementedException();
        }

        public bool Remove(Material item)
        {
            throw new NotImplementedException();
        }

        public bool Update(Material item)
        {
            throw new NotImplementedException();
        }
    }
}
