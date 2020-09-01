using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.Web.Models
{
    public class MenuItemModel
    {
        public string MenuId { get; set; }        
        public string MenuText { get; set; }
        public string MenuUrl { get; set; }
        public string MenuToolTip { get; set; }
        public string MenuDescription { get; set; }
       
        public string IconUrl { get; set; }

        public MenuItemModel()
        {

        }

        public MenuItemModel(string menuId, string menuText, string menuUrl, string menuDescription)
        {
            MenuId = menuId;
            MenuUrl = menuUrl;
            MenuText = menuText;
            MenuDescription = menuDescription;
        }
    }
}
