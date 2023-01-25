using LanchesMac.Models;

namespace LanchesMac.Repository.Interfaces;

public interface ILancheRepository
{
    IEnumerable<Lanche> Lanches { get; }
    IEnumerable<Lanche> LanchesPreferidos { get; }
    Lanche GetLancheById(int lancheId);
}
