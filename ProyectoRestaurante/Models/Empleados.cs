using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ProyectoRestaurante.Models
{
    public class Empleados
    {
        public int id_empleado { get; set; }
        [Required(ErrorMessage = "Debe ingresar el tipo del documento del empleado")]
        public string tipodoc_empleado { get; set; }
        [Required(ErrorMessage = "Debe ingresar el documento del empleado")]
        [Remote(action: "VerificarExisteEmpleado", controller: "Empleados")]
        public string doc_empleado { get; set; }
        [Required(ErrorMessage = "Debe ingresar el cargo del empleado")]
        public string cargo_empleado { get; set; }
        [Required(ErrorMessage = "Debe ingresar el primer nombre del empleado")]
        public string primer_nombre { get; set; }
        public string segundo_nombre { get; set; }
        [Required(ErrorMessage = "Debe ingresar el primer apellido del empleado")]
        public string primer_apellido { get; set; }
        public string segundo_apellido { get; set; }
        [Required(ErrorMessage = "Debe ingresar la contraseña del empleado")]
        public string password { get; set; }
        [Required(ErrorMessage = "Debe ingresar la fecha de ingreso del empleado")]
        [DataType(DataType.Date)]
        public DateTime fecha_ingreso { get; set; }
        public string estado_empleado { get; set; }
        [DataType(DataType.Date)]
        public string fecha_egreso { get; set; }
    }
}
