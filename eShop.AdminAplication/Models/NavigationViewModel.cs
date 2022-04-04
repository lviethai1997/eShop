using eShop.ViewModels.System.Languages;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShop.AdminAplication.Models
{
    public class NavigationViewModel
    {
        public List<LanguageViewModel> Languages { get; set; }
        public string CurrentLanguageId { get; set; }
    }
}
