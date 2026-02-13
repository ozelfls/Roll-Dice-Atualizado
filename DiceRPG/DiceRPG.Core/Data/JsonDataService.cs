using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using DiceRPG.Core.Models;
using DiceRPG.Core.Models.Cyberpunk;
using DiceRPG.Core.Models.Warhammer;
using DiceRPG.Core.Models.SMT;

namespace DiceRPG.Core.Data;

public class JsonDataService
{
    private readonly string _dataFolder;
    private readonly JsonSerializerOptions _options;

    public JsonDataService()
    {
        _dataFolder = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "DiceRPG", "fichas");
        Directory.CreateDirectory(_dataFolder);

        _options = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true
        };
    }

    public async Task SalvarFicha<T>(T ficha) where T : FichaBase
    {
        ficha.DataModificacao = DateTime.Now;
        var caminho = Path.Combine(_dataFolder, $"{ficha.Sistema}_{ficha.Nome}_{ficha.Id}.json");
        var json = JsonSerializer.Serialize(ficha, _options);
        await File.WriteAllTextAsync(caminho, json);
    }

    public async Task<List<T>> CarregarFichas<T>(string sistema) where T : FichaBase
    {
        var arquivos = Directory.GetFiles(_dataFolder, $"{sistema}_*.json");
        var fichas = new List<T>();

        foreach (var arquivo in arquivos)
        {
            try
            {
                var json = await File.ReadAllTextAsync(arquivo);
                var ficha = JsonSerializer.Deserialize<T>(json, _options);
                if (ficha != null) fichas.Add(ficha);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao carregar {arquivo}: {ex.Message}");
            }
        }

        return fichas;
    }

    public async Task<List<FichaBase>> CarregarTodasFichas()
    {
        var arquivos = Directory.GetFiles(_dataFolder, "*.json");
        var fichas = new List<FichaBase>();

        foreach (var arquivo in arquivos)
        {
            try
            {
                var json = await File.ReadAllTextAsync(arquivo);
                var dados = JsonSerializer.Deserialize<Dictionary<string, object>>(json, _options);
                var sistema = dados?["Sistema"]?.ToString();

                FichaBase? ficha = sistema switch
                {
                    "Cyberpunk" => JsonSerializer.Deserialize<FichaCyberpunk>(json, _options),
                    "Warhammer" => JsonSerializer.Deserialize<FichaWarhammer>(json, _options),
                    string s when s?.StartsWith("SMT-") ?? false => s switch
                    {
                        "SMT-Nahobino" => JsonSerializer.Deserialize<FichaNahobino>(json, _options),
                        "SMT-DemiFiend" => JsonSerializer.Deserialize<FichaDemiFiend>(json, _options),
                        "SMT-Samurai" => JsonSerializer.Deserialize<FichaSamurai>(json, _options),
                        "SMT-PersonaUser" => JsonSerializer.Deserialize<FichaPersonaUser>(json, _options),
                        _ => null
                    },
                    _ => null
                };

                if (ficha != null) fichas.Add(ficha);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao carregar {arquivo}: {ex.Message}");
            }
        }

        return fichas;
    }

    public void DeletarFicha(Guid id)
    {
        var arquivos = Directory.GetFiles(_dataFolder, $"*{id}*.json");
        foreach (var arquivo in arquivos)
            File.Delete(arquivo);
    }

    public async Task<bool> FichaExiste(string nome, string sistema)
    {
        var arquivos = Directory.GetFiles(_dataFolder, $"{sistema}_{nome}_*.json");
        return arquivos.Length > 0;
    }
}