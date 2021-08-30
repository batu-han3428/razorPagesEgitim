using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace razorPagesEgitim.Models
{
    public class BakimHizmetKart
    {
        public int Id { get; set; }
        public int MakinaId { get; set; }
        public int BakimTipiId { get; set; }

        [ForeignKey("MakinaId")]
        public virtual Makina Makina { get; set; }

        [ForeignKey("BakimTipiId")]
        public virtual bakimTipi BakimTipi { get; set; }

    }
}
