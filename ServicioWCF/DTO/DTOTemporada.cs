using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ServicioWCF.DTO
{
    [DataContract]
    public class DTOTemporada
    {
        [DataMember]
        public string CodISAN { get; set; }
        [DataMember]
        public string Titulo { get; set; }
        [DataMember]
        public int NroTemporada { get; set; }
        [DataMember]
        public DateTime FechaEstreno { get; set; }
        [DataMember]
        public string Imagen { get; set; }
        [DataMember]
        public IEnumerable<DTOEpisodio> Episodios { get; set; }

        public Temporada ConvertirDTO()
        {
            List<Episodio> ep = new List<Episodio>();
            foreach (var i in Episodios) { ep.Add(i.ConvertirDTO()); }
            return new Temporada()
            {
                Titulo = this.Titulo,
                NroTemporada = this.NroTemporada,
                FechaEstreno = this.FechaEstreno,
                Imagen = this.Imagen,
                Episodios = ep
            };
        }
    }
}
