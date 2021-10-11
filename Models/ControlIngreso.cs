using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace control.personal.Models
{
    public class ControlIngreso
    {
        [Key]
        public int id { get; set; }
        [Display(Name = "Uniforme Limpio")]
        public bool UniformeLimpio { get; set; }
        [Display(Name = "Uniforme Completo")]
        public bool UniformeCompleto { get; set; }
        [Display(Name = "Uniforme Correcto")]
        public bool UniformeColocadoBien { get; set; }
        [Display(Name = "Manos Limpias")]
        public bool ManoLimpias { get; set; }
        [Display(Name = "Sin Accesorios")]
        public bool ManoSinAccesorios { get; set; }
        [Display(Name = "Observaciónes")]
        public string Observaciones { get; set; }
        [Display(Name = "Responsable")]
        public string Responsable { get; set; }
        public int IdRegistro { get; set; }
        
        [ForeignKey("IdRegistro")]
        public virtual Registro Registro { get; set; }
    }
}