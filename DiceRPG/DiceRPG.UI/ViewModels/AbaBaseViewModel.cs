using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DiceRPG.UI.ViewModels;

public abstract class AbaBaseViewModel : INotifyPropertyChanged
{
    public abstract string NomeAba { get; }
    public abstract string Icone { get; }

    private bool _isLoading;
    private string _statusMessage = "";

    public bool IsLoading
    {
        get => _isLoading;
        set { _isLoading = value; OnPropertyChanged(); }
    }

    public string StatusMessage
    {
        get => _statusMessage;
        set
        {
            _statusMessage = value;
            OnPropertyChanged();
            StatusMessageChanged?.Invoke(this, value);
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    public event EventHandler<string>? StatusMessageChanged;

    protected void OnPropertyChanged([CallerMemberName] string? name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    protected async Task ExecutarComLoading(Func<Task> acao)
    {
        try
        {
            IsLoading = true;
            StatusMessage = "Processando...";
            await acao();
            StatusMessage = "Pronto";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Erro: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }
}