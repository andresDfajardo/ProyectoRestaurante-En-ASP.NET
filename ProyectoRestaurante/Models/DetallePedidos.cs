namespace ProyectoRestaurante.Models
{
    public class DetallePedidos
    {
        public int id_detalle { get; set; }
        public int id_pedido { get; set; }
        public int id_carta { get; set; }
        public int cantidad { get; set; }
    }
}
