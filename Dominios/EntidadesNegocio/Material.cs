using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio
{
    public abstract class Material
    {
        public string CodISAN { get; set; } //Codigo hexadecimal
        public string Titulo { get; set; }
        public string FechaEstreno { get; set; }
        public Persona Director { get; set; }
        public List<Persona> Elenco { get; set; }
        public Genero Genero { get; set; }
        public Pais Pais { get; set; }
        public string Descripcion { get; set; }
        public string Imagen { get; set; }

        public bool Validar()
        {
            throw new NotImplementedException();
        }
    }
}
