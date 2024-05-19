using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MinimalApiProject.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>();

// Configurar o serializador JSON
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddHostedService<NotificationBackgroundService>();

var app = builder.Build();

// CRUD de Usuários
app.MapPost("/api/usuario/criar", async ([FromBody] Usuario usuario, [FromServices] AppDbContext context) =>
{
    var erros = new List<ValidationResult>();
    if (!Validator.TryValidateObject(usuario, new ValidationContext(usuario), erros, true))
    {
        return Results.BadRequest(erros);
    }

    var usuarioBuscado = await context.Usuarios.FirstOrDefaultAsync(x => x.Nome == usuario.Nome);
    if (usuarioBuscado is null)
    {
        usuario.Nome = usuario.Nome?.ToUpper();
        context.Usuarios.Add(usuario);
        await context.SaveChangesAsync();
        return Results.Created($"/api/usuario/buscar/{usuario.Id}", usuario);
    }
    return Results.BadRequest("Já existe um usuário com o mesmo nome");
});

app.MapGet("/api/usuario/listar", async ([FromServices] AppDbContext context) =>
{
    var usuarios = await context.Usuarios.ToListAsync();
    if (usuarios.Any())
    {
        return Results.Ok(usuarios);
    }
    return Results.NotFound("Não existem usuários na tabela");
});

app.MapGet("/api/usuario/buscar/{id}", async ([FromRoute] int id, [FromServices] AppDbContext context) =>
{
    var usuario = await context.Usuarios.FindAsync(id);
    if (usuario is null)
    {
        return Results.NotFound("Usuário não encontrado!");
    }
    return Results.Ok(usuario);
});

app.MapDelete("/api/usuario/deletar/{id}", async ([FromRoute] int id, [FromServices] AppDbContext context) =>
{
    var usuario = await context.Usuarios.FindAsync(id);
    if (usuario is null)
    {
        return Results.NotFound("Usuário não encontrado!");
    }
    context.Usuarios.Remove(usuario);
    await context.SaveChangesAsync();
    return Results.Ok(usuario);
});

app.MapPut("/api/usuario/alterar/{id}", async ([FromRoute] int id, [FromBody] Usuario usuarioAlterado, [FromServices] AppDbContext context) =>
{
    var usuario = await context.Usuarios.FindAsync(id);
    if (usuario is null)
    {
        return Results.NotFound("Usuário não encontrado!");
    }

    usuario.Nome = usuarioAlterado.Nome;
    usuario.Email = usuarioAlterado.Email;

    context.Usuarios.Update(usuario);
    await context.SaveChangesAsync();

    return Results.Ok("Usuário alterado com sucesso!");
});

// CRUD de Tarefas
app.MapPost("/api/tarefa/criar", async ([FromBody] Tarefa tarefa, [FromServices] AppDbContext context) =>
{
    var erros = new List<ValidationResult>();
    if (!Validator.TryValidateObject(tarefa, new ValidationContext(tarefa), erros, true))
    {
        return Results.BadRequest(erros);
    }

    context.Tarefas.Add(tarefa);
    await context.SaveChangesAsync();
    return Results.Created($"/api/tarefa/buscar/{tarefa.Id}", tarefa);
});

app.MapGet("/api/tarefa/listar", async ([FromServices] AppDbContext context) =>
{
    var tarefas = await context.Tarefas.ToListAsync();
    if (tarefas.Any())
    {
        return Results.Ok(tarefas);
    }
    return Results.NotFound("Não existem tarefas na tabela");
});

app.MapGet("/api/tarefa/buscar/{id}", async ([FromRoute] int id, [FromServices] AppDbContext context) =>
{
    var tarefa = await context.Tarefas.FindAsync(id);
    if (tarefa is null)
    {
        return Results.NotFound("Tarefa não encontrada!");
    }
    return Results.Ok(tarefa);
});

app.MapDelete("/api/tarefa/deletar/{id}", async ([FromRoute] int id, [FromServices] AppDbContext context) =>
{
    var tarefa = await context.Tarefas.FindAsync(id);
    if (tarefa is null)
    {
        return Results.NotFound("Tarefa não encontrada!");
    }
    context.Tarefas.Remove(tarefa);
    await context.SaveChangesAsync();
    return Results.Ok(tarefa);
});

app.MapPut("/api/tarefa/alterar/{id}", async ([FromRoute] int id, [FromBody] Tarefa tarefaAlterada, [FromServices] AppDbContext context) =>
{
    var tarefa = await context.Tarefas.FindAsync(id);
    if (tarefa is null)
    {
        return Results.NotFound("Tarefa não encontrada!");
    }

    tarefa.Titulo = tarefaAlterada.Titulo;
    tarefa.Descricao = tarefaAlterada.Descricao;
    tarefa.Prazo = tarefaAlterada.Prazo;
    tarefa.Prioridade = tarefaAlterada.Prioridade;
    tarefa.ProjetoId = tarefaAlterada.ProjetoId;

    context.Tarefas.Update(tarefa);
    await context.SaveChangesAsync();

    return Results.Ok("Tarefa alterada com sucesso!");
});

