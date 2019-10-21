using ServicioWCF.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using Repositorio;
using Dominio;
using Dominios.EntidadesNegocio;
using System.IO;

namespace ServicioWCF
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service : IService
    {
        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }

        public bool addTemporada(DTOTemporada t)
        {
            return rTemp.Add(t.ConvertirDTO());
        }

        RepositorioTemporada rTemp = new RepositorioTemporada();

        public IEnumerable<DTOTemporada> traerTemporadas()
        {
            List<DTOTemporada> t = new List<DTOTemporada>();
            foreach (var i in rTemp.FindAll()) { t.Add(convertTemp(i)); }
            return t;
        }

        private DTOTemporada convertTemp(Temporada t)
        {
            DTOTemporada dt = new DTOTemporada()
            {
                CodISAN = t.CodISAN,
                NroTemporada = t.NroTemporada,
                Titulo = t.Titulo,
                FechaEstreno = t.FechaEstreno,
                Imagen = t.Imagen
            };
            List<DTOEpisodio> e = new List<DTOEpisodio>();
            foreach (var i in t.Episodios)
            {
                e.Add(new DTOEpisodio()
                { 
                    Ordial = i.Ordial,
                    Titulo = i.Titulo,
                    Descripcion = i.Descripcion,
                    Duracion = i.Duracion
                });
            }
            dt.Episodios = e;
            return dt;
        }

        RepositorioUser rus = new RepositorioUser();

        public bool validarUser(DTOUser e)
        {
            return rus.validarUser(new User() { user = e.user, pass = e.pass });
        }

        public bool addUser(DTOUser u)
        {
            return rus.Add(new User()
            {
                user = u.user,
                pass = u.pass
            });
        }

        RepositorioMaterial rMat = new RepositorioMaterial();

        public IEnumerable<DTOMaterial> cargarMaterial()
        {
            List<DTOMaterial> m = new List<DTOMaterial>();
            foreach (var i in rMat.FindAll())
            {
                DTOPersona dir = new DTOPersona()
                {
                    Id = i.Director.Id,
                    Nombre = i.Director.Nombre,
                    Apellido = i.Director.Apellido,
                    PaisNacimiento = convPais(i.Director.PaisNacimiento),
                    NombreArt = i.Director.NombreArt,
                    MinutoPantalla = i.Director.MinutoPantalla
                };
                DTOGenero g = new DTOGenero() { Nombre = i.Genero.Nombre, Descripcion = i.Genero.Descripcion };
                m.Add(new DTOMaterial()
                {
                    CodISAN = i.CodISAN,
                    Titulo = i.Titulo,
                    Director = dir,
                    FechaEstreno = i.FechaEstreno,
                    Genero = g,
                    Pais = convPais(i.Pais),
                    Descripcion = i.Descripcion,
                    Imagen = i.Imagen,
                    Tipo = (i is Pelicula)
                });
            }
            return m;
        }

        public object traerMaterial(string id)
        {
            Material m = rMat.FindById(id);
            DTOPersona dir = new DTOPersona()
            {
                Id = m.Director.Id,
                Nombre = m.Director.Nombre,
                Apellido = m.Director.Apellido,
                PaisNacimiento = convPais(m.Director.PaisNacimiento),
                NombreArt = m.Director.NombreArt,
                MinutoPantalla = m.Director.MinutoPantalla
            };
            DTOGenero g = new DTOGenero() { Nombre = m.Genero.Nombre, Descripcion = m.Genero.Descripcion };
            return new DTOMaterial()
            {
                CodISAN = m.CodISAN,
                Titulo = m.Titulo,
                FechaEstreno = m.FechaEstreno,
                Genero = g,
                Pais = convPais(m.Pais),
                Descripcion = m.Descripcion,
                Imagen = m.Imagen,
                Tipo = (m is Pelicula)
            };
        }

        public bool deleteMaterial(string id)
        {
            return rMat.Remove(id);
        }

        public DTOMaterial cargarMaterialxId(string id)
        {
            Material m = rMat.FindById(id);
            DTOPersona dir = new DTOPersona()
            {
                Id = m.Director.Id,
                Nombre = m.Director.Nombre,
                Apellido = m.Director.Apellido,
                PaisNacimiento = convPais(m.Director.PaisNacimiento)
            };
            DTOGenero g = new DTOGenero() { Nombre = m.Genero.Nombre, Descripcion = m.Genero.Descripcion };
            return new DTOMaterial()
            {
                CodISAN = m.CodISAN,
                Titulo = m.Titulo,
                Director = dir,
                FechaEstreno = m.FechaEstreno,
                Genero = g,
                Pais = convPais(m.Pais),
                Descripcion = m.Descripcion,
                Imagen = m.Imagen,
                Tipo = (m is Pelicula)
            };
        }

        private DTOPais convPais(Pais p)
        {
            return new DTOPais()
            {
                Id = p.Id,
                Nombre = p.Nombre
            };
        }


        public bool editMaterial(DTOMaterial m)
        {
            return rMat.Update(m.ConvertirDTO());
        }


        public bool removeMaterial(string id)
        {
            return rMat.Remove(id);
        }


        public IEnumerable<DTOMaterial> cargarMaterialxTipo(string type)
        {
            List<DTOMaterial> m = new List<DTOMaterial>();
            foreach (var i in rMat.FindAllByType(type))
            {
                DTOPersona dir = new DTOPersona()
                {
                    Id = i.Director.Id,
                    Nombre = i.Director.Nombre,
                    Apellido = i.Director.Apellido,
                    PaisNacimiento = convPais(i.Director.PaisNacimiento),
                    NombreArt = i.Director.NombreArt,
                    MinutoPantalla = i.Director.MinutoPantalla
                };
                DTOGenero g = new DTOGenero() { Nombre = i.Genero.Nombre, Descripcion = i.Genero.Descripcion };
                m.Add(new DTOMaterial()
                {
                    CodISAN = i.CodISAN,
                    Titulo = i.Titulo,
                    Director = dir,
                    FechaEstreno = i.FechaEstreno,
                    Genero = g,
                    Pais = convPais(i.Pais),
                    Descripcion = i.Descripcion,
                    Imagen = i.Imagen,
                    Tipo = (i is Pelicula)
                });
            }
            return m;
        }


        public bool addMaterial(DTOMaterial m)
        {
            List<Persona> elen = new List<Persona>();
            if (m.Elenco != null)
            {
                foreach (var i in m.Elenco)
                {
                    elen.Add(new Persona()
                    {
                        Id = i.Id,
                        Nombre = i.Nombre,
                        Apellido = i.Apellido,
                        PaisNacimiento = i.PaisNacimiento.ConvertirDTO(),
                        NombreArt = i.NombreArt,
                        MinutoPantalla = i.MinutoPantalla
                    });
                }
            }
            bool ret = false;
            if (m.Tipo) //TRUE = PELICULA
            {
                ret = rMat.Add(new Pelicula()
                {
                    CodISAN = m.CodISAN,
                    Titulo = m.Titulo,
                    //Director = m.Director.ConvertirDTO(),
                    FechaEstreno = m.FechaEstreno,
                    //Pais = m.Pais.ConvertirDTO(),
                    //Genero = m.Genero.ConvertirDTO(),
                    Elenco = elen
                });
            }
            else
            {
                ret = rMat.Add(new Serie()
                {
                    CodISAN = m.CodISAN,
                    Titulo = m.Titulo,
                    Director = m.Director.ConvertirDTO(),
                    FechaEstreno = m.FechaEstreno,
                    Elenco = elen
                });
            }
            return ret;
        }

        public IEnumerable<DTOPais> cargarPaises()
        {
            RepositorioPais rPais = new RepositorioPais();
            List<DTOPais> paises = new List<DTOPais>();
            foreach (var i in rPais.FindAll())
            {
                paises.Add(new DTOPais()
                {
                    Id = i.Id,
                    Nombre = i.Nombre
                });
            }
            return paises;
        }

        public IEnumerable<DTOGenero> cargarGenero()
        {
            RepositorioGenero rGenero = new RepositorioGenero();
            List<DTOGenero> generos = new List<DTOGenero>();
            foreach (var i in rGenero.FindAll())
            {
                generos.Add(new DTOGenero()
                {
                    Nombre = i.Nombre,
                    Descripcion = i.Descripcion
                });
            }
            return generos;
        }

        RepositorioPersona rPer = new RepositorioPersona();

        public void generarTxt()
        {
            FileStream fs = File.Open(@"C:\Migracion.txt", FileMode.OpenOrCreate);
            StreamWriter sw = new StreamWriter(fs);
            string sep = "#";
            sw.WriteLine("Materiales");
            foreach (Material i in rMat.FindAll())
            {
                sw.WriteLine(i.CodISAN + i.Titulo + i.Pais.Nombre + i.Genero.Nombre
                      + i.FechaEstreno + i.Descripcion + i.Imagen + sep);
            }

            sw.WriteLine("Temporadas");
            foreach (Temporada i in rTemp.FindAll())
            {
                sw.WriteLine(i.CodISAN + i.NroTemporada + i.Titulo
                    + i.FechaEstreno + sep);
                foreach (var e in i.Episodios)
                {
                    sw.WriteLine(e.Ordial + e.Titulo + e.Descripcion + e.Duracion + sep);
                }
            }

            sw.WriteLine("Personas");
            foreach (var i in rPer.FindAll())
            {
                sw.WriteLine(i.Id + i.Nombre + i.Apellido + i.NombreArt
                      + i.PaisNacimiento.Nombre + sep);
            }
            sw.Close();
        }
    }
}

