namespace MinimalApiProject.Models
{
public class Tarefa
{
    public int Id { get; set; }
    public string? Titulo { get; set; }
    public string? Descricao { get; set; }
    public DateTime Prazo { get; set; }
    public PrioridadeEnum Prioridade { get; set; }
    public int ProjetoId { get; set; }
    public Projeto? Projeto { get; set; }
    public List<AtribuicaoTarefaUsuario>? Atribuicoes { get; set; }
}
}