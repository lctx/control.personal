using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using control.personal.Utils;
namespace control.personal.Models
{
    public class Identificacion
    {
        [Key]
        public int id { get; set; }
        [Display(Name = "Usuario")]
        public string idUsuario { get; set; }
        [Required]
        public string Uid { get; set; }
        //si da problemas hacer una migración
        public EstadoIdentificacion Estado { get; set; }
        public TipoRFID Tipo { get; set; }
        [ForeignKey("idUsuario")]
        public Usuario Usuario { get; set; }
    }
}