namespace BibliotecaAPI.Models
{
    public class Livro
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Autor { get; set; } = string.Empty;
        public string ISBN { get; set; } = string.Empty;
        public int AnoPublicacao { get; set; }
        public string Genero { get; set; } = string.Empty;
        public bool Disponivel { get; set; } = true;
        public DateTime DataCadastro { get; set; }
    }

    public class Usuario
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public DateTime DataCadastro { get; set; }
    }

    public class Emprestimo
    {
        public int Id { get; set; }
        public int LivroId { get; set; }
        public int UsuarioId { get; set; }
        public DateTime DataEmprestimo { get; set; }
        public DateTime DataDevolucaoPrevista { get; set; }
        public DateTime? DataDevolucaoReal { get; set; }
        public string Status => DataDevolucaoReal.HasValue ? "Devolvido" : "Emprestado";
        
        // Propriedades de navegação (apenas para frontend)
        public string? LivroTitulo { get; set; }
        public string? UsuarioNome { get; set; }
    }
}