using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProyectoRestaurante.Models;
using ProyectoRestaurante.Servicios;
using System.Reflection;

namespace ProyectoRestaurante.Controllers
{
    public class PedidosController:Controller
    {
        private readonly IRepositorioPedidos repositorioPedidos;
        private readonly IRepositorioMesas repositorioMesas;
        private readonly IRepositorioEmpleados repositorioEmpleados;
        private readonly IMapper mapper;

        public PedidosController(IRepositorioPedidos repositorioPedidos, IRepositorioMesas repositorioMesas, IRepositorioEmpleados repositorioEmpleados, IMapper mapper)
        {
            this.repositorioPedidos = repositorioPedidos;
            this.repositorioMesas = repositorioMesas;
            this.repositorioEmpleados = repositorioEmpleados;
            this.mapper = mapper;
        }
        public async Task<IActionResult> index()
        {
            var pedidos = await repositorioPedidos.ObtenerActivos();
            return View(pedidos);
        }
        public async Task<IActionResult> Crear()
        {
            var modelo = new PedidosCreacionViewModel();
            modelo.MesaPedido = await ObtenerMesas();
            modelo.MeseroPedido = await ObtenerMesero();
            return View(modelo);
        }
        [HttpPost]
        public async Task<IActionResult> Crear(PedidosCreacionViewModel modelo)
        {
            if (!ModelState.IsValid)
            {
                modelo.MesaPedido = await ObtenerMesas();
                modelo.MeseroPedido = await ObtenerMesero();
                return View(modelo);
            }
            var mesa = await repositorioMesas.ObtenerporId(modelo.mesa_pedido);
            if (mesa is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            var empleado = await repositorioEmpleados.ObtenerporId(modelo.mesero_pedido);
            if (empleado is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            await repositorioPedidos.CrearPedido(modelo);
            return RedirectToAction("Crear","Detalle");

        }
        private async Task<IEnumerable<SelectListItem>> ObtenerMesas()
        {
            var mesas = await repositorioMesas.ObtenerDispo();
            return mesas.Select(x => new SelectListItem(x.num_mesa.ToString(), x.id_mesa.ToString()));

        }
        private async Task<IEnumerable<SelectListItem>> ObtenerMesero()
        {
            var mesero = await repositorioEmpleados.Obtener();
            return mesero.Select(x => new SelectListItem(x.primer_nombre+" "+x.primer_apellido+" - "+x.doc_empleado,x.id_empleado.ToString()));

        }

    }
}
