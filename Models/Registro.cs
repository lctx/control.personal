using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace control.personal.Models
{
    public class Registro
    {
        [Key]
        public int id { get; set; }
        public DateTime FechaHora { get; set; }
        public string idUsuario { get; set; }
        public virtual ControlIngreso ControlIngresos { get; set; }

        [ForeignKey("idUsuario")]
        public virtual Usuario Usuario { get; set; }
    }
}