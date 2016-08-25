using System.Collections.Generic;
using System.Windows.Controls;
using WPF.App.Helpers;
using WPF.App.Properties;
using WPF.App.Views;

namespace WPF.App.ViewModels
{
    public class MainViewModel
    {
        private readonly IMainView _view;

        public MainViewModel(IMainView view)
        {
            _view = view;
            MainMenuItems = GetMainMenuItems();
        }

        private List<MenuItem> GetMainMenuItems()
        {
            var items = new List<MenuItem>
            {
                UIHelper.CreateMenuItem(Resources.Title_File, UIHelper.CreateActionMenuItem(Resources.Title_Exit, ()=> _view.Close()))
            };
            return items;
        }

        public List<MenuItem> MainMenuItems { get; set; }
    }
}