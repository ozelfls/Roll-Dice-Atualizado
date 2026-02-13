using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using DiceRPG.Core.Models;
using DiceRPG.UI.Commands;

namespace DiceRPG.UI.ViewModels;

public class AbaDadosViewModel : AbaBaseViewModel
{
    private readonly Dado _dado;
    private int _tipoSelecionado = 20;
    private string _modoSelecionado = "Normal";
    private string _resultado = "";

    public override string NomeAba => "Dados";
    public override string Icone => "DiceMultiple";

    public ObservableCollection<int> TiposDado { get; } = new() { 4, 6, 8, 10, 12, 20, 100 };
    public ObservableCollection<string> ModosRolagem { get; } = new() { "Normal", "Vantagem", "Desvantagem" };
    public ObservableCollection<string> Historico { get; } = new();

    public int TipoSelecionado
    {
        get => _tipoSelecionado;
        set { _tipoSelecionado = value; OnPropertyChanged(); }
    }

    public string ModoSelecionado
    {
        get => _modoSelecionado;
        set { _modoSelecionado = value; OnPropertyChanged(); }
    }

    public string Resultado
    {
        get => _resultado;
        set { _resultado = value; OnPropertyChanged(); }
    }

    public ICommand RolarCommand { get; }
    public ICommand LimparHistoricoCommand { get; }

    public AbaDadosViewModel()
    {
        _dado = new Dado(20); // Inicialização padrão

        RolarCommand = new RelayCommand(async () => await ExecutarRolagem());
        LimparHistoricoCommand = new RelayCommand(() => Historico.Clear());
    }

    private async Task ExecutarRolagem()
    {
        await ExecutarComLoading(async () =>
        {
            await Task.Delay(100); // Simular processamento

            var dado = new Dado(TipoSelecionado);
            string resultadoTexto;

            switch (ModoSelecionado)
            {
                case "Vantagem":
                    var (resV, todosV) = dado.RolarComVantagem();
                    resultadoTexto = $"Vantagem: {resV} [{string.Join(",", todosV)}]";
                    break;
                case "Desvantagem":
                    var (resD, todosD) = dado.RolarComDesvantagem();
                    resultadoTexto = $"Desvantagem: {resD} [{string.Join(",", todosD)}]";
                    break;
                default:
                    resultadoTexto = $"Normal: {dado.Rolar()}";
                    break;
            }

            Resultado = resultadoTexto;

            Application.Current.Dispatcher.Invoke(() =>
            {
                Historico.Insert(0, $"{DateTime.Now:HH:mm:ss} - D{TipoSelecionado} - {resultadoTexto}");

                // Manter apenas últimos 50 resultados
                while (Historico.Count > 50)
                    Historico.RemoveAt(Historico.Count - 1);
            });
        });
    }
}