namespace Grafo{
    public class Arista
    {
        public Vertice Vertice1 { get; set; }
        public Vertice Vertice2 { get; set; }
        public int Peso { get; set; }

        public Arista(Vertice vertice1, Vertice vertice2, int peso)
        {
            Vertice1 = vertice1;
            Vertice2 = vertice2;
            Peso = peso;
        }
    }
}