using System;
using System.Collections.Generic;

namespace DiceRPG.Core.Models.Warhammer;

public class FichaWarhammer : FichaBase
{
    // Atributos primários
    public int CL { get; set; }  // Combate
    public int CC { get; set; }  // Combate Corpo a Corpo
    public int CT { get; set; }  // Combate à Distância
    public int F { get; set; }   // Força
    public int R { get; set; }   // Resistência
    public int AG { get; set; }  // Agilidade
    public int IN { get; set; }  // Inteligência
    public int WP { get; set; }  // Força de Vontade
    public int EM { get; set; }  // Empatia

    // Atributos secundários
    public int A { get; set; }   // Ataques
    public int P { get; set; }   // Progressão
    public int Feridas { get; set; }
    public int PM { get; set; }  // Pontos de Magia
    public int Destino { get; set; }

    // Carreira e experiência
    public string Raca { get; set; }
    public string Profissao { get; set; }
    public string Carreira { get; set; }
    public int ExperienciaAtual { get; set; }
    public int ExperienciaGasta { get; set; }
    public int ExperienciaTotal => ExperienciaAtual + ExperienciaGasta;

    // Habilidades e talentos
    public List<string> Habilidades { get; set; } = new();
    public List<string> Talentos { get; set; } = new();
    public List<string> Magias { get; set; } = new();
    public List<string> Equipamento { get; set; } = new();

    public FichaWarhammer(string nome) : base(nome, "Warhammer") { }

    public override Dictionary<string, object> ObterAtributos()
    {
        return new Dictionary<string, object>
        {
            ["CL"] = CL,
            ["CC"] = CC,
            ["CT"] = CT,
            ["F"] = F,
            ["R"] = R,
            ["AG"] = AG,
            ["IN"] = IN,
            ["WP"] = WP,
            ["EM"] = EM,
            ["A"] = A,
            ["P"] = P,
            ["Feridas"] = Feridas,
            ["PM"] = PM,
            ["Destino"] = Destino,
            ["Raca"] = Raca,
            ["Profissao"] = Profissao,
            ["Carreira"] = Carreira,
            ["ExperienciaAtual"] = ExperienciaAtual,
            ["ExperienciaGasta"] = ExperienciaGasta,
            ["Habilidades"] = Habilidades,
            ["Talentos"] = Talentos,
            ["Magias"] = Magias,
            ["Equipamento"] = Equipamento
        };
    }

    public override void CarregarAtributos(Dictionary<string, object> dados)
    {
        CL = Convert.ToInt32(dados["CL"]);
        CC = Convert.ToInt32(dados["CC"]);
        CT = Convert.ToInt32(dados["CT"]);
        F = Convert.ToInt32(dados["F"]);
        R = Convert.ToInt32(dados["R"]);
        AG = Convert.ToInt32(dados["AG"]);
        IN = Convert.ToInt32(dados["IN"]);
        WP = Convert.ToInt32(dados["WP"]);
        EM = Convert.ToInt32(dados["EM"]);
        A = Convert.ToInt32(dados["A"]);
        P = Convert.ToInt32(dados["P"]);
        Feridas = Convert.ToInt32(dados["Feridas"]);
        PM = Convert.ToInt32(dados["PM"]);
        Destino = Convert.ToInt32(dados["Destino"]);
        Raca = dados["Raca"].ToString();
        Profissao = dados["Profissao"].ToString();
        Carreira = dados["Carreira"].ToString();
        ExperienciaAtual = Convert.ToInt32(dados["ExperienciaAtual"]);
        ExperienciaGasta = Convert.ToInt32(dados["ExperienciaGasta"]);

        if (dados.ContainsKey("Habilidades"))
            Habilidades = (dados["Habilidades"] as List<object>)?.ConvertAll(x => x.ToString()) ?? new();
        if (dados.ContainsKey("Talentos"))
            Talentos = (dados["Talentos"] as List<object>)?.ConvertAll(x => x.ToString()) ?? new();
        if (dados.ContainsKey("Magias"))
            Magias = (dados["Magias"] as List<object>)?.ConvertAll(x => x.ToString()) ?? new();
        if (dados.ContainsKey("Equipamento"))
            Equipamento = (dados["Equipamento"] as List<object>)?.ConvertAll(x => x.ToString()) ?? new();
    }
}