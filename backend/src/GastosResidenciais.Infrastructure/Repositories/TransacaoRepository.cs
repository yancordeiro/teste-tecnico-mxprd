using GastosResidenciais.Domain.Entities;
using GastosResidenciais.Domain.Interfaces;
using GastosResidenciais.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GastosResidenciais.Infrastructure.Repositories
{
    public class TransacaoRepository : ITransacaoRepository
    {
        private readonly AppDbContext _context;

        public TransacaoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Transacao>> ObterTodosAsync()
        {
            return await _context.Transacoes
                .Include(t => t.Categoria)
                .Include(t => t.Pessoa)
                .ToListAsync();
        }

        public async Task<Transacao?> ObterPorIdAsync(Guid id)
        {
            return await _context.Transacoes
                .Include(t => t.Categoria)
                .Include(t => t.Pessoa)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<Transacao> AdicionarAsync(Transacao transacao)
        {
            _context.Transacoes.Add(transacao);
            await _context.SaveChangesAsync();
            return transacao;
        }

        public async Task RemoverPorPessoaIdAsync(Guid pessoaId)
        {
            var transacoes = await _context.Transacoes
                .Where(t => t.PessoaId == pessoaId)
                .ToListAsync();

            _context.Transacoes.RemoveRange(transacoes);
            await _context.SaveChangesAsync();
        }
    }
}
