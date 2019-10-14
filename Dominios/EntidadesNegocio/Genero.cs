using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio
{
    public class Genero
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }

        public bool Validar()
        {
            return true;
        }

        public override string ToString() => "Nombre: " + this.Nombre + ", Descripcion: " + this.Descripcion;
    }
}
