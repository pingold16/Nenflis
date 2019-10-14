using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio
{
    public class Persona
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public Pais PaisNacimiento { get; set; }
        public string NombreArt { get; set; }
        public int MinutoPantalla { get; set; }
    }
}
