using System.Collections.Generic;
using Grafos;
using Archivador;
Grafo grafo = new Grafo();
Menu.dibujarMenu();
int opc = Menu.esperarRespuesta();
switch (opc)
{
    case 1: grafo.mostrarSucursales(); break;
    case 2: grafo.encontrarCamino(300); break;
}