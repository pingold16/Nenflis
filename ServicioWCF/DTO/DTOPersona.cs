using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using Dominio;

namespace ServicioWCF.DTO
{
    [DataContract]
    public class DTOPersona
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Nombre { get; set; }
        [DataMember]
        public string Apellido { get; set; }
        [DataMember]
        public DTOPais PaisNacimiento { get; set; }
        [DataMember]
        public string NombreArt { get; set; }
        [DataMember]
        public int MinutoPantalla { get; set; }

        internal Persona ConvertirDTO()
        {
            return new Persona()
            {
                Id = this.Id,
                Nombre = this.Nombre,
                Apellido = this.Apellido,
                PaisNacimiento = this.PaisNacimiento.ConvertirDTO(),
                NombreArt = this.NombreArt,
                MinutoPantalla = this.MinutoPantalla
            };
        }
    }
}