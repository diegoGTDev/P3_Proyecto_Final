namespace Grafos{
    public class Vertice
    {
        public int id { get; set; }
        public string departamento { get; set; }
        public string tipo { get; set; }
        public List<Arista> aristas { get; set; }

        public Vertice(int id, string departamento, string tipo)
        {
            this.id = id;
            this.departamento = departamento;
            this.tipo = tipo;
            aristas = new List<Arista>();
        }

        public void agregarArista(Arista arista)
        {
            aristas.Add(arista);
        }
    }
}