// Listar tarefas por prioridade
app.MapGet("/api/tarefa/listarporprioridade/{prioridade}", async ([FromRoute] PrioridadeEnum prioridade, [FromServices] AppDbContext context) =>
{
    var tarefas = await context.Tarefas.Where(t => t.Prioridade == prioridade).ToListAsync();
    if (tarefas.Any())
    {
        return Results.Ok(tarefas);
    }
    return Results.NotFound("Não existem tarefas com a prioridade especificada");
});

// CRUD de Projetos
app.MapPost("/api/projeto/criar", async ([FromBody] Projeto projeto, [FromServices] AppDbContext context) =>
{
    var erros = new List<ValidationResult>();
    if (!Validator.TryValidateObject(projeto, new ValidationContext(projeto), erros, true))
    {
        return Results.BadRequest(erros);
    }

    context.Projetos.Add(projeto);
    await context.SaveChangesAsync();
    return Results.Created($"/api/projeto/buscar/{projeto.Id}", projeto);
});

app.MapGet("/api/projeto/listar", async ([FromServices] AppDbContext context) =>
{
    var projetos = await context.Projetos.ToListAsync();
    if (projetos.Any())
    {
        return Results.Ok(projetos);
    }
    return Results.NotFound("Não existem projetos na tabela");
});

app.MapGet("/api/projeto/buscar/{id}", async ([FromRoute] int id, [FromServices] AppDbContext context) =>
{
    var projeto = await context.Projetos.FindAsync(id);
    if (projeto is null)
    {
        return Results.NotFound("Projeto não encontrado!");
    }
    return Results.Ok(projeto);
});

app.MapDelete("/api/projeto/deletar/{id}", async ([FromRoute] int id, [FromServices] AppDbContext context) =>
{
    var projeto = await context.Projetos.FindAsync(id);
    if (projeto is null)
    {
        return Results.NotFound("Projeto não encontrado!");
    }
    context.Projetos.Remove(projeto);
    await context.SaveChangesAsync();
    return Results.Ok(projeto);
});

app.MapPut("/api/projeto/alterar/{id}", async ([FromRoute] int id, [FromBody] Projeto projetoAlterado, [FromServices] AppDbContext context) =>
{
    var projeto = await context.Projetos.FindAsync(id);
    if (projeto is null)
    {
        return Results.NotFound("Projeto não encontrado!");
    }

    projeto.Nome = projetoAlterado.Nome;

    context.Projetos.Update(projeto);
    await context.SaveChangesAsync();

    return Results.Ok("Projeto alterado com sucesso!");
});

// Atribuir usuário a uma tarefa
app.MapPost("/api/tarefa/{tarefaId}/atribuir/{usuarioId}", async (int tarefaId, int usuarioId, [FromServices] AppDbContext context) =>
{
    var tarefa = await context.Tarefas.FindAsync(tarefaId);
    var usuario = await context.Usuarios.FindAsync(usuarioId);

    if (tarefa is null || usuario is null)
    {
        return Results.NotFound("Tarefa ou Usuário não encontrado!");
    }

    var atribuicao = new AtribuicaoTarefaUsuario
    {
        TarefaId = tarefaId,
        UsuarioId = usuarioId
    };

    context.Atribuicoes.Add(atribuicao);
    await context.SaveChangesAsync();

    return Results.Ok("Usuário atribuído à tarefa com sucesso!");
});

// Remover usuário de uma tarefa
app.MapDelete("/api/tarefa/{tarefaId}/remover/{usuarioId}", async (int tarefaId, int usuarioId, [FromServices] AppDbContext context) =>
{
    var atribuicao = await context.Atribuicoes.FindAsync(tarefaId, usuarioId);

    if (atribuicao is null)
    {
        return Results.NotFound("Atribuição não encontrada!");
    }

    context.Atribuicoes.Remove(atribuicao);
    await context.SaveChangesAsync();

    return Results.Ok("Usuário removido da tarefa com sucesso!");
});

// Listar tarefas por projeto
// Listar tarefas por projeto
app.MapGet("/api/projeto/{projetoId}/tarefas", async ([FromRoute] int projetoId, [FromServices] AppDbContext context) =>
{
    var tarefas = await context.Tarefas
        .Where(t => t.ProjetoId == projetoId)
        .Select(t => new
        {
            t.Id,
            t.Titulo,
            t.Descricao,
            t.Prazo,
            t.Prioridade,
        })
        .ToListAsync();

    if (tarefas == null || !tarefas.Any())
    {
        return Results.NotFound("Projeto ou tarefas não encontradas!");
    }

    return Results.Ok(tarefas);
});

//notificacao
app.MapGet("/api/notificacao/listar/{usuarioId}", async ([FromRoute] int usuarioId, [FromServices] AppDbContext context) =>
{
    var notificacoes = await context.Notificacoes
        .Where(n => n.UsuarioId == usuarioId)
        .ToListAsync();

    if (!notificacoes.Any())
    {
        return Results.NotFound("Não existem notificações para o usuário especificado");
    }

    return Results.Ok(notificacoes);
});
app.Run();
