using System.ComponentModel;
using System.Numerics;
using System.Runtime.Serialization;
using Archivador;
using Microsoft.VisualBasic;

namespace Grafos
{
    public class Grafo
    {
        public Dictionary<string, List<string>>? departamentosVecinos { get; set; }
        public List<Vertice>? vertices { get; set; }

        public List<Vertice> sucursales { get; set; }

        public Grafo()
        {
            this.cargarVecinos();
            this.cargarVertices();
            this.cargarSucursales();
            this.ConectarGeograficamente(this.vertices, this.departamentosVecinos);

        }

        public void cargarVertices()
        {
            ArchivadorJSON archivador = new ArchivadorJSON("src/puntos_guatemala_500.json");
            this.vertices = archivador.cargarDatos();
        }

        public void cargarSucursales()
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
        public void cargarVecinos()
        {
            departamentosVecinos = CargadorVecinos.CargarVecinosDesdeJson("src/departamentos_vecinos.json");
        }

        public void ConectarGeograficamente(List<Vertice> vertices, Dictionary<string, List<string>> vecinosDepartamentales)
        {
            Random rand = new Random();

            foreach (var v in vertices)
            {
                // Conectar dentro del mismo departamento
                var mismosDepto = vertices.FindAll(d => d.departamento == v.departamento && d.id != v.id);
                AgregarConexiones(v, mismosDepto, rand);

                //Asegurarse de conectar la sucursal (si es que existe)
                var sucursal = vertices.FindAll(v2 => v2.departamento == v.departamento && (v2.tipo == "sucursal" || v2.tipo == "sucursal central") && v2.id != v.id);
                if (sucursal != null)
                {
                    AgregarConexiones(v, sucursal, rand);
                }
                // Conectar con departamentos vecinos si existen
                if (vecinosDepartamentales.TryGetValue(v.departamento, out var vecinos))
                {
                    var vecinosDepto = vertices.FindAll(v => vecinos.Contains(v.departamento));
                    vecinosDepto.RemoveAll(i => i.tipo == "sucursal" || i.tipo == "sucursal central");
                    AgregarConexiones(v, vecinosDepto, rand, vecinos: true);
                }

            }

        }

        private void AgregarConexiones(Vertice origen, List<Vertice> destinos, Random rand, bool vecinos = false)
        {
            HashSet<int> usados = new HashSet<int>();
            foreach (var destino in destinos)
            {
                if (usados.Contains(destino.id)) continue;

                usados.Add(destino.id);
                int tiempo = 0;
                if (vecinos)
                {
                    // Si es vecino, asignar un tiempo aleatorio entre 30 y 60 minutos
                    tiempo = rand.Next(30, 60);
                }
                else if (origen.departamento == destino.departamento)
                {
                    // Si es del mismo departamento, asignar un tiempo aleatorio entre 10 y 30 minutos
                    tiempo = rand.Next(10, 30);
                }
                else
                {
                    tiempo = rand.Next(60, 180);
                }
                origen.agregarArista(new Arista(origen, destino, tiempo));
            }
        }

        public (List<Vertice>, int) encontrarCamino(int origenId, int destinoId)
        {

            if (vertices == null) return (null, 0);

            var origen = vertices.FirstOrDefault(v => v.id == origenId);
            var destino = vertices.FirstOrDefault(v => v.id == destinoId);

            if (origen == null || destino == null)
            {
                Console.WriteLine("Origen o destino no encontrado.");
                return (null, 0);
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
                return (null, 0);
            }

            // Reconstruir el camino
            var camino = new List<Vertice>();
            for (var v = destino; v != null; v = previo[v])
                camino.Add(v);
            camino.Reverse();
            return (camino, distancias[destino]);
        }


    }
}