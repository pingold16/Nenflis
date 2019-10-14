using Dominio;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositorio
{
    public class RepositorioGenero : IRepositorio<Genero>
    {
        public bool Add(Genero item)
        {
            if (item == null || !item.Validar())
                return false;

            SqlConnection cn = UtilidadesBD.CrearConexion();
            //Preparar el comando
            SqlCommand cmd = new SqlCommand("INSERT INTO Genero VALUES (@nom,@desc)", cn);
            cmd.Parameters.Add(new SqlParameter("@nom", item.Nombre));
            cmd.Parameters.Add(new SqlParameter("@desc", item.Descripcion));
            //El comando está completo, ejecutarlo
            try
            {
                using (cn) // el bloque using asegura que se realice el dispose de la conexión
                {
                    UtilidadesBD.AbrirConexion(cn);
                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (SqlException ex)
            {
                System.Diagnostics.Debug.Fail(UtilidadesBD.MensajeExcepcion(ex));
                return false;
            }
        }

        public IEnumerable<Genero> FindAll()
        {
            SqlConnection cn = UtilidadesBD.CrearConexion();
            //Preparar el comando
            SqlCommand cmd = new SqlCommand("SELECT * FROM Genero", cn);

            //El comando está completo, ejecutarlo
            try
            {
                using (cn) // el bloque using asegura que se realice el dispose de la conexión
                {

                    UtilidadesBD.AbrirConexion(cn);
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        List<Genero> lista = new List<Genero>();
                        while (dr.Read())
                        {
                            lista.Add(new Genero()
                            {
                                Nombre = dr["Nombre"].ToString(),
                                Descripcion = dr["Descripcion"].ToString()
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

        public Genero FindById(object id)
        {
            SqlConnection cn = UtilidadesBD.CrearConexion();
            //Preparar el comando
            SqlCommand cmd = new SqlCommand("SELECT * FROM Genero WHERE Nombre=@nom", cn);
            cmd.Parameters.Add(new SqlParameter("@nom", (string)id));

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
                            return new Genero()
                            {
                                Nombre = dr["Nombre"].ToString(),
                                Descripcion = dr["Descripcion"].ToString()
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
            SqlConnection cn = UtilidadesBD.CrearConexion();
            //Preparar el comando
            SqlCommand cmd = new SqlCommand("DELETE Genero WHERE Nombre=@nom", cn);
            cmd.Parameters.Add(new SqlParameter("@nom", (string)id));

            //El comando está completo, ejecutarlo
            try
            {
                using (cn) // el bloque using asegura que se realice el dispose de la conexión
                {
                    UtilidadesBD.AbrirConexion(cn);
                    int filas = cmd.ExecuteNonQuery();
                    if (filas == 1)
                        return true;
                    else
                        return false;
                }
            }
            catch (SqlException ex)
            {
                System.Diagnostics.Debug.Fail(UtilidadesBD.MensajeExcepcion(ex));
                return false;
            }
        }

        public bool Update(Genero item)
        {
            //Verificar que los datos de la categoría cumplan las reglas de validación:
            if (item == null || !item.Validar())
                return false;

            SqlConnection cn = UtilidadesBD.CrearConexion();
            //Preparar el comando
            //Se cambian todos sus datos. Si alguno no está modificado en el objeto, permanecerá con el valor anterior.
            //La key no se cambia
            SqlCommand cmd = new SqlCommand(@" UPDATE CategoriaProducto 
                                                SET Nombre=@nom, Descripcion=@desc 
                                                WHERE id=@id", cn);

            cmd.Parameters.Add(new SqlParameter("@nom", item.Nombre));
            cmd.Parameters.Add(new SqlParameter("@desc", item.Descripcion));
            //El comando está completo, ejecutarlo
            try
            {
                using (cn) // el bloque using asegura que se realice el dispose de la conexión
                {
                    UtilidadesBD.AbrirConexion(cn);
                    int filas = cmd.ExecuteNonQuery();
                    if (filas == 1)
                        return true;
                    else
                        return false;
                }
            }
            catch (SqlException ex)
            {
                System.Diagnostics.Debug.Fail(UtilidadesBD.MensajeExcepcion(ex));
                return false;
            }
        }
    }
}
