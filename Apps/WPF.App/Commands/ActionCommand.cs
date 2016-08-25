using System;

namespace WPF.App.Commands
{
    internal class ActionCommand : CommandBase
    {
        private readonly Action _action;

        public ActionCommand(Action action)
        {
            _action = action;
        }

        public override void Execute(object parameter)
        {
            _action?.Invoke();
        }
    }
}