using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrabalhoCapacitacao.Data;
using TrabalhoCapacitacao.DTOs.Vaga;
using TrabalhoCapacitacao.Models;

namespace TrabalhoCapacitacao.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VagasController : ControllerBase
{
    private readonly AppDbContext _context;

    public VagasController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/Vagas
    [HttpGet]
    public async Task<ActionResult<IEnumerable<VagaResponseDto>>> GetVagas()
    {
        var vagas = await _context.Vagas
            .Select(v => new VagaResponseDto
            {
                Id = v.Id,
                Titulo = v.Titulo,
                Descricao = v.Descricao,
                Empresa = v.Empresa,
                Local = v.Local,
                Bairro = v.Bairro,
                ZonaDaCidade = v.ZonaDaCidade,
                EhVagaVerde = v.EhVagaVerde,
                AceitaRemoto = v.AceitaRemoto,
                TipoContrato = v.TipoContrato,
                DataPublicacao = v.DataPublicacao
            })
            .ToListAsync();

        return Ok(vagas);
    }

    // GET: api/Vagas/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<VagaResponseDto>> GetVaga(string id)
    {
        var vaga = await _context.Vagas.FindAsync(id);

        if (vaga == null)
        {
            return NotFound(new { message = $"Vaga com ID {id} não encontrada." });
        }

        var vagaDto = new VagaResponseDto
        {
            Id = vaga.Id,
            Titulo = vaga.Titulo,
            Descricao = vaga.Descricao,
            Empresa = vaga.Empresa,
            Local = vaga.Local,
            Bairro = vaga.Bairro,
            ZonaDaCidade = vaga.ZonaDaCidade,
            EhVagaVerde = vaga.EhVagaVerde,
            AceitaRemoto = vaga.AceitaRemoto,
            TipoContrato = vaga.TipoContrato,
            DataPublicacao = vaga.DataPublicacao
        };

        return Ok(vagaDto);
    }

    // POST: api/Vagas
    [HttpPost]
    public async Task<ActionResult<VagaResponseDto>> PostVaga([FromBody] VagaCreateDto vagaCreateDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var vaga = new Vaga
        {
            Id = Guid.NewGuid().ToString(),
            Titulo = vagaCreateDto.Titulo,
            Descricao = vagaCreateDto.Descricao,
            Empresa = vagaCreateDto.Empresa,
            Local = vagaCreateDto.Local,
            Bairro = vagaCreateDto.Bairro,
            ZonaDaCidade = vagaCreateDto.ZonaDaCidade,
            EhVagaVerde = vagaCreateDto.EhVagaVerde,
            AceitaRemoto = vagaCreateDto.AceitaRemoto,
            TipoContrato = vagaCreateDto.TipoContrato,
            DataPublicacao = DateTime.UtcNow
        };

        _context.Vagas.Add(vaga);
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Erro ao salvar a vaga no banco de dados." });
        }

        var vagaResponseDto = new VagaResponseDto
        {
            Id = vaga.Id,
            Titulo = vaga.Titulo,
            Descricao = vaga.Descricao,
            Empresa = vaga.Empresa,
            Local = vaga.Local,
            Bairro = vaga.Bairro,
            ZonaDaCidade = vaga.ZonaDaCidade,
            EhVagaVerde = vaga.EhVagaVerde,
            AceitaRemoto = vaga.AceitaRemoto,
            TipoContrato = vaga.TipoContrato,
            DataPublicacao = vaga.DataPublicacao
        };

        return CreatedAtAction(nameof(GetVaga), new { id = vaga.Id }, vagaResponseDto);
    }

    // PUT: api/Vagas/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> PutVaga(string id, [FromBody] VagaUpdateDto vagaUpdateDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var vagaExistente = await _context.Vagas.FindAsync(id);

        if (vagaExistente == null)
        {
            return NotFound(new { message = $"Vaga com ID {id} não encontrada para atualização." });
        }

        vagaExistente.Titulo = vagaUpdateDto.Titulo;
        vagaExistente.Descricao = vagaUpdateDto.Descricao;
        vagaExistente.Empresa = vagaUpdateDto.Empresa;
        vagaExistente.Local = vagaUpdateDto.Local;

        vagaExistente.Bairro = vagaUpdateDto.Bairro;
        vagaExistente.ZonaDaCidade = vagaUpdateDto.ZonaDaCidade;
        vagaExistente.EhVagaVerde = vagaUpdateDto.EhVagaVerde;
        vagaExistente.AceitaRemoto = vagaUpdateDto.AceitaRemoto;

        vagaExistente.TipoContrato = vagaUpdateDto.TipoContrato;

        _context.Entry(vagaExistente).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!VagaExists(id))
            {
                return NotFound(new { message = "Vaga não encontrada (concorrência)." });
            }
            else
            {
                throw;
            }
        }
        catch (DbUpdateException)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Erro ao atualizar a vaga." });
        }

        return NoContent();
    }

    // DELETE: api/Vagas/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteVaga(string id)
    {
        var vaga = await _context.Vagas.FindAsync(id);
        if (vaga == null)
        {
            return NotFound(new { message = $"Vaga com ID {id} não encontrada." });
        }

        var inscricoesAssociadas = await _context.Inscricoes.AnyAsync(i => i.VagaId == id);
        if (inscricoesAssociadas)
        {
            return BadRequest(new { message = "Não é possível excluir vaga com inscrições ativas." });
        }

        _context.Vagas.Remove(vaga);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool VagaExists(string id)
    {
        return _context.Vagas.Any(e => e.Id == id);
    }
}