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

        RepositorioUser rus = new RepositorioUser();
        [OperationContract]
        public bool validarUser(DTOUser e)
        {
            return rus.validarUser(new User() { user = e.user, pass = e.pass });
        }

        RepositorioMaterial rMat = new RepositorioMaterial();
        [OperationContract]
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
            return new DTOMaterial()
            {
                CodISAN = m.CodISAN
                //continuara...
            };
        }

        public bool deleteMaterial(string id)
        {
            return rMat.Remove(id);
        }

        [OperationContract]
        public DTOMaterial cargarMaterial(string id)
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

        [OperationContract]
        public bool editMaterial(DTOMaterial m)
        {
            return rMat.Update(m.ConvertirDTO());
        }

        [OperationContract]
        public bool removeMaterial(string id)
        {
            return rMat.Remove(id);
        }

        [OperationContract]
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

        [OperationContract]
        public bool addMaterial(DTOMaterial m)
        {
            bool ret = false;
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
            if (m.Tipo) //TRUE = PELICULA
            {
                 ret = rMat.Add(new Pelicula() {
                    CodISAN = m.CodISAN,
                    Titulo = m.Titulo,
                    Director = m.Director.ConvertirDTO(),
                    FechaEstreno = m.FechaEstreno,
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

        [OperationContract]
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

        [OperationContract]
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
    }
}
