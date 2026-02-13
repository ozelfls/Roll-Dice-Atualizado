using System;
using System.Collections.Generic;

namespace DiceRPG.Core.Models.Cyberpunk;

public class FichaCyberpunk : FichaBase
{
    // Atributos principais
    public int Reflexos { get; set; }
    public int Inteligencia { get; set; }
    public int Tecnica { get; set; }
    public int Cool { get; set; }
    public int Atraentes { get; set; }
    public int Vontade { get; set; }
    public int Movimento { get; set; }
    public int Corporal { get; set; }
    public int Empatia { get; set; }

    // Rolos de vida e ferimentos
    public int PontosDeVida { get; set; }
    public int FerimentosLeves { get; set; }
    public int FerimentosGraves { get; set; }
    public int Mortal { get; set; }

    // Habilidades e equipamento
    public List<string> Habilidades { get; set; } = new();
    public List<string> Cyberware { get; set; } = new();
    public List<string> Armas { get; set; } = new();
    public List<string> Equipamento { get; set; } = new();

    // Dinheiro e reputação
    public int DinheiroEletronico { get; set; }
    public int DinheiroReal { get; set; }
    public int Reputacao { get; set; }

    public FichaCyberpunk(string nome) : base(nome, "Cyberpunk") { }

    public override Dictionary<string, object> ObterAtributos()
    {
        return new Dictionary<string, object>
        {
            ["Reflexos"] = Reflexos,
            ["Inteligencia"] = Inteligencia,
            ["Tecnica"] = Tecnica,
            ["Cool"] = Cool,
            ["Atraentes"] = Atraentes,
            ["Vontade"] = Vontade,
            ["Movimento"] = Movimento,
            ["Corporal"] = Corporal,
            ["Empatia"] = Empatia,
            ["PontosDeVida"] = PontosDeVida,
            ["Habilidades"] = Habilidades,
            ["Cyberware"] = Cyberware,
            ["Armas"] = Armas,
            ["Equipamento"] = Equipamento,
            ["DinheiroEletronico"] = DinheiroEletronico,
            ["DinheiroReal"] = DinheiroReal,
            ["Reputacao"] = Reputacao
        };
    }

    public override void CarregarAtributos(Dictionary<string, object> dados)
    {
        Reflexos = Convert.ToInt32(dados["Reflexos"]);
        Inteligencia = Convert.ToInt32(dados["Inteligencia"]);
        Tecnica = Convert.ToInt32(dados["Tecnica"]);
        Cool = Convert.ToInt32(dados["Cool"]);
        Atraentes = Convert.ToInt32(dados["Atraentes"]);
        Vontade = Convert.ToInt32(dados["Vontade"]);
        Movimento = Convert.ToInt32(dados["Movimento"]);
        Corporal = Convert.ToInt32(dados["Corporal"]);
        Empatia = Convert.ToInt32(dados["Empatia"]);
        PontosDeVida = Convert.ToInt32(dados["PontosDeVida"]);
        DinheiroEletronico = Convert.ToInt32(dados["DinheiroEletronico"]);
        DinheiroReal = Convert.ToInt32(dados["DinheiroReal"]);
        Reputacao = Convert.ToInt32(dados["Reputacao"]);

        if (dados.ContainsKey("Habilidades"))
            Habilidades = (dados["Habilidades"] as List<object>)?.ConvertAll(x => x.ToString()) ?? new();
        if (dados.ContainsKey("Cyberware"))
            Cyberware = (dados["Cyberware"] as List<object>)?.ConvertAll(x => x.ToString()) ?? new();
        if (dados.ContainsKey("Armas"))
            Armas = (dados["Armas"] as List<object>)?.ConvertAll(x => x.ToString()) ?? new();
        if (dados.ContainsKey("Equipamento"))
            Equipamento = (dados["Equipamento"] as List<object>)?.ConvertAll(x => x.ToString()) ?? new();
    }
}