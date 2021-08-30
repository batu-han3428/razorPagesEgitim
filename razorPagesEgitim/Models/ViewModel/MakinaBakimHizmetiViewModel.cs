using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace razorPagesEgitim.Models.ViewModel
{
    public class MakinaBakimHizmetiViewModel
    {
        public Makina Makina { get; set; }

        public BakimHizmetiGenel BakimHizmetiGenel { get; set; }

        public BakimHizmetiDetay BakimHizmetiDetay { get; set; }

        public List<bakimTipi> BakimTipleriListesi { get; set; }

        public List<BakimHizmetKart> BakimHizmetKart { get; set; }

    }
}
