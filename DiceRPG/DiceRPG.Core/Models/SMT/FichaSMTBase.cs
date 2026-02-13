using System;
using System.Collections.Generic;
using System.Linq;

namespace DiceRPG.Core.Models.SMT;

public abstract class FichaSMTBase : FichaBase
{
    // Atributos base
    private int _level;
    private int _hp;
    private int _mp;
    private int _forca;
    private int _magia;
    private int _vitalidade;
    private int _agilidade;
    private int _sorte;

    // Status derivados
    private int _hpAtual;
    private int _mpAtual;
    private int _ataque;
    private int _defesa;
    private int _precisao;
    private int _esquiva;

    // Propriedades com notificação (para binding)
    public int Level
    {
        get => _level;
        set { _level = value; CalcularStatusDerivados(); }
    }

    public int HP
    {
        get => _hp;
        set { _hp = value; CalcularStatusDerivados(); }
    }

    public int MP
    {
        get => _mp;
        set { _mp = value; CalcularStatusDerivados(); }
    }

    public int Forca
    {
        get => _forca;
        set { _forca = value; CalcularStatusDerivados(); }
    }

    public int Magia
    {
        get => _magia;
        set { _magia = value; CalcularStatusDerivados(); }
    }

    public int Vitalidade
    {
        get => _vitalidade;
        set { _vitalidade = value; CalcularStatusDerivados(); }
    }

    public int Agilidade
    {
        get => _agilidade;
        set { _agilidade = value; CalcularStatusDerivados(); }
    }

    public int Sorte
    {
        get => _sorte;
        set { _sorte = value; CalcularStatusDerivados(); }
    }

    public int HPAtual
    {
        get => _hpAtual;
        set => _hpAtual = Math.Max(0, Math.Min(value, HP));
    }

    public int MPAtual
    {
        get => _mpAtual;
        set => _mpAtual = Math.Max(0, Math.Min(value, MP));
    }

    public int Ataque
    {
        get => _ataque;
        private set => _ataque = value;
    }

    public int Defesa
    {
        get => _defesa;
        private set => _defesa = value;
    }

    public int Precisao
    {
        get => _precisao;
        private set => _precisao = value;
    }

    public int Esquiva
    {
        get => _esquiva;
        private set => _esquiva = value;
    }

    // Afinidades com elementos
    private Dictionary<string, string> _afinidades = new()
    {
        ["Físico"] = "Normal",
        ["Fogo"] = "Normal",
        ["Gelo"] = "Normal",
        ["Eletricidade"] = "Normal",
        ["Vento"] = "Normal",
        ["Luz"] = "Normal",
        ["Escuridão"] = "Normal",
        ["Almighty"] = "Normal"
    };

    public Dictionary<string, string> Afinidades
    {
        get => _afinidades;
        set => _afinidades = value ?? new();
    }

    // Habilidades e itens
    private List<string> _habilidades = new();
    private List<string> _itens = new();

    public List<string> Habilidades
    {
        get => _habilidades;
        set => _habilidades = value ?? new();
    }

    public List<string> Itens
    {
        get => _itens;
        set => _itens = value ?? new();
    }

