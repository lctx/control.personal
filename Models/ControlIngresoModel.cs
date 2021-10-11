using System.Collections.Generic;
using control.personal.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace control.personal.Models
{
    //usar esto si se requiere un modelo mas complejo
    public class ControlIngresoModel
    {
        public ControlIngreso controlIngreso { get; set; }
        public IEnumerable<ControlIngreso> controlIngresoList { get; set; }

        public IEnumerable<Registro> registrosList { get; set; }
        public ControlIngresoModel()
        {
        }
    }
}