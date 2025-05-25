using Archivador;
using Microsoft.VisualBasic;

namespace Grafos
{
    public class Grafo
    {
        private Dictionary<string, List<string>>? departamentosVecinos { get; set; }
        private List<Vertice>? vertices { get; set; }

        public Grafo()
        {
            this.cargarVecinos();
            this.cargarVertices();
            this.ConectarGeograficamente(this.vertices, this.departamentosVecinos);

        }

        private void cargarVertices()
        {
            ArchivadorJSON archivador = new ArchivadorJSON("src/puntos_guatemala_500.json");
            this.vertices = archivador.cargarDatos();
        }
        private void cargarVecinos()
        {
            departamentosVecinos = CargadorVecinos.CargarVecinosDesdeJson("src/departamentos_vecinos.json");
        }

        public void ConectarGeograficamente(List<Vertice> vertices, Dictionary<string, List<string>> vecinosDepartamentales)
        {
            Random rand = new Random();

            foreach (var v in vertices)
            {
                // Conectar dentro del mismo departamento
                var mismosDepto = vertices.FindAll(v => v.departamento == v.departamento && v.id != v.id);
                AgregarConexiones(v, mismosDepto, rand, 3);

                // Conectar con departamentos vecinos si existen
                if (vecinosDepartamentales.TryGetValue(v.departamento, out var vecinos))
                {
                    var vecinosDepto = vertices.FindAll(v => vecinos.Contains(v.departamento));
                    AgregarConexiones(v, vecinosDepto, rand, 2);
                }
            }
        }

        private void AgregarConexiones(Vertice origen, List<Vertice> destinos, Random rand, int cantidadMax)
        {
            HashSet<int> usados = new HashSet<int>();
            for (int i = 0; i < cantidadMax && destinos.Count > 0; i++)
            {
                Vertice destino = destinos[rand.Next(destinos.Count)];
                if (usados.Contains(destino.id)) continue;

                usados.Add(destino.id);
                int tiempo = rand.Next(10, 180);
                origen.agregarArista(new Arista(origen, destino, tiempo));
            }
        }
        public void MostrarGrafo(int cantidad = 10)
        {
            Console.WriteLine("📊 DEMOSTRACIÓN DEL GRAFO");
            Console.WriteLine("-------------------------");

            foreach (var v in this.vertices.Take(cantidad))
            {
                Console.WriteLine($"Punto ID {v.id} ({v.tipo} - {v.departamento})");

                if (v.aristas.Count == 0)
                {
                    Console.WriteLine("Sin conexiones.");
                    continue;
                }

                foreach (var a in v.aristas)
                {
                    Console.WriteLine($"->Conecta a ID {a.destino.id} ({a.destino.departamento}) en {a.tiempo_camino} min");
                }

                Console.WriteLine();
            }
        }


    }
}