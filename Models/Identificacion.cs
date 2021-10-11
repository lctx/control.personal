using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using control.personal.Utils;
namespace control.personal.Models
{
    public class Identificacion
    {
        [Key]
        public int id { get; set; }
        public int idUsuario { get; set; }
        public string Uid { get; set; }
        public int Estado { get; set; }
        public TipoRFID Tipo { get; set; }
        [ForeignKey("idUsuario")]
        public Usuario Usuario { get; set; }
    }
}