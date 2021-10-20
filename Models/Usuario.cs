using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace control.personal.Models
{
    // IdentityUser<int> cambia la primary key de guid a int
    //crear indice con validación unica de la cedula
    [Index(nameof(Cedula),IsUnique =true)]
    [Table("AspNetUsers")]
    public class Usuario : IdentityUser
    {
        [StringLength(10)]
        public string Cedula { get; set; }
        public string Nombre { get; set; }
        public virtual List<Identificacion> Identificaciones { get; set; }
        public virtual List<Registro> Registros { get; set; }
    }
}