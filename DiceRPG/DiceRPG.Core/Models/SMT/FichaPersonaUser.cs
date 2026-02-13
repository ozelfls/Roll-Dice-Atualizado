using System.Collections.Generic;

namespace DiceRPG.Core.Models.SMT;

public class FichaPersonaUser : FichaSMTBase
{
    public string PersonaAtual { get; set; }
    public List<string> PersonasDisponiveis { get; set; } = new();
    public int SocialLinks { get; set; }
    public string ArcanaPrincipal { get; set; }

    // Dicionário de Personas com seus atributos
    public Dictionary<string, PersonaData> Personas { get; set; } = new();

    public FichaPersonaUser(string nome) : base(nome, "PersonaUser") { }

    public override Dictionary<string, object> ObterAtributos()
    {
        var baseDict = base.ObterAtributos();
        baseDict["PersonaAtual"] = PersonaAtual;
        baseDict["PersonasDisponiveis"] = PersonasDisponiveis;
        baseDict["SocialLinks"] = SocialLinks;
        baseDict["ArcanaPrincipal"] = ArcanaPrincipal;
        baseDict["Personas"] = Personas;
        return baseDict;
    }

    public override void CarregarAtributos(Dictionary<string, object> dados)
    {
        base.CarregarAtributos(dados);
        PersonaAtual = dados["PersonaAtual"].ToString();
        ArcanaPrincipal = dados["ArcanaPrincipal"].ToString();
        SocialLinks = Convert.ToInt32(dados["SocialLinks"]);

        if (dados.ContainsKey("PersonasDisponiveis"))
            PersonasDisponiveis = (dados["PersonasDisponiveis"] as List<object>)?.ConvertAll(x => x.ToString()) ?? new();

        if (dados.ContainsKey("Personas"))
        {
            var personasDict = dados["Personas"] as Dictionary<string, object>;
            if (personasDict != null)
            {
                foreach (var kv in personasDict)
                {
                    var personaData = kv.Value as Dictionary<string, object>;
                    if (personaData != null)
                    {
                        Personas[kv.Key] = new PersonaData
                        {
                            Level = Convert.ToInt32(personaData["Level"]),
                            HP = Convert.ToInt32(personaData["HP"]),
                            MP = Convert.ToInt32(personaData["MP"]),
                            Habilidades = (personaData["Habilidades"] as List<object>)?.ConvertAll(x => x.ToString()) ?? new()
                        };
                    }
                }
            }
        }
    }
}

public class PersonaData
{
    public int Level { get; set; }
    public int HP { get; set; }
    public int MP { get; set; }
    public List<string> Habilidades { get; set; } = new();
}