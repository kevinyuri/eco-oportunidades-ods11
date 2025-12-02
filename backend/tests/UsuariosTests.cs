using Xunit;
using Microsoft.EntityFrameworkCore;
using TrabalhoCapacitacao.Controllers;
using TrabalhoCapacitacao.Data;
using TrabalhoCapacitacao.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace TrabalhoCapacitacao.Tests
{
    public class FakePasswordHasher : IPasswordHasher<Usuario>
    {
        public string HashPassword(Usuario user, string password) => "hashed_" + password;
        public PasswordVerificationResult VerifyHashedPassword(Usuario user, string hashedPassword, string providedPassword)
        {
            return (hashedPassword == "hashed_" + providedPassword)
                ? PasswordVerificationResult.Success
                : PasswordVerificationResult.Failed;
        }
    }

    public class FakeConfiguration : IConfiguration
    {
        public string this[string key]
        {
            get => "chave_super_secreta_para_testes_apenas_123456"; // Chave falsa para o JWT
            set => throw new NotImplementedException();
        }
        public IConfigurationSection GetSection(string key) => null;
        public IEnumerable<IConfigurationSection> GetChildren() => null;
        public Microsoft.Extensions.Primitives.IChangeToken GetReloadToken() => null;
    }

    public class UsuariosTests
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
        public async Task RegistrarUsuario_DeveSalvarNoBanco_ComBairroODS11()
        {
            // Arrange
            var context = GetDatabaseContext();
            var hasher = new FakePasswordHasher();
            var config = new FakeConfiguration();

            var controller = new UsuariosController(context, hasher, config);

            var novoUsuario = new DTOs.Usuario.UsuarioCreateDto
            {
                Nome = "Kevin Teste",
                Email = "kevin@teste.com",
                Senha = "123",
                Perfil = "candidato",
                BairroResidencia = "Bom Jardim", // Testando ODS 11
                Telefone = "8599999999"
            };

            // Act
            var resultado = await controller.RegistrarUsuario(novoUsuario);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(resultado.Result);
            var usuarioRetornado = Assert.IsType<DTOs.Usuario.UsuarioResponseDto>(createdResult.Value);

            Assert.Equal("Bom Jardim", usuarioRetornado.BairroResidencia);
            Assert.Equal("kevin@teste.com", usuarioRetornado.Email);

            // Verifica se salvou no banco
            var usuarioNoBanco = await context.Usuarios.FirstOrDefaultAsync(u => u.Email == "kevin@teste.com");
            Assert.NotNull(usuarioNoBanco);
            Assert.Equal("hashed_123", usuarioNoBanco.SenhaHash); // Verifica se usou o nosso hasher falso
        }

        [Fact]
        public async Task GetUsuarios_DeveRetornarLista()
        {
            // Arrange
            var context = GetDatabaseContext();

            context.Usuarios.Add(new Usuario
            {
                Id = "1",
                Nome = "User1",
                Email = "u1@t.com",
                BairroResidencia = "Centro",
                Perfil = "candidato",
                SenhaHash = "hash_fake_123",
                Telefone = "85988887777"
            });
            context.SaveChanges();

            var controller = new UsuariosController(context, new FakePasswordHasher(), new FakeConfiguration());

            // Act
            var resultado = await controller.GetUsuarios();

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(resultado.Result);
            var lista = Assert.IsType<List<DTOs.Usuario.UsuarioResponseDto>>(actionResult.Value);
            Assert.Single(lista);
        }
    }
}