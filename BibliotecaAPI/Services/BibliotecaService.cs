using BibliotecaAPI.Models;

namespace BibliotecaAPI.Services
{
    public class BibliotecaService
    {
        private readonly List<Livro> _livros;
        private readonly List<Usuario> _usuarios;
        private readonly List<Emprestimo> _emprestimos;
        private int _nextLivroId = 1;
        private int _nextUsuarioId = 1;
        private int _nextEmprestimoId = 1;

        public BibliotecaService()
        {
            _livros = new List<Livro>();
            _usuarios = new List<Usuario>();
            _emprestimos = new List<Emprestimo>();
            
            // Inicializar com dados de exemplo
            InicializarDados();
        }

        private void InicializarDados()
        {
            // Livros de exemplo
            AdicionarLivro(new Livro { 
                Titulo = "Dom Casmurro", 
                Autor = "Machado de Assis", 
                ISBN = "9788535931234", 
                AnoPublicacao = 1899, 
                Genero = "Romance" 
            });
            
            AdicionarLivro(new Livro { 
                Titulo = "1984", 
                Autor = "George Orwell", 
                ISBN = "9788535931241", 
                AnoPublicacao = 1949, 
                Genero = "Ficção Científica" 
            });
            
            AdicionarLivro(new Livro { 
                Titulo = "O Senhor dos Anéis", 
                Autor = "J.R.R. Tolkien", 
                ISBN = "9788535931258", 
                AnoPublicacao = 1954, 
                Genero = "Fantasia" 
            });

            // Usuários de exemplo
            AdicionarUsuario(new Usuario { 
                Nome = "João Silva", 
                Email = "joao@email.com", 
                Telefone = "(11) 99999-9999" 
            });
            
            AdicionarUsuario(new Usuario { 
                Nome = "Maria Santos", 
                Email = "maria@email.com", 
                Telefone = "(11) 98888-8888" 
            });
        }

        // Livros
        public List<Livro> ObterTodosLivros() => _livros;
        public Livro? ObterLivroPorId(int id) => _livros.FirstOrDefault(l => l.Id == id);
        
        public Livro AdicionarLivro(Livro livro)
        {
            livro.Id = _nextLivroId++;
            livro.DataCadastro = DateTime.Now;
            livro.Disponivel = true;
            _livros.Add(livro);
            return livro;
        }

        public bool RemoverLivro(int id)
        {
            var livro = _livros.FirstOrDefault(l => l.Id == id);
            return livro != null && _livros.Remove(livro);
        }

        // Usuários
        public List<Usuario> ObterTodosUsuarios() => _usuarios;
        public Usuario? ObterUsuarioPorId(int id) => _usuarios.FirstOrDefault(u => u.Id == id);

        public Usuario AdicionarUsuario(Usuario usuario)
        {
            usuario.Id = _nextUsuarioId++;
            usuario.DataCadastro = DateTime.Now;
            _usuarios.Add(usuario);
            return usuario;
        }

        // Empréstimos
        public List<Emprestimo> ObterTodosEmprestimos()
        {
            return _emprestimos.Select(e => new Emprestimo
            {
                Id = e.Id,
                LivroId = e.LivroId,
                UsuarioId = e.UsuarioId,
                DataEmprestimo = e.DataEmprestimo,
                DataDevolucaoPrevista = e.DataDevolucaoPrevista,
                DataDevolucaoReal = e.DataDevolucaoReal,
                LivroTitulo = _livros.FirstOrDefault(l => l.Id == e.LivroId)?.Titulo,
                UsuarioNome = _usuarios.FirstOrDefault(u => u.Id == e.UsuarioId)?.Nome
            }).ToList();
        }

        public Emprestimo? RealizarEmprestimo(int livroId, int usuarioId, int diasEmprestimo = 7)
        {
            var livro = ObterLivroPorId(livroId);
            var usuario = ObterUsuarioPorId(usuarioId);

            if (livro == null || usuario == null || !livro.Disponivel)
                return null;

            livro.Disponivel = false;

            var emprestimo = new Emprestimo
            {
                Id = _nextEmprestimoId++,
                LivroId = livroId,
                UsuarioId = usuarioId,
                DataEmprestimo = DateTime.Now,
                DataDevolucaoPrevista = DateTime.Now.AddDays(diasEmprestimo),
                LivroTitulo = livro.Titulo,
                UsuarioNome = usuario.Nome
            };

            _emprestimos.Add(emprestimo);
            return emprestimo;
        }

        public bool RealizarDevolucao(int emprestimoId)
        {
            var emprestimo = _emprestimos.FirstOrDefault(e => e.Id == emprestimoId);
            if (emprestimo == null || emprestimo.DataDevolucaoReal.HasValue)
                return false;

            emprestimo.DataDevolucaoReal = DateTime.Now;

            var livro = ObterLivroPorId(emprestimo.LivroId);
            if (livro != null)
                livro.Disponivel = true;

            return true;
        }
    }
}