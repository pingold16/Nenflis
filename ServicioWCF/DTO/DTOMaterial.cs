using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ServicioWCF.DTO
{
    [DataContract]
    public class DTOMaterial
    {
        [DataMember]
        public string CodISAN { get; set; }
        [DataMember]
        public string Titulo { get; set; }
        [DataMember]
        public string FechaEstreno { get; set; }
        [DataMember]
        public DTOPersona Director { get; set; }
        [DataMember]
        public IEnumerable<DTOPersona> Elenco { get; set; }
        [DataMember]
        public DTOGenero Genero { get; set; }
        [DataMember]
        public DTOPais Pais { get; set; }
        [DataMember]
        public string Descripcion { get; set; }
        [DataMember]
        public string Imagen { get; set; }
        [DataMember]
        public bool Tipo { get; set; } //TRUE = PELICULA

        public Material ConvertirDTO()
        {
            List<Persona> elen = new List<Persona>();
            foreach (var i in Elenco){ elen.Add(i.ConvertirDTO()); }
            if (Tipo)
            {
                return new Pelicula()
                {
                    CodISAN = this.CodISAN,
                    Titulo = this.Titulo,
                    FechaEstreno = this.FechaEstreno,
                    Director = this.Director.ConvertirDTO(),
                    Elenco = elen,
                    Genero = this.Genero.ConvertirDTO(),
                    Descripcion = this.Descripcion,
                    Imagen = this.Imagen
                };
            }
            else
            {
                return new Serie()
                {
                    CodISAN = this.CodISAN,
                    Titulo = this.Titulo,
                    FechaEstreno = this.FechaEstreno,
                    Director = this.Director.ConvertirDTO(),
                    Elenco = elen,
                    Genero = this.Genero.ConvertirDTO(),
                    Descripcion = this.Descripcion,
                    Imagen = this.Imagen
                };
            }
        }
    }
}
