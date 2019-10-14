using Dominio;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositorio
{
    public class RepositorioPersona : IRepositorio<Persona>
    {
        private readonly RepositorioPais rPais = new RepositorioPais();
        public bool Add(Persona item)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Persona> FindAll()
        {
            throw new NotImplementedException();
        }

        public Persona FindById(object id)
        {
            SqlConnection cn = UtilidadesBD.CrearConexion();
            //Preparar el comando
            SqlCommand cmd = new SqlCommand("SELECT * FROM Persona WHERE Id=@nom", cn);
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
                            return new Persona()
                            {
                                Id = (int)dr["Id"],
                                Nombre = dr["Nombre"].ToString(),
                                Apellido = dr["Apellido"].ToString(),
                                PaisNacimiento = rPais.FindById((int)dr["PaisId"]),
                                NombreArt = dr["NombreArt"].ToString()
                                //MinutoPantalla = (int)dr["MinutoPantalla"]
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

        public bool Update(Persona item)
        {
            throw new NotImplementedException();
        }
    }
}
