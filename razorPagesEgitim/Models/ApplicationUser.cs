using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace razorPagesEgitim.Models
{
    public class ApplicationUser: IdentityUser
    {
        [Required(ErrorMessage = "Bu alan boş geçilemez..")]
        public string adSoyad { get; set; }
        [Required(ErrorMessage = "Bu alan boş geçilemez..")]
        public string adres { get; set; }
        [Required(ErrorMessage = "Bu alan boş geçilemez..")]
        public string sehir { get; set; }
        public string postaKodu { get; set; }

    }
}
