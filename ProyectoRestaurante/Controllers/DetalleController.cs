using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProyectoRestaurante.Models;
using ProyectoRestaurante.Servicios;

namespace ProyectoRestaurante.Controllers
{
    public class DetalleController:Controller
    {
        private readonly IRepositorioDetallePedido repositorioDetallePedido;
        private readonly IRepositorioPedidos repositorioPedidos;
        private readonly IRepositorioCarta repositorioCarta;
        private readonly IMapper mapper;

        public DetalleController(IRepositorioDetallePedido repositorioDetallePedido, IRepositorioPedidos repositorioPedidos, IRepositorioCarta repositorioCarta, IMapper mapper)
        {
            this.repositorioDetallePedido = repositorioDetallePedido;
            this.repositorioPedidos = repositorioPedidos;
            this.repositorioCarta = repositorioCarta;
            this.mapper = mapper;
        }
        /*public async Task<IActionResult> index()
        {
            var detallePedidos = await repositorioDetallePedido.Obtener();
            return View(detallePedidos);
        }*/
        public async Task<IActionResult> Crear()
        {
            var modelo = new DetallePedidosCreacionViewModel();
            modelo.PlatoPedido = await ObtenerPlatos();
            return View(modelo);

        }
        [HttpPost]
        public async Task <IActionResult> Crear(DetallePedidosCreacionViewModel modelo)
        {
            if (!ModelState.IsValid)
            {
                modelo.PlatoPedido = await ObtenerPlatos();
                return View(modelo);
            }
            var plato = await repositorioCarta.ObtenerporId(modelo.id_carta);
            if(plato is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            await repositorioDetallePedido.CrearDetalle(modelo);
            modelo.PlatoPedido = await ObtenerPlatos();
            modelo.cantidad = 0;
            return View(modelo);
        }
        private async Task<IEnumerable<SelectListItem>> ObtenerPlatos()
        {
            var platos = await repositorioCarta.Obtener();
            return platos.Select(x => new SelectListItem(x.nombre_plato+" "+x.precio_plato+" X Und", x.id.ToString()));

        }
        [HttpGet]
        public async Task<ActionResult> Consultar(DetallePedidosCreacionViewModel detal)
        {
            var detalles = await repositorioDetallePedido.ObtenerPlatos(detal.id_pedido);
            if (detalles is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            return View(detalles);
        }
    }
}
