using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ProyectoRestaurante.Models
{
    public class Carta
    {
        public int id { get; set; }
        [Required(ErrorMessage = "Debe ingresar el nombre del articulo")]
        [Remote(action: "VerificarExistePlato", controller: "Carta")]
        public string nombre_plato { get; set; }
        [Required(ErrorMessage = "Debe ingresar el tipo del articulo")]
        public int tipo_plato { get; set; }
        [Required(ErrorMessage = "Debe ingresar el precio del articulo")]
        public int precio_plato { get; set; }
        [Required(ErrorMessage = "Debe ingresar la descripción del articulo")]
        public string descripcion { get; set; }
        public string tipoPlato { get; set; }
    }
}
