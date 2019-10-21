using Dominio;
using Repositorio;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositorio
{
    public class RepositorioTemporada : IRepositorio<Temporada>
    {
        public bool Add(Temporada item)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Temporada> FindAll()
        {
            SqlConnection cn = UtilidadesBD.CrearConexion();
            //Preparar el comando
            SqlCommand cmd = new SqlCommand(@"SELECT t.*, e.Ordial, e.Titulo as ETitulo, e.Descripcion as EDesc, e.Duracion
                                            FROM Temporada t, Episodio e 
                                            Where t.CodISAN = e.CodISAN and t.NroTemporada = e.NroTemporada", cn);

            //El comando está completo, ejecutarlo
            try
            {
                using (cn) // el bloque using asegura que se realice el dispose de la conexión
                {

                    UtilidadesBD.AbrirConexion(cn);
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        List<Temporada> lista = new List<Temporada>();
                        while (dr.Read())
                        {
                            Temporada t = new Temporada()
                            {
                                CodISAN = dr["CodISAN"].ToString(),
                                Titulo = dr["Titulo"].ToString(),
                                NroTemporada = (int)dr["NroTemporada"],
                                FechaEstreno = (DateTime)dr["FechaEstreno"],
                                Episodios = new List<Episodio>(),
                                Imagen = dr["Imagen"].ToString()
                            };
                            Temporada aux = new Temporada();
                            bool isExist = false;
                            foreach (var i in lista)
                                if (i.CodISAN == t.CodISAN && i.NroTemporada == t.NroTemporada)
                                { 
                                    isExist = true;
                                    aux = i;
                                }
                            if (!isExist) lista.Add(t);

                            //Y agregamos los episodios a su lista! 
                            if (aux.CodISAN != null)
                            {
                                aux.Episodios.Add(new Episodio()
                                {
                                    Ordial = dr["Ordial"].ToString(),
                                    Titulo = dr["ETitulo"].ToString(),
                                    Descripcion = dr["EDesc"].ToString(),
                                    Duracion = (int)dr["Duracion"]
                                });
                            }
                            else
                            {
                                t.Episodios.Add(new Episodio()
                                {
                                    Ordial = dr["Ordial"].ToString(),
                                    Titulo = dr["ETitulo"].ToString(),
                                    Descripcion = dr["EDesc"].ToString(),
                                    Duracion = (int)dr["Duracion"]
                                });
                            }
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

        public Temporada FindById(object id)
        {
            throw new NotImplementedException();
        }

        public bool Remove(object id)
        {
            throw new NotImplementedException();
        }

        public bool Update(Temporada item)
        {
            throw new NotImplementedException();
        }
    }
}
