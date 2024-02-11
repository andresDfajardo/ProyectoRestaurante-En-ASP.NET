using Microsoft.AspNetCore.Mvc.Rendering;

namespace ProyectoRestaurante.Models
{
    public class CartaCreacionViewModel:Carta
    {
        public IEnumerable<SelectListItem> TiposPlatos { get; set; }
    }
}
