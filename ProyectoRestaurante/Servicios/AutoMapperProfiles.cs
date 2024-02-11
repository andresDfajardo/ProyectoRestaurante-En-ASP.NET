using AutoMapper;
using ProyectoRestaurante.Models;

namespace ProyectoRestaurante.Servicios
{
    public class AutoMapperProfiles:Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Carta, CartaCreacionViewModel>();
            CreateMap<Pedidos, PedidosCreacionViewModel>();
            CreateMap<DetallePedidos,DetallePedidosCreacionViewModel>();
        }
    }
}
