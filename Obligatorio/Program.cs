using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repositorio;
using Dominio;

namespace Obligatorio
{
    class Program
    {
        static void Main(string[] args)
        {
            SerieTest();
            GeneroTest();
        }

        static readonly RepositorioMaterial rMaterial = new RepositorioMaterial();
        static readonly RepositorioPersona rPersona = new RepositorioPersona();
        private static void SerieTest()
        {
            List<Persona> actors = new List<Persona>
            {
                rPersona.FindById(1),
                rPersona.FindById(2),
                rPersona.FindById(3),
                rPersona.FindById(4),
                rPersona.FindById(5)
            };
            Serie serie = new Serie
            {
                CodISAN = "ARG12",
                Titulo = "Peaky Blinders",
                FechaEstreno = "11-09-1997",
                Director = rDir.FindById(3),
                Elenco = actors,
                Genero = rGenero.FindById(3),
                Pais = new Pais()/*rPais.FindById(3)*/,
                Descripcion = "Ambientada en 1919, refleja la historia de una famosa mafia britanica.",
                Imagen = "Portada"
            };
        }



        #region GeneroTest
        static readonly RepositorioGenero rGenero = new RepositorioGenero();
        private static void GeneroTest()
        {
            Console.WriteLine("-----INICIO TEST GENERO-----");
            Console.WriteLine();
            CrearGenero();
            Console.WriteLine();
            Console.WriteLine("----------------------------");
            Console.WriteLine();
            TraerGenero();
            Console.WriteLine();
            Console.WriteLine("----------------------------");
            Console.WriteLine();
            ListarGeneros();
            Console.WriteLine();
            Console.WriteLine("----------------------------");
            Console.WriteLine();
            BorrarGenero();
            Console.WriteLine();
            Console.WriteLine("----------------------------");
            Console.WriteLine();
            ListarGeneros();
            Console.WriteLine();
            Console.WriteLine("------------FIN------------");
            Console.WriteLine();

            Console.ReadKey();
        }

        private static void CrearGenero()
        {
            Console.WriteLine("Agregando Genero: Terror");
            if (rGenero.Add(new Genero { Nombre = "Terror", Descripcion = "Da miedo..." }))
                Console.WriteLine("Agregado con esito.");
            else Console.WriteLine("Errorrrrrrr...");

            //rGenero.Add(new Genero { Nombre = "Comedia", Descripcion = "Da risa..." });
            //rGenero.Add(new Genero { Nombre = "Romantica", Descripcion = "Da amor..." });
            //rGenero.Add(new Genero { Nombre = "Documental", Descripcion = "Da aburrimiento..." });
        }

        private static void ListarGeneros()
        {
            Console.WriteLine("Listado de generos.");
            IEnumerable<Genero> g = rGenero.FindAll();
            foreach (var i in g)
            {
                Console.WriteLine(i.ToString());
            }
        }

        private static void TraerGenero()
        {
            Console.WriteLine("Buscando Genero: Terror");
            Genero g = rGenero.FindById("Terror");
            if (!(g is null))
                Console.WriteLine("Resultado: " + g.ToString());
            else Console.WriteLine("No se econtro el genero.");
        }

        private static void BorrarGenero()
        {
            Console.WriteLine("Borrando Genero: Terror");
            if (rGenero.Remove("Terror"))
                Console.WriteLine("Borrado con esito.");
            else Console.WriteLine("Errorrrrrrr...");
        }
        #endregion
    }
}
