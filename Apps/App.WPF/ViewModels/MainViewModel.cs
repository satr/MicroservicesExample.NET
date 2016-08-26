using System.Collections.Generic;
using System.Windows.Controls;
using App.WPF.Helpers;
using App.WPF.Properties;
using App.WPF.Views;

namespace App.WPF.ViewModels
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