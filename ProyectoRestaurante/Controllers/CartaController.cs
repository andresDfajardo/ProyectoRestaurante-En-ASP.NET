using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using ProyectoRestaurante.Models;
using ProyectoRestaurante.Servicios;
using System.Reflection;

namespace ProyectoRestaurante.Controllers
{
    public class CartaController:Controller
    {
        private readonly IRepositorioTiposPlatos repositorioTiposPlatos;
        private readonly IRepositorioCarta repositorioCarta;
        private readonly IMapper mapper;

        public CartaController(IRepositorioTiposPlatos repositorioTiposPlatos,IRepositorioCarta repositorioCarta,IMapper mapper)
        {
            this.repositorioTiposPlatos = repositorioTiposPlatos;
            this.repositorioCarta = repositorioCarta;
            this.mapper = mapper;
        }
        public async Task<IActionResult> index()
        {
            var cartas = await repositorioCarta.Obtener();
            return View(cartas);
        }
        [HttpGet]
        public async Task <IActionResult> Crear()
        {
            var cartas = await repositorioTiposPlatos.Obtener();
            var modelo = new CartaCreacionViewModel();
            modelo.TiposPlatos = await ObtenerTiposPlatos();
            return View(modelo);
        }
        [HttpPost]
        public async Task<IActionResult> Crear(CartaCreacionViewModel carta)
        {
            var tipoCarta = await repositorioTiposPlatos.ObtenerporId(carta.tipo_plato);
            var yaExistePlato = await repositorioCarta.Existe(carta.nombre_plato,carta.tipo_plato,carta.precio_plato,carta.descripcion);

            if (tipoCarta is null)
            {
                RedirectToAction("NoEncontrado", "Home");
            }
            if (!ModelState.IsValid)
            {
                carta.TiposPlatos = await ObtenerTiposPlatos();
                return View(carta);
            }
            if (yaExistePlato)
            {
                ModelState.AddModelError(nameof(carta.nombre_plato), $"El plato {carta.nombre_plato} ya existe");
                return View(carta);
            }
            await repositorioCarta.Crear(carta);
            return RedirectToAction("Index");

        }
        private async Task<IEnumerable<SelectListItem>> ObtenerTiposPlatos()
        {
            var tiposPlatos = await repositorioTiposPlatos.Obtener();
            return tiposPlatos.Select(x => new SelectListItem(x.categoria, x.id_categoria.ToString()));

        }
        [HttpGet]
        public async Task<IActionResult>VerificarExistePlato(string nombre_plato, int tipo_plato, int precio_plato, string descripcion)
        {
            var yaExistePlato = await repositorioCarta.Existe(nombre_plato,tipo_plato,precio_plato,descripcion);
            if (yaExistePlato)
            {
                return Json($"El plato {nombre_plato} ya existe");
            }
            return Json(true);
        }
        [HttpGet]
        public async Task<ActionResult> Editar(int id)
        {
            var carta = await repositorioCarta.ObtenerporId(id);
            if (carta is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            var modelo = mapper.Map<CartaCreacionViewModel>(carta);
            modelo.TiposPlatos = await ObtenerTiposPlatos();
            return View(modelo);   
        }
        [HttpPost]
        public async Task<IActionResult>Editar(CartaCreacionViewModel carta)
        {
            var platoExiste = await repositorioCarta.ObtenerporId(carta.id);
            if (platoExiste is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            await repositorioCarta.Actualizar(carta);
            return RedirectToAction("Index");
        }

        public async Task <IActionResult> Borrar(int id)
        {
            var carta = await repositorioCarta.ObtenerporId(id);
            try
            {
                
                if (carta is null)
                {
                    return RedirectToAction("NoEncontrado", "Home");
                }

                return View(carta);
            }
            catch (Exception ex)
            {
                if (ex.InnerException is SqlException sqlException && (sqlException.Number == 547 || sqlException.Number == 2601))
                {
                    // Manejar error de clave externa
                    ModelState.AddModelError(string.Empty, "No se puede eliminar este registro debido a que tiene transcacciones.");
                    return View(carta); // o redirigir a una vista con un mensaje de error
                }
                return RedirectToAction("Index");
            }
            
        }
        [HttpPost]
        public async Task<IActionResult> BorrarPlato(int id)
        {
            var carta = await repositorioCarta.ObtenerporId(id);
            if (carta is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            await repositorioCarta.Borrar(id);
            return RedirectToAction("Index");
        }
    }
}
