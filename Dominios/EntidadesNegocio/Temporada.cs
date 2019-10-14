using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio
{
    public class Temporada
    {
        public string Titulo { get; set; }
        public int NroTemporada { get; set; }
        public DateTime FechaEstreno { get; set; }
        public string Imagen { get; set; }
        public List<Episodio> Episodios { get; set; }
    }
}
