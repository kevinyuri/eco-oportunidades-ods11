using Xunit;
using Microsoft.EntityFrameworkCore;
using TrabalhoCapacitacao.Controllers;
using TrabalhoCapacitacao.Data;
using TrabalhoCapacitacao.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace TrabalhoCapacitacao.Tests
{
    public class CursosTests
    {
        private AppDbContext GetDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            var context = new AppDbContext(options);
            context.Database.EnsureCreated();
            return context;
        }

        [Fact]
        public async Task GetCursos_DeveRetornarListaDeCursos()
        {
            // Arrange
            var context = GetDatabaseContext();

            context.Cursos.Add(new Curso
            {
                Id = "1",
                Nome = "Curso Teste",
                Instituicao = "Inst Teste",
                DataInicio = DateTime.Now,
                CargaHoraria = "40h",
                Modalidade = "Online",
                ImpactoComunitario = "Impacto Teste",
                FocadoEmSustentabilidade = true
            });
            context.SaveChanges();

            var controller = new CursosController(context);

            // Act
            var resultado = await controller.GetCursos();

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(resultado.Result);
            var lista = Assert.IsType<List<DTOs.Curso.CursoResponseDto>>(actionResult.Value);
            Assert.Single(lista);
            Assert.Equal("Curso Teste", lista[0].Nome);
        }

        [Fact]
        public async Task PostCurso_DeveCriarCurso_ComFocoODS11()
        {
            // Arrange
            var context = GetDatabaseContext();
            var controller = new CursosController(context);

            var novoCurso = new DTOs.Curso.CursoCreateDto
            {
                Nome = "Horta Comunitária",
                Instituicao = "ONG Verde",
                CargaHoraria = "20h",
                Modalidade = "Presencial", // Garantir que está preenchido
                DataInicio = DateTime.Now.AddDays(10),
                FocadoEmSustentabilidade = true,
                ImpactoComunitario = "Ensina a plantar"
            };

            // Act
            var resultado = await controller.PostCurso(novoCurso);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(resultado.Result);
            var cursoRetornado = Assert.IsType<DTOs.Curso.CursoResponseDto>(createdResult.Value);

            Assert.True(cursoRetornado.FocadoEmSustentabilidade);
            Assert.Equal("Horta Comunitária", cursoRetornado.Nome);
        }
    }
}