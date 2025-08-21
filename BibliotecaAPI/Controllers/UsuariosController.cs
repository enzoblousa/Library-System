using BibliotecaAPI.Models;
using BibliotecaAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly BibliotecaService _bibliotecaService;

        public UsuariosController(BibliotecaService bibliotecaService)
        {
            _bibliotecaService = bibliotecaService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var usuarios = _bibliotecaService.ObterTodosUsuarios();
            return Ok(usuarios);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var usuario = _bibliotecaService.ObterUsuarioPorId(id);
            if (usuario == null) return NotFound();
            return Ok(usuario);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Usuario usuario)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            
            var novoUsuario = _bibliotecaService.AdicionarUsuario(usuario);
            return CreatedAtAction(nameof(GetById), new { id = novoUsuario.Id }, novoUsuario);
        }

        [HttpGet("pesquisar")]
        public IActionResult Pesquisar([FromQuery] string termo)
        {
            if (string.IsNullOrEmpty(termo))
                return Ok(_bibliotecaService.ObterTodosUsuarios());

            var usuarios = _bibliotecaService.ObterTodosUsuarios()
                .Where(u => u.Nome.Contains(termo, StringComparison.OrdinalIgnoreCase) ||
                           u.Email.Contains(termo, StringComparison.OrdinalIgnoreCase))
                .ToList();
            return Ok(usuarios);
        }
    }
}