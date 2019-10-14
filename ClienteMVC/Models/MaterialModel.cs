using ServicioWCF.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClienteMVC.Models
{
    public class MaterialModel
    {
        public string CodISAN { get; set; }
         
        public string Titulo { get; set; }
         
        public string FechaEstreno { get; set; }
         
        public DTOPersona Director { get; set; }
         
        public IEnumerable<DTOPersona> Elenco { get; set; }
         
        public DTOGenero Genero { get; set; }
         
        public DTOPais Pais { get; set; }
         
        public string Descripcion { get; set; }
         
        public string Imagen { get; set; }
        
        public bool Tipo { get; set; } //TRUE = PELICULA
    }
}