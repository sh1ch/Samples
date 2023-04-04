using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ContentRenderedSample;

public class MainWindowViewModel
{
    public ICommand ContentRenderedCommand { get; }

    public MainWindowViewModel()
    {
        ContentRenderedCommand = new Command(() =>
        {
            MessageBox.Show($"{ContentRenderedCommand} is executed.");
        });
    }
}

public class Command : ICommand
{
    private Action _Action;
    public bool CanExecute(object? parameter) => true;
    public event EventHandler? CanExecuteChanged;

    public Command(Action action)
    {
        _Action = action;
    }

    public void Execute(object? parameter)
    {
        _Action?.Invoke();
    }
}
