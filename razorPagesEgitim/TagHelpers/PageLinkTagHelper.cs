using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using razorPagesEgitim.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace razorPagesEgitim.TagHelpers
{
    [HtmlTargetElement("div", Attributes = "page-model")]//tag helper yarattık. div için
    public class PageLinkTagHelper: TagHelper
    {
        private IUrlHelperFactory urlHelperFactory;
        public PageLinkTagHelper(IUrlHelperFactory helperFactory)
        {
            urlHelperFactory = helperFactory;
        }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        public PagingInfo PageModel { get; set; }

        //kullanıcı sayfaya tıkladığında gideceği yer
        public string PageAction { get; set; }

        //css kodlarını tutacak
        public string PageClass { get; set; }

        //alt tarafta sayfa no larında seçili olmayanların css i
        public string PageClassNormal { get; set; }

        //alt tarafta sayfa no larında seçili olanın css i
        public string PageClassSelected { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            IUrlHelper urlHelper = urlHelperFactory.GetUrlHelper(ViewContext);
            TagBuilder result = new TagBuilder("div");//div tagı oluşturduk

            for (int i = 1; i <= PageModel.TotalPage; i++)//1. sayfadan başlayacağı için i= 1 yaptık
            {
                TagBuilder tag = new TagBuilder("a");//a tagı oluşturduk. altta da özelliklerini atadık
                string url = PageModel.UrlParam.Replace(":", i.ToString());
                tag.Attributes["href"] = url;
                tag.AddCssClass(PageClass);
                tag.AddCssClass(i == PageModel.CurrentPage ? PageClassSelected : PageClassNormal);
                tag.InnerHtml.Append(i.ToString());
                result.InnerHtml.AppendHtml(tag);//div tagının içine a yı attık
            }

            output.Content.AppendHtml(result.InnerHtml);//div tagının output parametresiyle çıkısını sağladık
        }
    }
}
