using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominios.EntidadesNegocio;

namespace Repositorio
{
    public class RepositorioUser
    {
        public bool validarUser(User user)
        {
            SqlConnection cn = UtilidadesBD.CrearConexion();
            //Preparar el comando
            SqlCommand cmd = new SqlCommand("SELECT * FROM Usuario WHERE Usu=@nom and Pass = @pass", cn);
            cmd.Parameters.Add(new SqlParameter("@nom", (string)user.user));
            cmd.Parameters.Add(new SqlParameter("@pass", (string)user.pass));

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
                            return true;
                        }
                    }
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
