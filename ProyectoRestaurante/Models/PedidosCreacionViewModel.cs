using Microsoft.AspNetCore.Mvc.Rendering;

namespace ProyectoRestaurante.Models
{
    public class PedidosCreacionViewModel:Pedidos
    {
        public IEnumerable<SelectListItem> MesaPedido { get; set; }
        public IEnumerable<SelectListItem> MeseroPedido { get; set; }

    }
}
