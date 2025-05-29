using System.Numerics;
using Archivador;
using Microsoft.VisualBasic;

namespace Grafos
{
    public class Grafo
    {
        private Dictionary<string, List<string>>? departamentosVecinos { get; set; }
        private List<Vertice>? vertices { get; set; }

        private List<Vertice> sucursales { get; set; }

        public Grafo()
        {
            this.cargarVecinos();
            this.cargarVertices();
            this.cargarSucursales();
            this.ConectarGeograficamente(this.vertices, this.departamentosVecinos);

        }

        private void cargarVertices()
        {
            ArchivadorJSON archivador = new ArchivadorJSON("src/puntos_guatemala_500.json");
            this.vertices = archivador.cargarDatos();
        }

        private void cargarSucursales()
        {
            ArchivadorJSON archivador = new ArchivadorJSON("src/sucursales.json");
            this.sucursales = archivador.cargarDatos();
        }

        public void mostrarSucursales()
        {
            foreach (var v in this.sucursales)
            {
                Console.WriteLine($"Sucursal {v.departamento}");
            }
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

        public void encontrarCamino(int destinoId)
        {
            var origenId = this.sucursales[0].id;
            if (vertices == null) return;

            var origen = vertices.FirstOrDefault(v => v.id == origenId);
            var destino = vertices.FirstOrDefault(v => v.id == destinoId);

            if (origen == null || destino == null)
            {
            Console.WriteLine("Origen o destino no encontrado.");
            return;
            }

            var distancias = new Dictionary<Vertice, int>();
            var previo = new Dictionary<Vertice, Vertice?>();
            var visitados = new HashSet<Vertice>();
            var cola = new List<(Vertice vertice, int prioridad)>();

            foreach (var v in vertices)
            {
            distancias[v] = int.MaxValue;
            previo[v] = null;
            }
            distancias[origen] = 0;
            cola.Add((origen, 0));

            while (cola.Count > 0)
            {
            // Extraer el nodo con menor prioridad (distancia)
            cola.Sort((a, b) => a.prioridad.CompareTo(b.prioridad));
            var actual = cola[0].vertice;
            cola.RemoveAt(0);

            if (visitados.Contains(actual)) continue;
            visitados.Add(actual);

            if (actual == destino) break;

            foreach (var arista in actual.aristas)
            {
                var vecino = arista.destino;
                int nuevaDist = distancias[actual] + arista.tiempo_camino;
                if (nuevaDist < distancias[vecino])
                {
                distancias[vecino] = nuevaDist;
                previo[vecino] = actual;
                cola.Add((vecino, nuevaDist));
                }
            }
            }

            if (distancias[destino] == int.MaxValue)
            {
            Console.WriteLine("No existe un camino entre los nodos dados.");
            return;
            }

            // Reconstruir el camino
            var camino = new List<Vertice>();
            for (var v = destino; v != null; v = previo[v])
            camino.Add(v);
            camino.Reverse();

            Console.WriteLine($"Camino más corto de Sucursal Central {vertices.Find(v => v.id == origenId).departamento} a {vertices.Find(v => v.id == destinoId).departamento} (tiempo total: {distancias[destino]} min):");
            foreach (var v in camino)
            {
            Console.Write($"{v.id} ");
            }
            Console.WriteLine();
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