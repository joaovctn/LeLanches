using LanchesMac.Models;

namespace LanchesMac.Repository.Interfaces;

public interface ICategoriaRepository
{
    IEnumerable<Categoria> Categorias { get; }
}
