using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace razorPagesEgitim.Models
{
    public class Makina
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string SN {get;set;}

        [Required]
        public string Marka { get; set; }

        [Required]
        public string Model { get; set; }

        public string MakinaTipi { get; set; }

        [Required]
        public int Yil { get; set; }

        [Required]
        public double MakinaSaatSayac { get; set; }

        public string EkAciklama { get; set; }

        public string KullaniciId { get; set; }

        [ForeignKey("KullaniciId")]
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
