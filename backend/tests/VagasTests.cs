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
    public class VagasTests
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
        public async Task GetVagas_DeveRetornarListaVazia_QuandoNaoHouverVagas()
        {
            // Arrange
            var context = GetDatabaseContext();
            var controller = new VagasController(context);

            // Act
            var resultado = await controller.GetVagas();

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(resultado.Result);
            // Verifica se o retorno não é nulo
            Assert.NotNull(actionResult.Value);
        }

        [Fact]
        public async Task PostVaga_DeveCriarVagaComSucesso()
        {
            // Arrange
            var context = GetDatabaseContext();
            var controller = new VagasController(context);

            var novaVagaDto = new DTOs.Vaga.VagaCreateDto
            {
                Titulo = "Vaga Teste Visual Studio",
                Descricao = "Criada pelo teste automatizado",
                Empresa = "Empresa Teste",
                Local = "Fortaleza",
                TipoContrato = "CLT",
                Bairro = "Benfica",
                ZonaDaCidade = "Centro",
                EhVagaVerde = true,
                AceitaRemoto = false
            };

            // Act
            var resultado = await controller.PostVaga(novaVagaDto);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(resultado.Result);
            var vagaRetornada = Assert.IsType<DTOs.Vaga.VagaResponseDto>(createdResult.Value);

            Assert.Equal("Vaga Teste Visual Studio", vagaRetornada.Titulo);
            Assert.Equal("Centro", vagaRetornada.ZonaDaCidade);
        }
    }
}