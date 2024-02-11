using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using ProyectoRestaurante.Models;
using ProyectoRestaurante.Servicios;

namespace ProyectoRestaurante.Controllers
{
    public class MesasController:Controller
    {
        private readonly IRepositorioMesas repositorioMesas;

        public MesasController(IRepositorioMesas repositorioMesas) {
            this.repositorioMesas = repositorioMesas;
        }
        public async Task<IActionResult> index()
        {
            var mesas = await repositorioMesas.Obtener();
            return View(mesas);
        }
        public IActionResult Crear()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Crear(Mesas mesas)
        {

            if(!ModelState.IsValid)
            {
                return View(mesas);
            }
            var yaExisteMesa = await repositorioMesas.Existe(mesas.num_mesa);
            if (yaExisteMesa)
            {
                ModelState.AddModelError(nameof(mesas.num_mesa), $"La mesa numero {mesas.num_mesa} ya existe.");

                return View(mesas);
            }
            await repositorioMesas.Crear(mesas);

            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult>VerificarExisteMesa(int num_mesa)
        {
            var yaExisteMesa = await repositorioMesas.Existe(num_mesa);
            if(yaExisteMesa)
            {
                return Json($"El numero de mesa {num_mesa} ya existe");
            }
            return Json(true);
        }
        [HttpGet]
        public async Task<ActionResult> Editar(int id_mesa)
        {
            var mesa = await repositorioMesas.ObtenerporId(id_mesa);
            if(mesa is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            return View(mesa);
        }
        [HttpPost]
        public async Task<IActionResult> Editar(Mesas mesas)
        {
            var mesaExiste = await repositorioMesas.ObtenerporId(mesas.id_mesa);
            if (mesaExiste is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            
            await repositorioMesas.Actualizar(mesas);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult>Borrar(int id_mesa)
        {
            var mesa = await repositorioMesas.ObtenerporId(id_mesa);
            if (mesa is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            return View(mesa);
        }
        [HttpPost]
        public async Task<IActionResult> BorrarMesa(int id_mesa)
        {
            var mesa = await repositorioMesas.ObtenerporId(id_mesa);
            if (mesa is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            await repositorioMesas.Borrar(id_mesa);
            return RedirectToAction("Index");

        }
    }
}
