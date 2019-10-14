using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Dominio;

namespace Repositorio
{
    public class RepositorioMaterial : IRepositorio<Material>
    {
        SqlConnection cn = new SqlConnection();
        RepositorioPersona rPersona = new RepositorioPersona();
        RepositorioGenero rGenero = new RepositorioGenero();
        RepositorioPais rPais = new RepositorioPais();

        public bool Add(Material item)
        {
            if (item == null || !item.Validar())
                return false;

            SqlConnection cn = UtilidadesBD.CrearConexion();
            //Preparar el comando

            SqlCommand cmd = new SqlCommand("INSERT INTO Material VALUES (@cod, @titulo, @fechEst, @dir, @gen, @pais, @desc, @img)", cn);
            cmd.Parameters.Add(new SqlParameter("@cod", item.CodISAN));
            cmd.Parameters.Add(new SqlParameter("@titulo", item.Titulo));
            cmd.Parameters.Add(new SqlParameter("@fechEst", item.FechaEstreno));
            cmd.Parameters.Add(new SqlParameter("@dir", item.Director.Id));
            cmd.Parameters.Add(new SqlParameter("@gen", item.Genero.Nombre));
            cmd.Parameters.Add(new SqlParameter("@pais", item.Pais.Id));
            cmd.Parameters.Add(new SqlParameter("@desc", item.Descripcion));
            cmd.Parameters.Add(new SqlParameter("@img", item.Imagen));

            SqlCommand cmdH = new SqlCommand();
            SqlTransaction transaction = null;
            Pelicula p = item as Pelicula;
            if (p != null)
            {
                cmdH = new SqlCommand("INSERT INTO Pelicula Values (@cod, @cantEntradas, @monto, @duracion)", cn);
                cmdH.Parameters.Add(new SqlParameter("@cod", item.CodISAN));
                cmdH.Parameters.Add(new SqlParameter("@cantEntradas", p.CantEntradas));
                cmdH.Parameters.Add(new SqlParameter("@monto", p.MontoRecaudado));
                cmdH.Parameters.Add(new SqlParameter("@duracion", p.Duracion));
            }
            else
            {
                cmdH = new SqlCommand("INSERT INTO Serie Values (@cod)", cn);
                cmdH.Parameters.AddWithValue("@cod", item.CodISAN);
            }

            //El comando está completo, ejecutarlo
            try
            {
                using (cn) // el bloque using asegura que se realice el dispose de la conexión
                {
                    UtilidadesBD.AbrirConexion(cn);
                    transaction = cn.BeginTransaction();
                    cmd.Transaction = transaction;

                    cmd.ExecuteNonQuery();
                    cmdH.ExecuteNonQuery();

                    //Me creo un id unico para todo el elenco. Ademas, me aseguro que no se repitan los actores.
                    SqlCommand sel = new SqlCommand("SELECT MAX(ElencoId) FROM Elenco ", cn);
                    int ElencoId = (int)sel.ExecuteScalar() + 1; //Le sumo uno al id mas grande

                    //Alta elenco
                    foreach (var e in item.Elenco)
                    {
                        if (e.Id != item.Director.Id) //El director no puede actuar
                        {
                            //Si no existe el Actor, lo agrego
                            SqlCommand cmdE = new SqlCommand(@"INSERT INTO Persona Values (@id, @nombre, @apellido, @pais, @nArt) 
                                                            Where @cod NOT IN (SELECT PersonaId From Persona Where PersonaId = @cod)", cn);
                            cmdE.Parameters.AddWithValue("@id", e.Id);
                            cmdE.Parameters.AddWithValue("@nombre", e.Nombre);
                            cmdE.Parameters.AddWithValue("@apellido", e.Apellido);
                            cmdE.Parameters.AddWithValue("@pais", e.PaisNacimiento.Id);
                            cmdE.Parameters.AddWithValue("@nArt", e.NombreArt);
                            cmdE.ExecuteNonQuery();
                            //Inserto en Elenco
                            cmdE = new SqlCommand(@"INSERT INTO Elenco Values (@id, @codIsan, @persId)", cn);
                            cmdE.Parameters.AddWithValue("@id", ElencoId);
                            cmdE.Parameters.AddWithValue("@codIsan", item.CodISAN);
                            cmdE.Parameters.AddWithValue("@persId", e.Id);
                            cmdE.ExecuteNonQuery();
                        }
                    }
                    transaction.Commit();
                    return true;
                }
            }
            catch (SqlException ex)
            {
                System.Diagnostics.Debug.Fail(UtilidadesBD.MensajeExcepcion(ex));
                return false;
            }
        }

        public IEnumerable<Material> FindAll()
        {
            SqlConnection cn = UtilidadesBD.CrearConexion();
            //Preparar el comando, con join para traer todo de una
            SqlCommand cmd = new SqlCommand(@"Select m.*, peli.*, p.Name as PaisNombre, g.Descripcion as DescGenero
                                            from Material m, Countries p, Genero g, Pelicula peli
                                            where m.PaisId = p.Id and m.Genero = g.Nombre and m.CodISAN = peli.CodISAN;
                                            Select m.*, peli.*, p.Name as PaisNombre, g.Descripcion as DescGenero
                                            from Material m, Countries p, Genero g, Serie peli
                                            where m.PaisId = p.Id and m.Genero = g.Nombre and m.CodISAN = peli.CodISAN;", cn);

            //El comando está completo, ejecutarlo
            try
            {
                using (cn) // el bloque using asegura que se realice el dispose de la conexión
                {
                    UtilidadesBD.AbrirConexion(cn);
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        List<Material> lista = new List<Material>();
                        do {
                            while (dr.Read())
                            {
                                Material m;
                                if (dr["Tipo"].ToString().Equals("P"))
                                {
                                    m = new Pelicula()
                                    {
                                        CodISAN = dr["CodISAN"].ToString(),
                                        Titulo = dr["Titulo"].ToString(),
                                        FechaEstreno = dr["FechaEstreno"].ToString(),
                                        Director = rPersona.FindById((int)dr["DirectorId"]),
                                        Genero = new Genero() {
                                            Nombre = dr["Genero"].ToString(),
                                            Descripcion = dr["DescGenero"].ToString()
                                        },
                                        Pais = new Pais() {
                                            Id = (int)dr["PaisId"],
                                            Nombre = dr["PaisNombre"].ToString()
                                        },
                                        Descripcion = dr["Descripcion"].ToString(),
                                        Imagen = dr["Imagen"].ToString()
                                    };
                                }
                                else
                                {
                                    m = new Serie()
                                    {
                                        CodISAN = dr["CodISAN"].ToString(),
                                        Titulo = dr["Titulo"].ToString(),
                                        FechaEstreno = dr["FechaEstreno"].ToString(),
                                        Director = rPersona.FindById((int)dr["DirectorId"]),
                                        Genero = new Genero()
                                        {
                                            Nombre = dr["Genero"].ToString(),
                                            Descripcion = dr["DescGenero"].ToString()
                                        },
                                        Pais = new Pais()
                                        {
                                            Id = (int)dr["PaisId"],
                                            Nombre = dr["PaisNombre"].ToString()
                                        },
                                        Descripcion = dr["Descripcion"].ToString(),
                                        Imagen = dr["Imagen"].ToString()
                                    };
                                }
                                lista.Add(m);
                            }
                        } while (dr.NextResult());
                        return lista;
                    }
                    return new List<Material>();
                }
            }
            catch (SqlException ex)
            {
                System.Diagnostics.Debug.Fail(UtilidadesBD.MensajeExcepcion(ex));
                return null;
            }
        }
        
        public IEnumerable<Material> FindAllByType(string type)
        {
            SqlConnection cn = UtilidadesBD.CrearConexion();
            //Preparar el comando
            SqlCommand cmd = new SqlCommand(@"SELECT m.*, e.* FROM Material m, Elenco e, Persona p
                                            WHERE e.CodISAN = m.CodISAN and p.Id = e.ElencoId and Tipo=@type;", cn);
            cmd.Parameters.Add(new SqlParameter("@type", type));

            List<Material> retorno = new List<Material>();
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
                            List<Persona> elenco = new List<Persona>();
                            while (dr.Read())
                            {
                                elenco.Add(new Persona
                                {
                                    Id = (int)dr["Id"],
                                    Nombre = dr["Nombre"].ToString(),
                                    Apellido = dr["Apellido"].ToString(),
                                    PaisNacimiento = rPais.FindById((int)dr["PaisId"]),
                                    NombreArt = dr["NombreArt"].ToString(),
                                    MinutoPantalla = (int)dr["MinutoPantalla"]
                                });
                            }
                            if (type == "P")
                            {
                                while (dr.Read())
                                {
                                    retorno.Add(new Pelicula()
                                    {
                                        CodISAN = dr["CodISAN"].ToString(),
                                        Titulo = dr["Titulo"].ToString(),
                                        FechaEstreno = dr["FechaEstreno"].ToString(),
                                        Director = rPersona.FindById(dr["Director"]),
                                        Genero = rGenero.FindById(dr["Genero"]),
                                        Pais = rPais.FindById(dr["PaisId"]),
                                        Descripcion = dr["Descripcion"].ToString(),
                                        Imagen = dr["Imagen"].ToString(),
                                        Elenco = elenco
                                    });
                                }
                            }
                            else
                            {
                                while (dr.Read())
                                {
                                    retorno.Add(new Serie()
                                    {
                                        CodISAN = dr["CodISAN"].ToString(),
                                        Titulo = dr["Titulo"].ToString(),
                                        FechaEstreno = dr["FechaEstreno"].ToString(),
                                        Director = rPersona.FindById(dr["Director"]),
                                        Genero = rGenero.FindById(dr["Genero"]),
                                        Pais = rPais.FindById(dr["PaisId"]),
                                        Descripcion = dr["Descripcion"].ToString(),
                                        Imagen = dr["Imagen"].ToString(),
                                        Elenco = elenco
                                    });
                                }
                            }
                        }
                    }
                    IEnumerable<Material> ret = retorno;
                    return ret;
                }
            }
            catch (SqlException ex)
            {
                System.Diagnostics.Debug.Fail(UtilidadesBD.MensajeExcepcion(ex));
                return null;
            }
        }
        
        public Material FindById(object id)
        {
            SqlConnection cn = UtilidadesBD.CrearConexion();
            //Preparar el comando
            SqlCommand cmd = new SqlCommand("SELECT * FROM Material WHERE CodISAN=@id; SELECT * FROM Elenco Where CodISAN=@id", cn);
            cmd.Parameters.Add(new SqlParameter("@id", id));

            //El comando está completo, ejecutarlo
            try
            {
                using (cn) // el bloque using asegura que se realice el dispose de la conexión
                {
                    UtilidadesBD.AbrirConexion(cn);
                    SqlDataReader dr = cmd.ExecuteReader();
                    Material ret = null;
                    if (dr.HasRows)
                    {
                        if (dr.Read())
                        {
                            if (dr["Tipo"].ToString() == "S")
                            {
                                ret = new Serie()
                                {
                                    CodISAN = dr["CodISAN"].ToString(),
                                    Titulo = dr["Titulo"].ToString(),
                                    FechaEstreno = dr["FechaEstreno"].ToString(),
                                    Director = rPersona.FindById(dr["Director"]),
                                    Genero = rGenero.FindById(dr["Genero"]),
                                    Pais = rPais.FindById(dr["PaisId"]),
                                    Descripcion = dr["Descripcion"].ToString(),
                                    Imagen = dr["Imagen"].ToString()
                                };
                            }
                            else
                            {
                                ret = new Pelicula()
                                {
                                    CodISAN = dr["CodISAN"].ToString(),
                                    Titulo = dr["Titulo"].ToString(),
                                    FechaEstreno = dr["FechaEstreno"].ToString(),
                                    Director = rPersona.FindById(dr["Director"]),
                                    Genero = rGenero.FindById(dr["Genero"]),
                                    Pais = rPais.FindById(dr["PaisId"]),
                                    Descripcion = dr["Descripcion"].ToString(),
                                    Imagen = dr["Imagen"].ToString()
                                };
                            }
                        }
                        if (dr.NextResult())
                        {
                            while (dr.Read())
                            {
                                ret.Elenco.Add(new Persona
                                {
                                    Id = (int)dr["Id"],
                                    Nombre = dr["Nombre"].ToString(),
                                    Apellido = dr["Apellido"].ToString(),
                                    PaisNacimiento = rPais.FindById((int)dr["PaisId"]),
                                    NombreArt = dr["NombreArt"].ToString(),
                                    MinutoPantalla = (int)dr["MinutoPantalla"]
                                });
                            }
                        }
                    }
                    return ret;
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
            SqlCommand cmd = new SqlCommand(@"Delete Elenco where id=@id;
                                            Delete Serie where id=@id;
                                            Delete Pelicula where id=@id;
                                            Delete Material WHERE id=@id", cn);
            cmd.Parameters.Add(new SqlParameter("@id", id));

            //El comando está completo, ejecutarlo
            try
            {
                using (cn) // el bloque using asegura que se realice el dispose de la conexión
                {
                    UtilidadesBD.AbrirConexion(cn);
                    int filas = cmd.ExecuteNonQuery();
                    if (filas > 0)
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

        public bool Update(Material item)
        {
            //Verificar que los datos de la categoría cumplan las reglas de validación:
            if (item == null || !item.Validar())
                return false;
            SqlConnection cn = UtilidadesBD.CrearConexion();
            //Preparar el comando
            //Se cambian todos sus datos. Si alguno no está modificado en el objeto, permanecerá con el valor anterior.
            //La key no se cambia
            SqlCommand cmd = new SqlCommand(@" UPDATE Material 
                                                SET Nombre=@nom, Descripcion=@desc 
                                                WHERE id=@id", cn);
            cmd.Parameters.Add(new SqlParameter("@id", item.CodISAN));
            cmd.Parameters.Add(new SqlParameter("@nom", item.Titulo));
            cmd.Parameters.Add(new SqlParameter("@nom", item.FechaEstreno));
            cmd.Parameters.Add(new SqlParameter("@nom", item.Imagen));
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
