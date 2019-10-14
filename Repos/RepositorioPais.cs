using Dominio;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositorio
{
    public class RepositorioPais : IRepositorio<Pais>
    {
        public bool Add(Pais item)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Pais> FindAll()
        {
            SqlConnection cn = UtilidadesBD.CrearConexion();
            //Preparar el comando
            SqlCommand cmd = new SqlCommand("SELECT * FROM Countries", cn);

            //El comando está completo, ejecutarlo
            try
            {
                using (cn) // el bloque using asegura que se realice el dispose de la conexión
                {

                    UtilidadesBD.AbrirConexion(cn);
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        List<Pais> lista = new List<Pais>();
                        while (dr.Read())
                        {
                            lista.Add(new Pais()
                            {
                                Id = (int)dr["Id"],
                                Nombre = dr["Name"].ToString()
                            });
                        }
                        return lista;
                    }
                    return null;
                }
            }
            catch (SqlException ex)
            {
                System.Diagnostics.Debug.Fail(UtilidadesBD.MensajeExcepcion(ex));
                return null;
            }
        }

        public Pais FindById(object id)
        {
            SqlConnection cn = UtilidadesBD.CrearConexion();
            //Preparar el comando
            SqlCommand cmd = new SqlCommand("SELECT * FROM Countries WHERE Id=@nom", cn);
            cmd.Parameters.Add(new SqlParameter("@nom", id));

            //El comando está completo, ejecutarlo
            try
            {
                using (cn) // el bloque using asegura que se realice el dispose de la conexión
                {
                    UtilidadesBD.AbrirConexion(cn);
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        if (dr.Read())
                        {
                            return new Pais()
                            {
                                Id = (int)dr["Id"],
                                Nombre = dr["Name"].ToString()
                            };
                        }
                    }
                    return null;
                }
            }
            catch (SqlException ex)
            {
                System.Diagnostics.Debug.Fail(UtilidadesBD.MensajeExcepcion(ex));
                return null;
            }
        }

        public bool Remove(object id)
        {
            throw new NotImplementedException();
        }

        public bool Update(Pais item)
        {
            throw new NotImplementedException();
        }
    }
}
