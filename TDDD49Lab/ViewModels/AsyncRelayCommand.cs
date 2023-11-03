using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace TDDD49Lab.ViewModels
{
    class AsyncRelayCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;


        private readonly Action<Exception> _onException;

        private Func<object, Task> _execute { get; set; }
        private Predicate<object> _canExcute { get; set; }


        public AsyncRelayCommand(Func<object, Task> ExcuteMethod, Predicate<object> CanExcuteMethod, Action<Exception> onException)
        {

            _execute = ExcuteMethod;
            _canExcute = CanExcuteMethod;
            _onException = onException;
        }





        public bool CanExecute(object? parameter)
        {
            return _canExcute(parameter);
        }

        public async void Execute(object? parameter)
        {
            
            try
            {
                await executeAsync(parameter);
            }
            catch (Exception ex)
            {
               
                _onException?.Invoke(ex);
            }
            
        }


        private async Task executeAsync(object parameter)
        {
            await _execute(parameter);
        }


        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
