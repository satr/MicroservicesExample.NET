﻿using System;
using System.Windows.Input;

namespace WPF.App.Commands
{
    internal abstract class CommandBase: ICommand
    {
        public virtual bool CanExecute(object parameter)
        {
            return true;
        }

        public abstract void Execute(object parameter);

        public event EventHandler CanExecuteChanged;
    }
}