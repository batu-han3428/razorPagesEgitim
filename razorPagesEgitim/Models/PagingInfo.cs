using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace razorPagesEgitim.Models
{
    //burada ki proplara kullaniların index modelinde atama yapıyoruz
    public class PagingInfo
    {
        //kullanıcıların toplam sayısı
        public int TotalItems { get; set; }

        //her sayfada gösterilecek kullanıcı sayısı
        public int ItemsPerPage {get;set;}

        //güncel sayfa tutulacak
        public int CurrentPage { get; set; }

        //toplam sayfayı gösterecek
        public int TotalPage => (int)Math.Ceiling((decimal)TotalItems / ItemsPerPage);//üye sayısını, sayfada kaç kişi göstermek istiyorsak o sayıya bölüp yukarı yuvarladık

        //url adres
        public string UrlParam { get; set; }
    }
}
