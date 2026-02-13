using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using DiceRPG.Core.Data;
using DiceRPG.Core.Models.SMT;
using DiceRPG.UI.Commands;

namespace DiceRPG.UI.ViewModels;

public class AbaSMTViewModel : AbaBaseViewModel
{
    private readonly JsonDataService _dataService;
    private FichaSMTBase? _fichaAtual;
    private string _fichaSelecionada = "";
    private string _novaFichaNome = "";
    private string _subtipoSelecionado = "Nahobino";

    public override string NomeAba => "Shin Megami Tensei";
    public override string Icone => "Ghost";

    public ObservableCollection<string> ListaFichas { get; } = new();
    public ObservableCollection<string> Subtipos { get; } = new() { "Nahobino", "DemiFiend", "Samurai", "PersonaUser" };

    public FichaSMTBase? FichaAtual
    {
        get => _fichaAtual;
        set { _fichaAtual = value; OnPropertyChanged(); OnPropertyChanged(nameof(TemFicha)); }
    }

    public bool TemFicha => FichaAtual != null;

    public string FichaSelecionada
    {
        get => _fichaSelecionada;
        set { _fichaSelecionada = value; OnPropertyChanged(); }
    }

    public string NovaFichaNome
    {
        get => _novaFichaNome;
        set { _novaFichaNome = value; OnPropertyChanged(); }
    }

    public string SubtipoSelecionado
    {
        get => _subtipoSelecionado;
        set { _subtipoSelecionado = value; OnPropertyChanged(); }
    }

    public ICommand NovaFichaCommand { get; }
    public ICommand SalvarFichaCommand { get; }
    public ICommand CarregarFichaCommand { get; }
    public ICommand DeletarFichaCommand { get; }

    public AbaSMTViewModel(JsonDataService dataService)
    {
        _dataService = dataService;

        NovaFichaCommand = new RelayCommand(async () => await CriarNovaFicha());
        SalvarFichaCommand = new RelayCommand(async () => await SalvarFichaAtual(), () => TemFicha);
        CarregarFichaCommand = new RelayCommand(async () => await CarregarFichaSelecionada());
        DeletarFichaCommand = new RelayCommand(async () => await DeletarFichaSelecionada(), () => TemFicha);

        _ = CarregarListaFichas();
    }

    private async Task CriarNovaFicha()
    {
        await ExecutarComLoading(async () =>
        {
            if (string.IsNullOrWhiteSpace(NovaFichaNome))
            {
                StatusMessage = "Digite um nome para a ficha";
                return;
            }

            var sistemaCompleto = $"SMT-{SubtipoSelecionado}";
            if (await _dataService.FichaExiste(NovaFichaNome, sistemaCompleto))
            {
                StatusMessage = "Já existe uma ficha com este nome";
                return;
            }

            FichaAtual = SubtipoSelecionado switch
            {
                "Nahobino" => new FichaNahobino(NovaFichaNome),
                "DemiFiend" => new FichaDemiFiend(NovaFichaNome),
                "Samurai" => new FichaSamurai(NovaFichaNome),
                "PersonaUser" => new FichaPersonaUser(NovaFichaNome),
                _ => null
            };

            if (FichaAtual != null)
            {
                await SalvarFichaAtual();
                NovaFichaNome = "";
                await CarregarListaFichas();
            }
        });
    }

    private async Task SalvarFichaAtual()
    {
        if (FichaAtual == null) return;

        await ExecutarComLoading(async () =>
        {
            await _dataService.SalvarFicha(FichaAtual);
            StatusMessage = "Ficha salva com sucesso";
            await CarregarListaFichas();
        });
    }

    private async Task CarregarFichaSelecionada()
    {
        if (string.IsNullOrWhiteSpace(FichaSelecionada)) return;

        await ExecutarComLoading(async () =>
        {
            var todasFichas = await _dataService.CarregarTodasFichas();
            FichaAtual = todasFichas
                .OfType<FichaSMTBase>()
                .FirstOrDefault(f => f.Nome == FichaSelecionada);

            if (FichaAtual == null)
                StatusMessage = "Ficha não encontrada";
            else
                SubtipoSelecionado = FichaAtual.Sistema.Replace("SMT-", "");
        });
    }

    private async Task DeletarFichaSelecionada()
    {
        if (FichaAtual == null) return;

        await ExecutarComLoading(async () =>
        {
            _dataService.DeletarFicha(FichaAtual.Id);
            FichaAtual = null;
            await CarregarListaFichas();
            StatusMessage = "Ficha deletada";
        });
    }

    private async Task CarregarListaFichas()
    {
        await ExecutarComLoading(async () =>
        {
            var todasFichas = await _dataService.CarregarTodasFichas();
            var fichasSMT = todasFichas.OfType<FichaSMTBase>().ToList();

            Application.Current.Dispatcher.Invoke(() =>
            {
                ListaFichas.Clear();
                foreach (var f in fichasSMT.OrderBy(f => f.Nome))
                    ListaFichas.Add(f.Nome);
            });
        });
    }
}