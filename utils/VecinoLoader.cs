using System.Text.Json;

public static class CargadorVecinos
{
    public static Dictionary<string, List<string>> CargarVecinosDesdeJson(string ruta)
    {
        try
        {
            string contenido = File.ReadAllText(ruta);
            var vecinos = JsonSerializer.Deserialize<Dictionary<string, List<string>>>(contenido);
            return vecinos ?? new Dictionary<string, List<string>>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error cargando vecinos: {ex.Message}");
            return new Dictionary<string, List<string>>();
        }
    }
}