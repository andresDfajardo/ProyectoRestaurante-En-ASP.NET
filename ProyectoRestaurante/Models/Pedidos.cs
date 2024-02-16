using System.ComponentModel.DataAnnotations;

namespace ProyectoRestaurante.Models
{
    public class Pedidos
    {
        public int id_pedido { get; set; }
        public int mesa_pedido { get; set; }
        public int mesero_pedido { get; set; }
        public string estado_pedido { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime fecha_pedido { get; set; } = DateTime.Now;
        public int TotalPedido { get; set; }
    }
}
