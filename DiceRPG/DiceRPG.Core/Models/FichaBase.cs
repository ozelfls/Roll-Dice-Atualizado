using System;
using System.Collections.Generic;

namespace DiceRPG.Core.Models;

public abstract class FichaBase
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Nome { get; set; }
    public string Sistema { get; protected set; }
    public DateTime DataCriacao { get; set; } = DateTime.Now;
    public DateTime DataModificacao { get; set; } = DateTime.Now;

    protected FichaBase(string nome, string sistema)
    {
        Nome = nome;
        Sistema = sistema;
    }

    public abstract Dictionary<string, object> ObterAtributos();
    public abstract void CarregarAtributos(Dictionary<string, object> dados);
}