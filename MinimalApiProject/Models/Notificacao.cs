namespace MinimalApiProject.Models
{
    public class Notificacao
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public string? Mensagem { get; set; }
        public DateTime DataCriacao { get; set; }
    }
}
