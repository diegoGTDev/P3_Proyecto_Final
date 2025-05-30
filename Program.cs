using System.Collections.Generic;
using Grafos;
using Archivador;
Grafo grafo = new Grafo();
// Menu.cargarAnimación();
Menu.cargarOpciones();
bool correr = true;
while (correr)
{
    Menu.dibujarMenu();
    try
    {
        Menu.esperarRespuesta(out int opc);
        switch (opc)
        {
            case 1:
                Menu.mostrarDepartamentos();
                Console.Write("Selecciona tu departamento donde te ubicas: ");
                int departamentoSeleccionado = Int32.Parse(Console.ReadLine());
                Console.Clear();
                grafo.MostrarPedidoGraficamente(departamentoSeleccionado);
                break;
            case 2:
                Console.Clear();
                Console.WriteLine("Gracias por usar nuestro programa");
                correr = false;
                break;
            default:
                Console.WriteLine("Escoja una opción disponible");break;
        }
    }
    catch (Exception)
    {
        Menu.cambiarColor(ConsoleColor.Yellow); Console.WriteLine("Intente de nuevo"); Menu.cambiarColor();
    }


    Console.Write("Presiona una tecla para continuar");
    Console.ReadKey(true);
}
