using System;
using System.Collections.Generic;

namespace DiceRPG.Core.Models;

public class Dado
{
    public int Lados { get; }
    private static readonly Random _random = new();

    public Dado(int lados)
    {
        if (lados <= 0) throw new ArgumentException("Número de lados inválido");
        Lados = lados;
    }

    public int Rolar() => _random.Next(1, Lados + 1);

    public (int resultado, List<int> todos) RolarComVantagem()
    {
        var r1 = Rolar();
        var r2 = Rolar();
        return (Math.Max(r1, r2), new List<int> { r1, r2 });
    }

    public (int resultado, List<int> todos) RolarComDesvantagem()
    {
        var r1 = Rolar();
        var r2 = Rolar();
        return (Math.Min(r1, r2), new List<int> { r1, r2 });
    }

    public override string ToString() => $"D{Lados}";
}