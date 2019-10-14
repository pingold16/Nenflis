using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio
{
    public class Serie : Material
    {
        public List<Temporada> Temporadas { get; set; }
    }
}
