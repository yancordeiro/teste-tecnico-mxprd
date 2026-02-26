using GastosResidenciais.Application.DTOs;
using GastosResidenciais.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GastosResidenciais.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriasController : ControllerBase
    {
        private readonly ICategoriaService _categoriaService;

        public CategoriasController(ICategoriaService categoriaService)
        {
            _categoriaService = categoriaService;
        }

        // GET: api/categorias
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoriaOutputDto>>> ObterTodos()
        {
            var categorias = await _categoriaService.ObterTodosAsync();
            return Ok(categorias);
        }

        // GET: api/categorias/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoriaOutputDto>> ObterPorId(Guid id)
        {
            var categoria = await _categoriaService.ObterPorIdAsync(id);
            if (categoria == null)
                return NotFound();

            return Ok(categoria);
        }

        // POST: api/categorias
        [HttpPost]
        public async Task<ActionResult<CategoriaOutputDto>> Criar([FromBody] CategoriaInputDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var categoria = await _categoriaService.CriarAsync(dto);
            return CreatedAtAction(nameof(ObterPorId), new { id = categoria.Id }, categoria);
        }

        // GET: api/categorias/totais
        // Retorna o total de receitas, despesas e saldo por categoria (funcionalidade opcional)
        [HttpGet("totais")]
        public async Task<ActionResult<TotaisCategoriasDto>> ObterTotais()
        {
            var totais = await _categoriaService.ObterTotaisPorCategoriaAsync();
            return Ok(totais);
        }
    }
}
