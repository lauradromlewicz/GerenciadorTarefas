using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MinimalApiProject.Models;

public class NotificationBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public NotificationBackgroundService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var now = DateTime.UtcNow;

                // Verifica tarefas próximas do prazo
                var tasksToNotify = await dbContext.Tarefas
                    .Where(task => task.Prazo <= now.AddDays(1) && task.Prazo > now)
                    .Include(task => task.Atribuicoes!)
                        .ThenInclude(at => at.Usuario)
                    .ToListAsync();

                foreach (var task in tasksToNotify)
                {
                    foreach (var atribuicao in task.Atribuicoes!)
                    {
                        var notification = new Notificacao
                        {
                            UsuarioId = atribuicao.UsuarioId,
                            Mensagem = $"A tarefa '{task.Titulo}' está próxima do prazo de conclusão.",
                            DataCriacao = DateTime.UtcNow
                        };

                        dbContext.Notificacoes.Add(notification);
                    }
                }

                // Verifica tarefas expiradas
                var expiredTasks = await dbContext.Tarefas
                    .Where(task => task.Prazo <= now)
                    .Include(task => task.Atribuicoes!)
                        .ThenInclude(at => at.Usuario)
                    .ToListAsync();

                foreach (var task in expiredTasks)
                {
                    foreach (var atribuicao in task.Atribuicoes!)
                    {
                        var notification = new Notificacao
                        {
                            UsuarioId = atribuicao.UsuarioId,
                            Mensagem = $"A tarefa '{task.Titulo}' expirou.",
                            DataCriacao = DateTime.UtcNow
                        };

                        dbContext.Notificacoes.Add(notification);
                    }
                }

                await dbContext.SaveChangesAsync();
            }

            await Task.Delay(TimeSpan.FromHours(1), stoppingToken); // Verifica a cada hora
        }
    }
}
