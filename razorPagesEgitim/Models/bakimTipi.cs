using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace razorPagesEgitim.Models
{
    public class bakimTipi
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="Bu alan boş geçilemez..")]
        public string BakimAdi { get; set; } 

        [Required(ErrorMessage = "Bu alan boş geçilemez..")]
        public double BakimFiyati { get; set; }
    }
}
