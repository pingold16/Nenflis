using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using Dominio;

namespace ServicioWCF.DTO
{
    [DataContract]
    public class DTOGenero
    {
        [DataMember]
        public string Nombre { get; set; }
        [DataMember]
        public string Descripcion { get; set; }

        internal Genero ConvertirDTO()
        {
            return new Genero()
            {
                Nombre = this.Nombre,
                Descripcion = this.Descripcion
            };
        }
    }
}