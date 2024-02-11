using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ProyectoRestaurante.Models
{
    public class Mesas
    {
        public int id_mesa { get; set; }
        [Required(ErrorMessage = "Debe ingresar el numero de la mesa")]
        [Remote(action: "VerificarExisteMesa", controller: "Mesas")]
        public int num_mesa { get; set; }
        
        public string estado_mesa { get; set; }
        [Required(ErrorMessage = "Debe ingresar la capacidad en personas de la mesa")]
        public int capacidad_mesa { get; set; }
    }
}
