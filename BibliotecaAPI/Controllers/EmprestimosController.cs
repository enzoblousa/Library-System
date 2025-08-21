using BibliotecaAPI.Models;
using BibliotecaAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmprestimosController : ControllerBase
    {
        private readonly BibliotecaService _bibliotecaService;

        public EmprestimosController(BibliotecaService bibliotecaService)
        {
            _bibliotecaService = bibliotecaService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var emprestimos = _bibliotecaService.ObterTodosEmprestimos();
            return Ok(emprestimos);
        }

        [HttpPost]
        public IActionResult Post([FromBody] EmprestimoRequest request)
        {
            var emprestimo = _bibliotecaService.RealizarEmprestimo(request.LivroId, request.UsuarioId, request.DiasEmprestimo);
            if (emprestimo == null) 
                return BadRequest("Não foi possível realizar o empréstimo. Verifique se o livro está disponível.");
            
            return Ok(emprestimo);
        }

        [HttpPost("{id}/devolucao")]
        public IActionResult Devolver(int id)
        {
            var sucesso = _bibliotecaService.RealizarDevolucao(id);
            if (!sucesso) return NotFound("Empréstimo não encontrado ou já devolvido");
            return Ok(new { message = "Livro devolvido com sucesso" });
        }

        [HttpGet("ativos")]
        public IActionResult GetAtivos()
        {
            var emprestimos = _bibliotecaService.ObterTodosEmprestimos()
                .Where(e => !e.DataDevolucaoReal.HasValue)
                .ToList();
            return Ok(emprestimos);
        }

        [HttpGet("usuario/{usuarioId}")]
        public IActionResult GetPorUsuario(int usuarioId)
        {
            var emprestimos = _bibliotecaService.ObterTodosEmprestimos()
                .Where(e => e.UsuarioId == usuarioId)
                .ToList();
            return Ok(emprestimos);
        }
    }

    public class EmprestimoRequest
    {
        public int LivroId { get; set; }
        public int UsuarioId { get; set; }
        public int DiasEmprestimo { get; set; } = 7;
    }
}