using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using DiceRPG.Core.Data;
using DiceRPG.Core.Models.Cyberpunk;
using DiceRPG.UI.Commands;

namespace DiceRPG.UI.ViewModels;

public class AbaCyberpunkViewModel : AbaBaseViewModel
{
    private readonly JsonDataService _dataService;
    private FichaCyberpunk? _fichaAtual;
    private string _fichaSelecionada = "";
    private string _novaFichaNome = "";

    public override string NomeAba => "Cyberpunk";
    public override string Icone => "Robot";

    public ObservableCollection<string> ListaFichas { get; } = new();

    public FichaCyberpunk? FichaAtual
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

    public ICommand NovaFichaCommand { get; }
    public ICommand SalvarFichaCommand { get; }
    public ICommand CarregarFichaCommand { get; }
    public ICommand DeletarFichaCommand { get; }

    public AbaCyberpunkViewModel(JsonDataService dataService)
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

            if (await _dataService.FichaExiste(NovaFichaNome, "Cyberpunk"))
            {
                StatusMessage = "Já existe uma ficha com este nome";
                return;
            }

            FichaAtual = new FichaCyberpunk(NovaFichaNome);
            await SalvarFichaAtual();
            NovaFichaNome = "";
            await CarregarListaFichas();
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
            var fichas = await _dataService.CarregarFichas<FichaCyberpunk>("Cyberpunk");
            FichaAtual = fichas.FirstOrDefault(f => f.Nome == FichaSelecionada);

            if (FichaAtual == null)
                StatusMessage = "Ficha não encontrada";
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
            var fichas = await _dataService.CarregarFichas<FichaCyberpunk>("Cyberpunk");

            Application.Current.Dispatcher.Invoke(() =>
            {
                ListaFichas.Clear();
                foreach (var f in fichas.OrderBy(f => f.Nome))
                    ListaFichas.Add(f.Nome);
            });
        });
    }
}