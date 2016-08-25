using System;
using System.Windows.Controls;
using WPF.App.Commands;

namespace WPF.App.Helpers
{
    public static class UIHelper
    {
        public static MenuItem CreateActionMenuItem(string header, Action action)
        {
            return new MenuItem
            {
                Header = header,
                Command = new ActionCommand(action)
            };
        }

        public static MenuItem CreateMenuItem(string header, params MenuItem[] submenuItems)
        {
            var menuItem = new MenuItem
            {
                Header = header
            };
            foreach (var submenuItem in submenuItems)
                menuItem.Items.Add(submenuItem);
            return menuItem;
        }
    }
}