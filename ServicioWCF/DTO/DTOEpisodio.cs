using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ServicioWCF.DTO
{
    [DataContract]
    public class DTOEpisodio
    {
        [DataMember]
        public string Ordial { get; set; }
        [DataMember]
        public string Titulo { get; set; }
        [DataMember]
        public string Descripcion { get; set; }
        [DataMember]
        public int Duracion { get; set; }

        public Episodio ConvertirDTO()
        {
            return new Episodio()
            {
                Ordial = this.Ordial,
                Titulo = this.Titulo,
                Descripcion = this.Descripcion,
                Duracion = this.Duracion
            };
        }
    }
}