using System.Collections.Generic;

namespace DiceRPG.Core.Models.SMT;

public class FichaNahobino : FichaSMTBase
{
    public string MagatsuhiHabilidade { get; set; }
    public bool PodeFundir { get; set; } = true;
    public List<string> HabilidadesUnicas { get; set; } = new();

    // Nahobino pode absorver habilidades de demônios
    public List<string> HabilidadesAbsorvidas { get; set; } = new();

    public FichaNahobino(string nome) : base(nome, "Nahobino") { }

    public override Dictionary<string, object> ObterAtributos()
    {
        var baseDict = base.ObterAtributos();
        baseDict["MagatsuhiHabilidade"] = MagatsuhiHabilidade;
        baseDict["PodeFundir"] = PodeFundir;
        baseDict["HabilidadesUnicas"] = HabilidadesUnicas;
        baseDict["HabilidadesAbsorvidas"] = HabilidadesAbsorvidas;
        return baseDict;
    }

    public override void CarregarAtributos(Dictionary<string, object> dados)
    {
        base.CarregarAtributos(dados);
        MagatsuhiHabilidade = dados["MagatsuhiHabilidade"].ToString();
        PodeFundir = Convert.ToBoolean(dados["PodeFundir"]);

        if (dados.ContainsKey("HabilidadesUnicas"))
            HabilidadesUnicas = (dados["HabilidadesUnicas"] as List<object>)?.ConvertAll(x => x.ToString()) ?? new();
        if (dados.ContainsKey("HabilidadesAbsorvidas"))
            HabilidadesAbsorvidas = (dados["HabilidadesAbsorvidas"] as List<object>)?.ConvertAll(x => x.ToString()) ?? new();
    }
}