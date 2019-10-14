using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio
{
    public abstract class Material
    {
        public string CodISAN { get; set; } //Codigo hexadecimal
        public string Titulo { get; set; }
        public DateTime FechaEstreno { get; set; }
        public Director Director { get; set; }
        public List<Actor> Elenco { get; set; }
        public Pais Pais { get; set; }
        public string Descripcion { get; set; }
        public string Imagen { get; set; }
    }
}
