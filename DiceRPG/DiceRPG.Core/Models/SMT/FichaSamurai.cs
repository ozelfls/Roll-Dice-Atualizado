using System.Collections.Generic;

namespace DiceRPG.Core.Models.SMT;

public class FichaSamurai : FichaSMTBase
{
    public string Cla { get; set; }
    public string Karma { get; set; } // Law/Chaos/Neutral
    public int Honra { get; set; }
    public string KatanaPrincipal { get; set; }
    public List<string> Katanas { get; set; } = new();

    // Samurai tem habilidades especiais baseadas no clã
    public string HabilidadeCla { get; set; }

    public FichaSamurai(string nome) : base(nome, "Samurai") { }

    public override Dictionary<string, object> ObterAtributos()
    {
        var baseDict = base.ObterAtributos();
        baseDict["Cla"] = Cla;
        baseDict["Karma"] = Karma;
        baseDict["Honra"] = Honra;
        baseDict["KatanaPrincipal"] = KatanaPrincipal;
        baseDict["Katanas"] = Katanas;
        baseDict["HabilidadeCla"] = HabilidadeCla;
        return baseDict;
    }

    public override void CarregarAtributos(Dictionary<string, object> dados)
    {
        base.CarregarAtributos(dados);
        Cla = dados["Cla"].ToString();
        Karma = dados["Karma"].ToString();
        Honra = Convert.ToInt32(dados["Honra"]);
        KatanaPrincipal = dados["KatanaPrincipal"].ToString();
        HabilidadeCla = dados["HabilidadeCla"].ToString();

        if (dados.ContainsKey("Katanas"))
            Katanas = (dados["Katanas"] as List<object>)?.ConvertAll(x => x.ToString()) ?? new();
    }
}