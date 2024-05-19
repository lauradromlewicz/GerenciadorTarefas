namespace MinimalApiProject.Models
{
public class AtribuicaoTarefaUsuario
{
    public int TarefaId { get; set; }
    public Tarefa? Tarefa { get; set; }
    public int UsuarioId { get; set; }
    public Usuario? Usuario { get; set; }
}
}