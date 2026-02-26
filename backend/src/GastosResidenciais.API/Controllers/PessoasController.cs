using GastosResidenciais.Application.DTOs;
using GastosResidenciais.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GastosResidenciais.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PessoasController : ControllerBase
    {
        private readonly IPessoaService _pessoaService;

        public PessoasController(IPessoaService pessoaService)
        {
            _pessoaService = pessoaService;
        }

        // GET: api/pessoas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PessoaOutputDto>>> ObterTodos()
        {
            var pessoas = await _pessoaService.ObterTodosAsync();
            return Ok(pessoas);
        }

        // GET: api/pessoas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PessoaOutputDto>> ObterPorId(Guid id)
        {
            var pessoa = await _pessoaService.ObterPorIdAsync(id);
            if (pessoa == null)
                return NotFound();

            return Ok(pessoa);
        }

        // POST: api/pessoas
        [HttpPost]
        public async Task<ActionResult<PessoaOutputDto>> Criar([FromBody] PessoaInputDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var pessoa = await _pessoaService.CriarAsync(dto);
            return CreatedAtAction(nameof(ObterPorId), new { id = pessoa.Id }, pessoa);
        }

        // PUT: api/pessoas/5
        [HttpPut("{id}")]
        public async Task<ActionResult<PessoaOutputDto>> Atualizar(Guid id, [FromBody] PessoaInputDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var pessoa = await _pessoaService.AtualizarAsync(id, dto);
            if (pessoa == null)
                return NotFound();

            return Ok(pessoa);
        }

        // DELETE: api/pessoas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Remover(Guid id)
        {
            var removido = await _pessoaService.RemoverAsync(id);
            if (!removido)
                return NotFound();

            return NoContent();
        }

        // GET: api/pessoas/totais
        // Retorna o total de receitas, despesas e saldo por pessoa
        [HttpGet("totais")]
        public async Task<ActionResult<TotaisGeraisDto>> ObterTotais()
        {
            var totais = await _pessoaService.ObterTotaisPorPessoaAsync();
            return Ok(totais);
        }
    }
}
