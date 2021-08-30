using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace razorPagesEgitim.Models
{
    public class BakimHizmetiGenel
    {
        public int Id { get; set; }
        public double MakinaSayacSaat { get; set; }

        [Required]
        public double ToplamFiyat { get; set; }

        public string Detaylar { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:dd MMM Y}")]

        public DateTime EklendigiTarih { get; set; }

        public int MakinaId { get; set; }

        [ForeignKey("MakinaId")]
        public virtual Makina Makina { get; set; }

    }
}
