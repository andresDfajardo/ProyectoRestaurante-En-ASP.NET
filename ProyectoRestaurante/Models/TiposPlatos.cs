using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ProyectoRestaurante.Models
{
    public class TiposPlatos
    {
        public int id_categoria { get; set; }
        //Validaciones que se mostrarán en la vista
        [Required(ErrorMessage ="Debe ingresar la categoria del plato.")]
        [Remote(action: "VerificarExisteCategoriaPlato", controller: "TiposPlatos")]
        public string categoria { get; set; }
    }
}
