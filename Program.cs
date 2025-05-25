using System.Collections.Generic;
using Grafos;
using Archivador;
Grafo grafo = new Grafo();
grafo.MostrarGrafo();
//Se guarda el color original de la consola.
ConsoleColor colorActual = Console.ForegroundColor;
//Función para realizar el cambio de color en la consola.
void cambiarColor(ConsoleColor ?color = null)
{
    if (color == null)
    {
        Console.ForegroundColor = colorActual;
    }
    else
    {
        Console.ForegroundColor = (ConsoleColor) color;
    }
}
//Creamos las opciones
Dictionary<int, string> opciones = new Dictionary<int, string>();
opciones.Add(1, "Realizar pedido");
opciones.Add(2, "Salir");
int opcion;

//Realizamos una función para dibujar el menú de forma dinámica.
void crearMenu()
{
    foreach (var e in opciones)
    {
        cambiarColor(ConsoleColor.Yellow);
        Console.Write($"[{e.Key}]"); cambiarColor(); Console.WriteLine($" {e.Value}");
    }
}
//Principal
cambiarColor(ConsoleColor.Green);
Console.WriteLine($"\t\tSistema de entrega de paquetería");
cambiarColor();
crearMenu();
// void Camino(Vertice oVertice, string sangria = "")
// {
//     if (oVertice == null)
//     {
//         return;
//     }
//     Console.WriteLine(sangria+oVertice.valor);
//     foreach (var oV in oVertice.aristas)
//     {
//         Camino(oV, sangria+"\t");
//     }
// }
// Vertice v1 = new Vertice(6);
// Vertice v2 = new Vertice(4);
// Vertice v3 = new Vertice(3);
// Vertice v4 = new Vertice(5);
// Vertice v5 = new Vertice(2);
// Vertice v6 = new Vertice(1);
// v1.aristas.Add(v2); //6 a 4
// v2.aristas.Add(v3); // 4 a 3
// v2.aristas.Add(v4); //4 a 5
// v4.aristas.Add(v5); //5 a 2
// v4.aristas.Add(v6); //5 a 1
// v3.aristas.Add(v5); //3 a 2
// v5.aristas.Add(v6); //2 a 1
// Camino(v1);
