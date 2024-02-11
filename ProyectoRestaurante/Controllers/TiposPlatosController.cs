using Microsoft.AspNetCore.Mvc;
using ProyectoRestaurante.Models;
using ProyectoRestaurante.Servicios;

namespace ProyectoRestaurante.Controllers
{
    public class TiposPlatosController:Controller
    {
        private readonly IRepositorioTiposPlatos repositorioTiposPlatos;

        public TiposPlatosController(IRepositorioTiposPlatos repositorioTiposPlatos) 
        {
            this.repositorioTiposPlatos = repositorioTiposPlatos;
        }
        public async Task<IActionResult> Index()
        {
            var tiposPlatos = await repositorioTiposPlatos.Obtener();
            return View(tiposPlatos);
        }
        public IActionResult Crear()
        {
            return View();
        }
        [HttpPost]
        public async Task <IActionResult> Crear (TiposPlatos tiposPlatos)
        {
            if (!ModelState.IsValid)
            {
                return View(tiposPlatos);
            }
            var yaExisteCategoriaPlato = await repositorioTiposPlatos.Existe(tiposPlatos.categoria);
            if (yaExisteCategoriaPlato)
            {
                ModelState.AddModelError(nameof(tiposPlatos.categoria), $"La categoria {tiposPlatos.categoria} ya existe.");
                return View(tiposPlatos);
            }
            await repositorioTiposPlatos.Crear(tiposPlatos);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> VerificarExisteCategoriaPlato(string categoria)
        {
            var yaExisteCategoriaPlato = await repositorioTiposPlatos.Existe(categoria);
            if (yaExisteCategoriaPlato)
            {
                return Json($"La categoria {categoria} ya existe.");
            }
            return Json(true);
        }
        [HttpGet]
        public async Task<ActionResult> Editar(int id_categoria)
        {
            var tipoPlato = await repositorioTiposPlatos.ObtenerporId(id_categoria);
            if (tipoPlato is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            return View(tipoPlato);
        }
        [HttpPost]
        public async Task<IActionResult> Editar(TiposPlatos tiposPlatos)
        {
            var tipoPExiste = await repositorioTiposPlatos.ObtenerporId(tiposPlatos.id_categoria);

            if (tipoPExiste is null)
            {
               
               return RedirectToAction("NoEncontrado", "Home");
            }

            await repositorioTiposPlatos.Actualizar(tiposPlatos);
            return RedirectToAction("Index");
        }
        public async Task<ActionResult> Borrar(int id_categoria)
        {
            var tipoPlato = await repositorioTiposPlatos.ObtenerporId(id_categoria);
            if (tipoPlato is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            return View(tipoPlato);
        }
        [HttpPost]
        public async Task<IActionResult> EliminarTipo(int id_categoria)
        {
            var tipoPExiste = await repositorioTiposPlatos.ObtenerporId(id_categoria);

            if (tipoPExiste is null)
            {

                return RedirectToAction("NoEncontrado", "Home");
            }

            await repositorioTiposPlatos.Borrar(id_categoria);
            return RedirectToAction("Index");
        }
    }
}
