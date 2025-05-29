using System.Collections.Generic;
using Grafos;
using Archivador;
Grafo grafo = new Grafo();
grafo.MostrarGrafo();
// Menu.cargarAnimación();
// Menu.cargarOpciones();
// bool correr = true;
// while (correr)
// {
//     Menu.dibujarMenu();
//     try
//     {
//         Menu.esperarRespuesta(out int opc);
//         switch (opc)
//         {
//             case 1:
//                 Console.WriteLine("ID\t|\tNombre");
//                 foreach (var d in Departamentos.encontrarDepartamentos())
//                 {
//                     Console.WriteLine($"[{d.id}]\t\t{d.departamento}\t");
//                 }
//                 Console.Write("Selecciona un departamento: ");
//                 int departamentoSeleccionado = Int32.Parse(Console.ReadLine());
//                 ///generar pedido
//                 break;
//             case 2: grafo.encontrarCamino(300); break;
//             case 3:
//                 Console.Clear();
//                 Console.WriteLine("Gracias por usar nuestro programa");
//                 correr = false;
//                 break;
//             default:
//                 Console.WriteLine("Escoja una opción disponible");break;
//         }
//     }
//     catch (Exception)
//     {
//         Menu.cambiarColor(ConsoleColor.Yellow); Console.WriteLine("Intente de nuevo"); Menu.cambiarColor();
//     }


//     Console.Write("Presiona una tecla para continuar");
//     Console.ReadKey(true);
// }
