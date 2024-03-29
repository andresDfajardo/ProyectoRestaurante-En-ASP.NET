﻿using Microsoft.AspNetCore.Mvc.Rendering;

namespace ProyectoRestaurante.Models
{
    public class DetallePedidosCreacionViewModel:DetallePedidos
    {
        public IEnumerable<SelectListItem> PlatoPedido { get; set; }
        public string NombrePlato { get; set; }
        public int Subtotal { get; set; }
    }
}
