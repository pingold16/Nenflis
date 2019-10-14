using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio
{
    public class Pelicula : Material
    {
        public int CantEntradas { get; set; }
        public int MontoRecaudado { get; set; }
        public int Duracion { get; set; }
    }
}
