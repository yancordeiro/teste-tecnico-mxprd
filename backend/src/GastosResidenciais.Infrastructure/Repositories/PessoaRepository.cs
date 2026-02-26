using GastosResidenciais.Domain.Entities;
using GastosResidenciais.Domain.Interfaces;
using GastosResidenciais.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GastosResidenciais.Infrastructure.Repositories
{
    public class PessoaRepository : IPessoaRepository
    {
        private readonly AppDbContext _context;

        public PessoaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Pessoa>> ObterTodosAsync()
        {
            return await _context.Pessoas.ToListAsync();
        }

        public async Task<Pessoa?> ObterPorIdAsync(Guid id)
        {
            return await _context.Pessoas.FindAsync(id);
        }

        public async Task<Pessoa> AdicionarAsync(Pessoa pessoa)
        {
            _context.Pessoas.Add(pessoa);
            await _context.SaveChangesAsync();
            return pessoa;
        }

        public async Task AtualizarAsync(Pessoa pessoa)
        {
            _context.Pessoas.Update(pessoa);
            await _context.SaveChangesAsync();
        }

        public async Task RemoverAsync(Guid id)
        {
            var pessoa = await _context.Pessoas.FindAsync(id);
            if (pessoa != null)
            {
                _context.Pessoas.Remove(pessoa);
                await _context.SaveChangesAsync();
            }
        }
    }
}
