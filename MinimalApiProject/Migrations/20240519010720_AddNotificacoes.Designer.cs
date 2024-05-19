﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MinimalApiProject.Models;

#nullable disable

namespace MinimalApiProject.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240519010720_AddNotificacoes")]
    partial class AddNotificacoes
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.5");

            modelBuilder.Entity("MinimalApiProject.Models.AtribuicaoTarefaUsuario", b =>
                {
                    b.Property<int>("TarefaId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("UsuarioId")
                        .HasColumnType("INTEGER");

                    b.HasKey("TarefaId", "UsuarioId");

                    b.HasIndex("UsuarioId");

                    b.ToTable("Atribuicoes");
                });

            modelBuilder.Entity("MinimalApiProject.Models.Notificacao", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("DataCriacao")
                        .HasColumnType("TEXT");

                    b.Property<string>("Mensagem")
                        .HasColumnType("TEXT");

                    b.Property<int>("UsuarioId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Notificacoes");
                });

            modelBuilder.Entity("MinimalApiProject.Models.Projeto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Nome")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Projetos");
                });

            modelBuilder.Entity("MinimalApiProject.Models.Tarefa", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Descricao")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Prazo")
                        .HasColumnType("TEXT");

                    b.Property<int>("Prioridade")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ProjetoId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Titulo")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ProjetoId");

                    b.ToTable("Tarefas");
                });

            modelBuilder.Entity("MinimalApiProject.Models.Usuario", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Email")
                        .HasColumnType("TEXT");

                    b.Property<string>("Nome")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Usuarios");
                });

            modelBuilder.Entity("MinimalApiProject.Models.AtribuicaoTarefaUsuario", b =>
                {
                    b.HasOne("MinimalApiProject.Models.Tarefa", "Tarefa")
                        .WithMany("Atribuicoes")
                        .HasForeignKey("TarefaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MinimalApiProject.Models.Usuario", "Usuario")
                        .WithMany("Atribuicoes")
                        .HasForeignKey("UsuarioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Tarefa");

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("MinimalApiProject.Models.Tarefa", b =>
                {
                    b.HasOne("MinimalApiProject.Models.Projeto", "Projeto")
                        .WithMany("Tarefas")
                        .HasForeignKey("ProjetoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Projeto");
                });

            modelBuilder.Entity("MinimalApiProject.Models.Projeto", b =>
                {
                    b.Navigation("Tarefas");
                });

            modelBuilder.Entity("MinimalApiProject.Models.Tarefa", b =>
                {
                    b.Navigation("Atribuicoes");
                });

            modelBuilder.Entity("MinimalApiProject.Models.Usuario", b =>
                {
                    b.Navigation("Atribuicoes");
                });
#pragma warning restore 612, 618
        }
    }
}