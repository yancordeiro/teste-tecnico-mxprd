using GastosResidenciais.Domain.Entities;
using GastosResidenciais.Domain.Interfaces;
using GastosResidenciais.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GastosResidenciais.Infrastructure.Repositories
{
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly AppDbContext _context;

        public CategoriaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Categoria>> ObterTodosAsync()
        {
            return await _context.Categorias.ToListAsync();
        }

        public async Task<Categoria?> ObterPorIdAsync(Guid id)
        {
            return await _context.Categorias.FindAsync(id);
        }

        public async Task<Categoria> AdicionarAsync(Categoria categoria)
        {
            _context.Categorias.Add(categoria);
            await _context.SaveChangesAsync();
            return categoria;
        }
    }
}
