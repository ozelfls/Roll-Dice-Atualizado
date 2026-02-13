using System.Collections.Generic;

namespace DiceRPG.Core.Models.SMT;

public class FichaDemiFiend : FichaSMTBase
{
    public int PoderDoLuto { get; set; }
    public List<string> Magatama { get; set; } = new();
    public string MagatamaAtual { get; set; }

    // Demi-fiend pode aprender habilidades das magatamas
    public List<string> HabilidadesAprendidas { get; set; } = new();

    public FichaDemiFiend(string nome) : base(nome, "DemiFiend") { }

    public override Dictionary<string, object> ObterAtributos()
    {
        var baseDict = base.ObterAtributos();
        baseDict["PoderDoLuto"] = PoderDoLuto;
        baseDict["Magatama"] = Magatama;
        baseDict["MagatamaAtual"] = MagatamaAtual;
        baseDict["HabilidadesAprendidas"] = HabilidadesAprendidas;
        return baseDict;
    }

    public override void CarregarAtributos(Dictionary<string, object> dados)
    {
        base.CarregarAtributos(dados);
        PoderDoLuto = Convert.ToInt32(dados["PoderDoLuto"]);
        MagatamaAtual = dados["MagatamaAtual"].ToString();

        if (dados.ContainsKey("Magatama"))
            Magatama = (dados["Magatama"] as List<object>)?.ConvertAll(x => x.ToString()) ?? new();
        if (dados.ContainsKey("HabilidadesAprendidas"))
            HabilidadesAprendidas = (dados["HabilidadesAprendidas"] as List<object>)?.ConvertAll(x => x.ToString()) ?? new();
    }
}