    protected FichaSMTBase(string nome, string subtipo) : base(nome, $"SMT-{subtipo}) 
    {
        // Valores iniciais padrão
        Level = 1;
        HP = 50;
        MP = 20;
        Forca = 5;
        Magia = 5;
        Vitalidade = 5;
        Agilidade = 5;
        Sorte = 5;

        HPAtual = HP;
        MPAtual = MP;

        CalcularStatusDerivados();
    }

    private void CalcularStatusDerivados()
    {
        // Fórmulas baseadas em SMT (aproximadas)
        Ataque = Forca * 2 + Level;
        Defesa = Vitalidade * 2 + Level;
        Precisao = Agilidade * 3 + Sorte;
        Esquiva = Agilidade * 2 + Sorte / 2;
    }

    public virtual void ReceberDano(int dano, string elemento)
    {
        var afinidade = Afinidades.GetValueOrDefault(elemento, "Normal");
        var danoFinal = dano;

        switch (afinidade)
        {
            case "Fraco":
                danoFinal = (int)(dano * 1.5);
                break;
            case "Resiste":
                danoFinal = (int)(dano * 0.5);
                break;
            case "Reflete":
                danoFinal = 0;
                // Lógica de refletir dano seria implementada aqui
                break;
            case "Absorve":
                danoFinal = -dano; // Cura ao invés de dano
                break;
        }

        HPAtual -= danoFinal;
    }

    public virtual void UsarHabilidade(string habilidade)
    {
        // Lógica base para usar habilidades
        // Será sobrescrita pelas classes filhas
    }

    public override Dictionary<string, object> ObterAtributos()
    {
        return new Dictionary<string, object>
        {
            ["Level"] = Level,
            ["HP"] = HP,
            ["MP"] = MP,
            ["Forca"] = Forca,
            ["Magia"] = Magia,
            ["Vitalidade"] = Vitalidade,
            ["Agilidade"] = Agilidade,
            ["Sorte"] = Sorte,
            ["HPAtual"] = HPAtual,
            ["MPAtual"] = MPAtual,
            ["Ataque"] = Ataque,
            ["Defesa"] = Defesa,
            ["Precisao"] = Precisao,
            ["Esquiva"] = Esquiva,
            ["Afinidades"] = new Dictionary<string, string>(Afinidades),
            ["Habilidades"] = new List<string>(Habilidades),
            ["Itens"] = new List<string>(Itens)
        };
    }

    public override void CarregarAtributos(Dictionary<string, object> dados)
    {
        if (dados.TryGetValue("Level", out var level)) Level = Convert.ToInt32(level);
        if (dados.TryGetValue("HP", out var hp)) HP = Convert.ToInt32(hp);
        if (dados.TryGetValue("MP", out var mp)) MP = Convert.ToInt32(mp);
        if (dados.TryGetValue("Forca", out var forca)) Forca = Convert.ToInt32(forca);
        if (dados.TryGetValue("Magia", out var magia)) Magia = Convert.ToInt32(magia);
        if (dados.TryGetValue("Vitalidade", out var vital)) Vitalidade = Convert.ToInt32(vital);
        if (dados.TryGetValue("Agilidade", out var agi)) Agilidade = Convert.ToInt32(agi);
        if (dados.TryGetValue("Sorte", out var sorte)) Sorte = Convert.ToInt32(sorte);

        if (dados.TryGetValue("HPAtual", out var hpAtual)) HPAtual = Convert.ToInt32(hpAtual);
        if (dados.TryGetValue("MPAtual", out var mpAtual)) MPAtual = Convert.ToInt32(mpAtual);

        if (dados.TryGetValue("Afinidades", out var afinidadesObj))
        {
            var afinidadesDict = afinidadesObj as Dictionary<string, object>;
            if (afinidadesDict != null)
            {
                Afinidades = afinidadesDict.ToDictionary(
                    kv => kv.Key,
                    kv => kv.Value?.ToString() ?? "Normal"
                );
            }
        }

        if (dados.TryGetValue("Habilidades", out var habObj) && habObj is List<object> habList)
        {
            Habilidades = habList.Select(x => x?.ToString() ?? "").Where(x => !string.IsNullOrEmpty(x)).ToList();
        }

        if (dados.TryGetValue("Itens", out var itensObj) && itensObj is List<object> itensList)
        {
            Itens = itensList.Select(x => x?.ToString() ?? "").Where(x => !string.IsNullOrEmpty(x)).ToList();
        }

        CalcularStatusDerivados();
    }

    // Métodos auxiliares
    public void CurarHP(int quantidade)
    {
        HPAtual = Math.Min(HP, HPAtual + quantidade);
    }

    public void CurarMP(int quantidade)
    {
        MPAtual = Math.Min(MP, MPAtual + quantidade);
    }

    public void RestaurarCompleto()
    {
        HPAtual = HP;
        MPAtual = MP;
    }

    public bool EstaVivo => HPAtual > 0;

    public override string ToString()
    {
        return $"{Nome} (Lv.{Level}) - HP:{HPAtual}/{HP} MP:{MPAtual}/{MP}";
    }
}