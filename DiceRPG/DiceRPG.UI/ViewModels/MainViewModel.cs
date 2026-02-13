using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;

namespace DiceRPG.UI.ViewModels;

public class MainViewModel : INotifyPropertyChanged
{
    private readonly IServiceProvider _services;
    private bool _isDarkTheme = true;
    private int _selectedTabIndex;
    private string _statusMessage = "Pronto";

    public ObservableCollection<AbaBaseViewModel> Abas { get; } = new();

    public bool IsDarkTheme
    {
        get => _isDarkTheme;
        set { _isDarkTheme = value; OnPropertyChanged(); }
    }

    public int SelectedTabIndex
    {
        get => _selectedTabIndex;
        set { _selectedTabIndex = value; OnPropertyChanged(); }
    }

    public string StatusMessage
    {
        get => _statusMessage;
        set { _statusMessage = value; OnPropertyChanged(); }
    }

    public MainViewModel(IServiceProvider services)
    {
        _services = services;

        // Criar abas na ordem desejada
        Abas.Add(services.GetService<AbaDadosViewModel>());
        Abas.Add(services.GetService<AbaCyberpunkViewModel>());
        Abas.Add(services.GetService<AbaWarhammerViewModel>());
        Abas.Add(services.GetService<AbaSMTViewModel>());

        // Assinar eventos de mudança de status
        foreach (var aba in Abas)
        {
            aba.StatusMessageChanged += (s, msg) => StatusMessage = msg;
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}