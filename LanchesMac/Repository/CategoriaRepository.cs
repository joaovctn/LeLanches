using LanchesMac.Context;
using LanchesMac.Models;
using LanchesMac.Repository.Interfaces;

namespace LanchesMac.Repository;

public class CategoriaRepository : ICategoriaRepository
{
    private readonly AppDbContext _context;

    public CategoriaRepository(AppDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Categoria> Categorias => _context.Categorias;
}
