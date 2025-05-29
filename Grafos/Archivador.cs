using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Grafos;
namespace Archivador
{
    public class ArchivadorJSON
    {
        string rutaArchivo { get; set; }
        public ArchivadorJSON(string rutaArchivo)
        {
            this.rutaArchivo = rutaArchivo;
        }

        // Crea una funcion para cargar los datos del archivo JSON
        public List<Vertice> cargarDatos()
        {
            string contenido = File.ReadAllText(rutaArchivo);
            List<PuntoReferencia> ?puntosReferencia = JsonSerializer.Deserialize<List<PuntoReferencia>>(contenido);
            List<Vertice> vertices = new List<Vertice>();

            if (puntosReferencia != null)
            {
                foreach (var punto in puntosReferencia)
                {
                    Vertice vertice = new Vertice(punto.id, punto.departamento, punto.tipo);
                    // Console.WriteLine($"Cargando punto de referencia: {punto.id}, {punto.departamento}, {punto.tipo}");
                    vertices.Add(vertice);
                }
            }

            return vertices;
        }
    }
}