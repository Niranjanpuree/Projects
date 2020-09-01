using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Northwind.Core.Utilities;
using Northwind.Core.Entities;

namespace Northwind.Web.Helpers
{
    public class ESSMenuTagHelper: TagHelper
    {
        public TreeNode<UserInterfaceMenu> MenuData { get; set; }


        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.Attributes.Add("class", "navbar-collapse collapse d-lg-inline-flex flex-lg-row");
            output.Content.AppendHtml(@"<ul class=""navbar-nav flex-grow-1"">");            
            foreach(var rootChild in MenuData.Children)
            {
                output.Content.AppendHtml("<li class=\"nav-item dropdown menu-large\" id=\"navbar-contracts\">");
                output.Content.AppendHtml(@"<a class=""nav-link dropdown-toggle"" href=""#"" id=""navbarDropdown"" role=""button"" data-toggle=""dropdown"" aria-haspopup=""true"" aria-expanded=""false"">" + rootChild.Data.MenuText + "</a>");

                output.Content.AppendHtml(@"<div class=""dropdown-menu megamenu"" aria-labelledby=""navbarDropdown"">");
                output.Content.AppendHtml(@"<div class="""">");
                // output.Content.AppendHtml($"<h5>{rootChild.Data.MenuText}</h5>");

                var counter = 0;
          
                foreach (var firstLevelChild in rootChild.Children)
                {
                    if (counter % 4 == 0)
                    {
                        
                        output.Content.AppendHtml(@"<div class=""row"">");
                    }
                    counter = counter + 1;
                    output.Content.AppendHtml(@"<div class=""col-lg-3"">"); 
                    output.Content.AppendHtml($"<a href=\"{firstLevelChild.Data.MenuUrl}\" class=\"megamenu-firstchild\">{firstLevelChild.Data.MenuText}</a>");
                   
                    foreach (var secondLevelChild in firstLevelChild.Children)
                    {
                        //output.Content.AppendHtml($"<div class=\"pl-3\" style=\"font-size:12px\"><a href=\"{secondLevelChild.Data.MenuUrl}\">{secondLevelChild.Data.MenuText}</a></div>");
                        output.Content.AppendHtml($"<div class=\"pl-3\"><a href=\"{secondLevelChild.Data.MenuUrl}\">{secondLevelChild.Data.MenuText}</a></div>");
                       

                    }
                    if (counter % 4 == 0)
                    {
                        output.Content.AppendHtml(@"</div>");
                    }
                  
                    output.Content.AppendHtml(@"</div>");
                }
                
                
                output.Content.AppendHtml(@"</div>");
                output.Content.AppendHtml(@"</div>");
                output.Content.AppendHtml(@"</li>");
            }
            output.Content.AppendHtml(@"</ul>"); 

        }

    }
}
