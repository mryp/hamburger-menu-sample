using Prism.Windows.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HamburgerMenuPrism.ViewModels
{
    public class BasicPageViewModel : ViewModelBase
    {
        public BasicPageViewModel()
        {
            DisplayText = "This is the main page!";
        }

        public string DisplayText { get; private set; }
    }
}
