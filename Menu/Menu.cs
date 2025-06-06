using System.Dynamic;
using Microsoft.VisualBasic;

public static class Menu
{
    private static ConsoleColor colorLetraActual = ConsoleColor.White;
    private static Dictionary<int, string> opciones = new Dictionary<int, string>();

    public static void cargarOpciones()
    {
        opciones.Add(1, "Realizar pedido");
        opciones.Add(2, "Salir");
    }
    private static void dibujarOpciones()
    {
        foreach (var opc in opciones)
        {
            cambiarColor(ConsoleColor.Yellow);
            Console.Write($"[{opc.Key}]"); cambiarColor(); Console.WriteLine($" {opc.Value}");
        }
    }
    public static void cargarAnimación()
    {
        Console.Clear();
        cambiarColor(ConsoleColor.Red);
        Console.SetCursorPosition(50, 10);
        Console.Write("Cargando recursos");
        cambiarColor(ConsoleColor.Green);

        for (var i = 0; i < 50; i++)
        {
            Console.Write(".");
            if (i % 10 == 0)
            {
                Console.Clear();
                cambiarColor(ConsoleColor.Red);
                Console.SetCursorPosition(50, 10);
                Console.Write("Cargando recursos");
                cambiarColor(ConsoleColor.Green);
            }
            if (i <= 50)
            {
                Thread.Sleep(25);
            }
            else
            {
                Thread.Sleep(40);
            }
        }
    }
    public static void dibujarMenu()
    {
        Console.Clear();
        cambiarColor(ConsoleColor.Green);
        Console.WriteLine($"\t\tSistema de entrega de paquetería");
        cambiarColor();
        dibujarOpciones();
    }

    public static int esperarRespuesta(out int respuesta)
    {
        Console.Write(">> ");
        respuesta = Int32.Parse(Console.ReadLine() ?? "");
        return respuesta;
    }
    public static void cambiarColor(ConsoleColor? color = null)
    {
        if (color == null)
        {
            Console.ForegroundColor = colorLetraActual;
        }
        else
        {
            Console.ForegroundColor = (ConsoleColor)color;
        }
    }

    public static void mostrarDepartamentos()
    {
        Console.WriteLine("ID\t|\tNombre");
        foreach (var d in Departamentos.encontrarDepartamentos())
        {
            Console.WriteLine($"[{d.id}]\t\t{d.departamento}\t");
        }
    }
}
