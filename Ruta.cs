using Grafos;
public static class Ruta
{
    public static void MostrarRuta(Grafo grafo, int destino)
    {
        //Encontrar la sucursal más cercana al destino
        int idSucursalCercana = 0;
        int caminoMasCorto = int.MaxValue;
        List<Vertice> caminoClienteSucursal = null;

        foreach (var s in grafo.sucursales)
        {
            var aux_camino = grafo.encontrarCamino(destino, s.id);
            if (aux_camino.Item2 < caminoMasCorto && aux_camino.Item1 != null)
            {
                caminoMasCorto = aux_camino.Item2;
                idSucursalCercana = s.id;
                caminoClienteSucursal = aux_camino.Item1;
            }
        }

        var sucursalCentral = grafo.sucursales.FirstOrDefault(s => s.tipo == "sucursal central");
        List<Vertice> caminoCentralSucursal = null;
        if (sucursalCentral != null && sucursalCentral.id != idSucursalCercana)
        {
            var caminoCentral = grafo.encontrarCamino(sucursalCentral.id, idSucursalCercana);
            caminoCentralSucursal = caminoCentral.Item1;
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
        Console.WriteLine($"{grafo.vertices.Find(v => v.id == destino).departamento}");
        Menu.cambiarColor();
        Console.Write("Sucursal más cercana a su ubicación: ");
        Menu.cambiarColor(ConsoleColor.Yellow);
        Console.WriteLine($"{grafo.vertices.Find(v => v.id == idSucursalCercana).departamento}");
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
            Console.Write($"{marca} Punto ({v.tipo} - {v.departamento})");

            if (i < caminoTotal.Count - 1)
            {
                var siguiente = caminoTotal[i + 1];
                var arista = v.aristas.FirstOrDefault(a => a.destino.id == siguiente.id);
                if (arista != null)
                {
                    Console.WriteLine();
                    Console.WriteLine($"  Hacia ===> ({arista.destino.tipo} - {arista.destino.departamento}) en {arista.tiempo_camino} min");
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