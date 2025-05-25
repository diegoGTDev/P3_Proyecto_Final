namespace Grafos{
    public class Arista
    {
        public Vertice origen { get; set; }
        public Vertice destino { get; set; }
        public int tiempo_camino { get; set; }

        public Arista(Vertice origen, Vertice destino, int tiempo)
        {
            this.origen = origen;
            this.destino = destino;
            this.tiempo_camino = tiempo;
        }

    }
}