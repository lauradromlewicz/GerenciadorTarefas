namespace MinimalApiProject.Models
{
public class Usuario
{
    public int Id { get; set; }
    public string? Nome { get; set; }
    public string? Email { get; set; }
    public List<AtribuicaoTarefaUsuario>? Atribuicoes { get; set; }
}
}