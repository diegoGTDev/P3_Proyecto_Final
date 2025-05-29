using System.Collections.Generic;
using Grafos;
using Archivador;
Grafo grafo = new Grafo();
Menu.dibujarMenu();
int opc = Menu.esperarRespuesta();
switch (opc)
{
    case 1:
        Console.WriteLine("ID\t|\tNombre");
        foreach (var d in Departamentos.encontrarDepartamentos())
        {
            Console.WriteLine($"[{d.id}]\t\t{d.departamento}\t");
        }
        Console.Write("Selecciona un departamento: ");
        int departamentoSeleccionado = Int32.Parse(Console.ReadLine());
        ///generar pedido
        break;
    case 2: grafo.encontrarCamino(300); break;
}
Console.Read();
