using System;
using System.Runtime.Serialization;
using Dominio;

namespace ServicioWCF.DTO
{
    [DataContract]
    public class DTOPais
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Nombre { get; set; }

        internal Pais ConvertirDTO()
        {
            return new Pais()
            {
                Id = this.Id,
                Nombre = this.Nombre
            };
        }
    }
}