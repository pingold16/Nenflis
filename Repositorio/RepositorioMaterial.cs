using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Dominio;

namespace Repositorio
{
    class RepositorioMaterial : IRepositorio<Material>
    {
        SqlConnection miCon = new SqlConnection();
        miCon.ConnectionString = @"Server=LAPTOP-O3S7JT2T\SQLEXPRESS;
                            DataBase=Obligatorio; Integrated Security=true";

        public bool Add(Material item)
        {
            bool ret = false;
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = miCon;
            cmd.CommandText = "Insert into ";
            try
            {
                miCon.Open();
                cmd.ExecuteNonQuery();
                ret = true;
            }
            catch (Exception)
            {

                throw;
            }
            finally { miCon.Close(); }
            return ret;
        }

        public IEnumerable<Material> FindAll()
        {
            IEnumerable<Material> ret = null;
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = miCon;
            cmd.CommandText = "Select * from Material";
            try
            {
                miCon.Open();
                ret = cmd.ExecuteQuery();
            }
            catch (Exception)
            {
                throw;
            }
            finally { miCon.Close(); }

            return ret;
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
