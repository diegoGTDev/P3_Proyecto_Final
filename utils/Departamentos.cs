using System.Numerics;
using Archivador;
using Grafos;

public static class Departamentos
{

    public static List<Vertice> encontrarEnElMismoDepto(String departamento)
    {
        List<Vertice> deptos = new ArchivadorJSON("src/puntos_guatemala_500.json").cargarDatos();

        List<Vertice> mismoDepto = deptos.FindAll(v => (v.departamento == departamento && v.tipo != "sucursal" && v.tipo != "sucursal central"));

        return mismoDepto;

    }

    public static List<Vertice> encontrarDepartamentos()
    {
        var deptos = new ArchivadorJSON("src/puntos_guatemala_500.json").cargarDatos();
        deptos = deptos.FindAll(v => v.tipo == "municipio");
        return deptos;
    }
}