﻿using System.Windows.Input;

namespace WorldCupWPF.Commands;

//public class RelayCommand(Action execute) : ICommand
//{
//    public event EventHandler? CanExecuteChanged;

//    public bool CanExecute(object? parameter) => true;

//    public void Execute(object? parameter) => execute();
//}

public class RelayCommand(Action execute,
                          Func<bool>? canExecute = null) : ICommand
{
    public event EventHandler? CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }

    public bool CanExecute(object? parameter) => canExecute?.Invoke() ?? true;

    public void Execute(object? parameter) => execute();
}
