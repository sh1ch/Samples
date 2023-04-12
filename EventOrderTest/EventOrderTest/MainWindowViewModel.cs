using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EventOrderTest;

public class MainWindowViewModel
{
    private static readonly NLog.Logger _Logger = NLog.LogManager.GetCurrentClassLogger();

    public ICommand InitializedCommand { get; }
    public ICommand LoadedCommand { get; }
    public ICommand ContentRenderedCommand { get; }

    /// <summary>
    /// <see cref="MainWindowViewModel"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    public MainWindowViewModel()
    {
        _Logger.Info("MainWindow VM constructor called.");

        InitializedCommand = new Command(() =>
        {
            _Logger.Info("Initialized event called.");
        });

        LoadedCommand = new Command(() =>
        {
            _Logger.Info("Loaded event called.");
        });

        ContentRenderedCommand = new Command(() =>
        {
            _Logger.Info("ContentRendered event called.");
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
