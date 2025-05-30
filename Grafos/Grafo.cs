using System.ComponentModel;
using System.Numerics;
using System.Runtime.Serialization;
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

        public void MostrarPedidoGraficamente(int destino)
        {
            //Encontrar la sucursal más cercana al destino
            int idSucursalCercana = 0;
            int caminoMasCorto = int.MaxValue;
            List<Vertice> caminoClienteSucursal = null;

            foreach (var s in this.sucursales)
            {
                var aux_camino = encontrarCamino(destino, s.id);
                if (aux_camino.Item2 < caminoMasCorto && aux_camino.Item1 != null)
                {
                    Console.WriteLine($"Camino mas corto: {aux_camino.Item2} minutos");
                    caminoMasCorto = aux_camino.Item2;
                    idSucursalCercana = s.id;
                    caminoClienteSucursal = aux_camino.Item1;
                }
            }

            var sucursalCentral = this.sucursales.FirstOrDefault(s => s.tipo == "sucursal central");
            List<Vertice> caminoCentralSucursal = null;
            if (sucursalCentral != null && sucursalCentral.id != idSucursalCercana)
            {
                var caminoCentral = encontrarCamino(sucursalCentral.id, idSucursalCercana);
                caminoCentralSucursal = caminoCentral.Item1;
                Console.WriteLine($"Camino mas corto: {caminoCentral.Item2} minutos");
                caminoMasCorto += caminoCentral.Item2;
            }
            var caminoTotal = new List<Vertice>();
            caminoClienteSucursal.Reverse();
            caminoCentralSucursal?.RemoveAt(caminoCentralSucursal.Count - 1);
            if (caminoCentralSucursal != null)
            {
                caminoTotal.AddRange(caminoCentralSucursal);
            }
            if (caminoClienteSucursal != null)
            {

                caminoTotal.AddRange(caminoClienteSucursal);
            }
            var tiempoEstimado = 0;
            for (int i = 0; i < caminoTotal.Count; i++)
            {
                var v = caminoTotal[i];

                if (i < caminoTotal.Count - 1)
                {
                    var siguiente = caminoTotal[i + 1];
                    var arista = v.aristas.FirstOrDefault(a => a.destino.id == siguiente.id);
                    if (arista != null)
                    {
                        tiempoEstimado += arista.tiempo_camino;
                    }
                }
            }
            Menu.cambiarColor(ConsoleColor.Green);
            Console.WriteLine("Se ha procesado su pedido: ");
            Menu.cambiarColor();
            Console.Write("Su ubicación destino es: ");
            Menu.cambiarColor(ConsoleColor.Yellow);
            Console.WriteLine($"{vertices.Find(v => v.id == destino).departamento}");
            Menu.cambiarColor();
            Console.Write("Sucursal más cercana a su ubicación: ");
            Menu.cambiarColor(ConsoleColor.Yellow);
            Console.WriteLine($"{vertices.Find(v => v.id == idSucursalCercana).departamento}");
            Menu.cambiarColor();
            if (tiempoEstimado > 60)
            {
                int horas = tiempoEstimado / 60;
                int minutos = tiempoEstimado % 60;

                Console.Write("Tiempo estimado: ");
                Menu.cambiarColor(ConsoleColor.Yellow);
                Console.Write($"{horas}");
                Menu.cambiarColor();
                Console.Write(" horas y ");
                Menu.cambiarColor(ConsoleColor.Yellow);
                Console.Write($"{minutos}");
                Menu.cambiarColor();
                Console.WriteLine(" minutos");

            }
            else
            {
                Console.Write("Tiempo estimado: ");
                Menu.cambiarColor(ConsoleColor.Yellow);
                Console.Write($"{tiempoEstimado}");
                Menu.cambiarColor();
                Console.WriteLine(" minutos");

            }
            Menu.cambiarColor(ConsoleColor.Green);
            Console.WriteLine("Ruta:");
            Menu.cambiarColor();
            for (int i = 0; i < caminoTotal.Count; i++)
            {
                var v = caminoTotal[i];
                string marca = "[*]";
                Console.Write($"{marca} Punto ID {v.id} ({v.tipo} - {v.departamento})");

                if (i < caminoTotal.Count - 1)
                {
                    var siguiente = caminoTotal[i + 1];
                    var arista = v.aristas.FirstOrDefault(a => a.destino.id == siguiente.id);
                    if (arista != null)
                    {
                        Console.WriteLine();
                        Console.WriteLine($"   ===> ID {arista.destino.id} ({arista.destino.departamento}) en {arista.tiempo_camino} min");
                    }
                    else
                    {
                        Console.WriteLine();
                        Console.WriteLine("   ===> (sin información de arista)");
                    }
                }
                else
                {
                    Console.WriteLine();
                }
            }
        }


    }
}