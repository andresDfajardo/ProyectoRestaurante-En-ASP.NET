using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using ProyectoRestaurante.Models;
using ProyectoRestaurante.Servicios;

namespace ProyectoRestaurante.Controllers
{
    public class EmpleadosController:Controller
    {
        private readonly IRepositorioEmpleados repositorioEmpleados;
        public EmpleadosController(IRepositorioEmpleados repositorioEmpleados)
        {
            this.repositorioEmpleados = repositorioEmpleados;
        }
        [HttpGet]
        public async Task<IActionResult> TodosEmpleados()
        {
            var empleados = await repositorioEmpleados.ObtenerTodos();
            return View(empleados);
        }
        public async Task<IActionResult> Index()
        {
            var empleados = await repositorioEmpleados.Obtener();
            return View(empleados);
        }
        
        public IActionResult Crear()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Crear(Empleados empleados)
        {
            if (!ModelState.IsValid)
            {
                return View(empleados);
            }
            var yaExisteEmpleado = await repositorioEmpleados.Existe(empleados.doc_empleado);
            if (yaExisteEmpleado)
            {
                ModelState.AddModelError(nameof(empleados.doc_empleado), $"El documento numero {empleados.doc_empleado} ya existe.");
                return View(empleados);
            }
            await repositorioEmpleados.Crear(empleados);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task <IActionResult> VerificarExisteEmpleado(string doc_empleado)
        {
            var yaExisteEmpleado = await repositorioEmpleados.Existe(doc_empleado);
            if (yaExisteEmpleado)
            {
                return Json($"El empleado con documento {doc_empleado} ya existe.");
            }
            return Json(true);
        }
        [HttpGet]
        public async Task<ActionResult>Editar(string doc_empleado)
        {
            var empleado = await repositorioEmpleados.ObtenerporDoc(doc_empleado);
            if (empleado is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            return View(empleado);
        }
        [HttpPost]
        public async Task<IActionResult> Editar(Empleados empleados)
        {
            var empleadoExiste = await repositorioEmpleados.ObtenerporDoc(empleados.doc_empleado);
            if (empleadoExiste is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            await repositorioEmpleados.Actualizar(empleados);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<ActionResult> Desactivar(string doc_empleado)
        {
            var empleado = await repositorioEmpleados.ObtenerporDoc(doc_empleado);
            if (empleado is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            return View(empleado);
        }
        [HttpPost]
        public async Task<IActionResult>DesactivarEmpleado(Empleados empleados)
        {
            var empleadoExiste = await repositorioEmpleados.ObtenerporDoc(empleados.doc_empleado);
            if (empleadoExiste is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            await repositorioEmpleados.Desactivar(empleados);
            return RedirectToAction("Index");
        }


    }
}
