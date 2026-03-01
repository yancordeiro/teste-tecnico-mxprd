using GastosResidenciais.Application.Requests;
using GastosResidenciais.Application.Responses;
using GastosResidenciais.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GastosResidenciais.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransacoesController : ControllerBase
    {
        private readonly ITransacaoService _transacaoService;

        public TransacoesController(ITransacaoService transacaoService)
        {
            _transacaoService = transacaoService;
        }

        // GET: api/transacoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TransacaoResponse>>> ObterTodos()
        {
            var transacoes = await _transacaoService.ObterTodosAsync();
            return Ok(transacoes);
        }

        // GET: api/transacoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TransacaoResponse>> ObterPorId(Guid id)
        {
            var transacao = await _transacaoService.ObterPorIdAsync(id);
            if (transacao == null)
                return NotFound();

            return Ok(transacao);
        }

        // POST: api/transacoes
        // Cria uma nova transacao com as validacoes de regra de negocio:
        // - Menor de idade so pode ter despesas
        // - Categoria deve aceitar o tipo da transacao
        [HttpPost]
        public async Task<ActionResult<TransacaoResponse>> Criar([FromBody] CreateTransacaoRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var transacao = await _transacaoService.CriarAsync(request);
                return CreatedAtAction(nameof(ObterPorId), new { id = transacao.Id }, transacao);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { erro = ex.Message });
            }
        }
    }
}
