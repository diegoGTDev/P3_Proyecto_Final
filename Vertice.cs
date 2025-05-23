namespace Grafo{
    public class Vertice
    {
        public int valor { get; set; }
        public List<Vertice> aristas { get; set; }

        public Vertice(int valor)
        {
            this.valor = valor;
            aristas = new List<Vertice>();
        }

        public void AdicionarVertice(Vertice vertice)
        {
            aristas.Add(vertice);
        }
    }
}