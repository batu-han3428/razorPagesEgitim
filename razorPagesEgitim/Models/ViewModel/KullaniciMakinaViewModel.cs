using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace razorPagesEgitim.Models.ViewModel
{
    public class KullaniciMakinaViewModel
    {
        public ApplicationUser KullaniciObj { get; set; }

        public IEnumerable<Makina> Makinalar { get; set; }

    }
}
