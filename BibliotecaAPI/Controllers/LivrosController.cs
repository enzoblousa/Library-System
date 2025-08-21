using BibliotecaAPI.Models;
using BibliotecaAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LivrosController : ControllerBase
    {
        private readonly BibliotecaService _bibliotecaService;

        public LivrosController(BibliotecaService bibliotecaService)
        {
            _bibliotecaService = bibliotecaService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var livros = _bibliotecaService.ObterTodosLivros();
            return Ok(livros);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var livro = _bibliotecaService.ObterLivroPorId(id);
            if (livro == null) return NotFound();
            return Ok(livro);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Livro livro)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            
            var novoLivro = _bibliotecaService.AdicionarLivro(livro);
            return CreatedAtAction(nameof(GetById), new { id = novoLivro.Id }, novoLivro);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var removido = _bibliotecaService.RemoverLivro(id);
            if (!removido) return NotFound();
            return NoContent();
        }

        [HttpGet("disponiveis")]
        public IActionResult GetDisponiveis()
        {
            var livros = _bibliotecaService.ObterTodosLivros()
                .Where(l => l.Disponivel)
                .ToList();
            return Ok(livros);
        }

        [HttpGet("genero/{genero}")]
        public IActionResult GetPorGenero(string genero)
        {
            var livros = _bibliotecaService.ObterTodosLivros()
                .Where(l => l.Genero.Equals(genero, StringComparison.OrdinalIgnoreCase))
                .ToList();
            return Ok(livros);
        }

        [HttpGet("pesquisar")]
        public IActionResult Pesquisar([FromQuery] string termo)
        {
            if (string.IsNullOrEmpty(termo))
                return Ok(_bibliotecaService.ObterTodosLivros());

            var livros = _bibliotecaService.ObterTodosLivros()
                .Where(l => l.Titulo.Contains(termo, StringComparison.OrdinalIgnoreCase) ||
                           l.Autor.Contains(termo, StringComparison.OrdinalIgnoreCase) ||
                           l.Genero.Contains(termo, StringComparison.OrdinalIgnoreCase))
                .ToList();
            return Ok(livros);
        }
    }
}