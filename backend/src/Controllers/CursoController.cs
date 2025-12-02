using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrabalhoCapacitacao.Data;
using TrabalhoCapacitacao.DTOs.Curso;
using TrabalhoCapacitacao.Models;

namespace TrabalhoCapacitacao.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CursosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CursosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Cursos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CursoResponseDto>>> GetCursos()
        {
            var cursos = await _context.Cursos
                .Select(c => new CursoResponseDto
                {
                    Id = c.Id,
                    Nome = c.Nome,
                    Instituicao = c.Instituicao,
                    FocadoEmSustentabilidade = c.FocadoEmSustentabilidade,
                    ImpactoComunitario = c.ImpactoComunitario,
                    CargaHoraria = c.CargaHoraria,
                    Modalidade = c.Modalidade,
                    DataInicio = c.DataInicio
                })
                .ToListAsync();
            return Ok(cursos);
        }

        // GET: api/Cursos/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<CursoResponseDto>> GetCurso(string id)
        {
            var curso = await _context.Cursos.FindAsync(id);

            if (curso == null)
            {
                return NotFound(new { message = $"Curso com ID {id} não encontrado." });
            }

            var cursoDto = new CursoResponseDto
            {
                Id = curso.Id,
                Nome = curso.Nome,
                Instituicao = curso.Instituicao,
                FocadoEmSustentabilidade = curso.FocadoEmSustentabilidade,
                ImpactoComunitario = curso.ImpactoComunitario,
                CargaHoraria = curso.CargaHoraria,
                Modalidade = curso.Modalidade,
                DataInicio = curso.DataInicio
            };

            return Ok(cursoDto);
        }

        // POST: api/Cursos
        [HttpPost]
        public async Task<ActionResult<CursoResponseDto>> PostCurso([FromBody] CursoCreateDto cursoCreateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var curso = new Curso
            {
                Id = Guid.NewGuid().ToString(),
                Nome = cursoCreateDto.Nome,
                Instituicao = cursoCreateDto.Instituicao,
                FocadoEmSustentabilidade = cursoCreateDto.FocadoEmSustentabilidade,
                ImpactoComunitario = cursoCreateDto.ImpactoComunitario,
                CargaHoraria = cursoCreateDto.CargaHoraria,
                Modalidade = cursoCreateDto.Modalidade,
                DataInicio = cursoCreateDto.DataInicio
            };

            _context.Cursos.Add(curso);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Erro ao salvar curso: {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Erro ao salvar o curso no banco de dados." });
            }

            var cursoResponseDto = new CursoResponseDto
            {
                Id = curso.Id,
                Nome = curso.Nome,
                Instituicao = curso.Instituicao,
                FocadoEmSustentabilidade = curso.FocadoEmSustentabilidade,
                ImpactoComunitario = curso.ImpactoComunitario,
                CargaHoraria = curso.CargaHoraria,
                Modalidade = curso.Modalidade,
                DataInicio = curso.DataInicio
            };

            return CreatedAtAction(nameof(GetCurso), new { id = curso.Id }, cursoResponseDto);
        }

        // PUT: api/Cursos/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCurso(string id, [FromBody] CursoUpdateDto cursoUpdateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var cursoExistente = await _context.Cursos.FindAsync(id);

            if (cursoExistente == null)
            {
                return NotFound(new { message = $"Curso com ID {id} não encontrado para atualização." });
            }

            cursoExistente.Nome = cursoUpdateDto.Nome;
            cursoExistente.Instituicao = cursoUpdateDto.Instituicao;

            cursoExistente.FocadoEmSustentabilidade = cursoUpdateDto.FocadoEmSustentabilidade;
            cursoExistente.ImpactoComunitario = cursoUpdateDto.ImpactoComunitario;

            cursoExistente.CargaHoraria = cursoUpdateDto.CargaHoraria;
            cursoExistente.Modalidade = cursoUpdateDto.Modalidade;
            cursoExistente.DataInicio = cursoUpdateDto.DataInicio;

            _context.Entry(cursoExistente).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!await CursoExistsAsync(id))
                {
                    return NotFound(new { message = "Curso não encontrado (concorrência)." });
                }
                else
                {
                    Console.WriteLine($"Erro de concorrência: {ex}");
                    return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Erro de concorrência ao atualizar o curso." });
                }
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Erro ao atualizar: {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Erro ao atualizar o curso no banco de dados." });
            }

            return NoContent();
        }

        // DELETE: api/Cursos/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCurso(string id)
        {
            var curso = await _context.Cursos.FindAsync(id);
            if (curso == null)
            {
                return NotFound(new { message = $"Curso com ID {id} não encontrado para exclusão." });
            }

            var inscricoesAssociadas = await _context.Inscricoes.AnyAsync(i => i.CursoId == id);
            if (inscricoesAssociadas)
            {
                return BadRequest(new { message = "Não é possível excluir o curso pois existem inscrições associadas a ele." });
            }

            _context.Cursos.Remove(curso);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Erro ao excluir: {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Erro ao excluir o curso no banco de dados." });
            }

            return NoContent();
        }

        private async Task<bool> CursoExistsAsync(string id)
        {
            return await _context.Cursos.AnyAsync(e => e.Id == id);
        }
    }
}