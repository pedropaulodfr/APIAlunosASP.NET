using AlunosAPI.Models;
using AlunosAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlunosAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlunosController : ControllerBase
    {
        private IAlunoService _alunoService;

        public AlunosController(IAlunoService alunoService)
        {
            _alunoService = alunoService;
        }

        [HttpGet]
        public async Task<ActionResult<IAsyncEnumerable<Aluno>>> GetAlunos()
        {
            try 
            { 
                var alunos = await _alunoService.GetAlunos();
                return Ok(alunos);
            }
            catch 
            {
                return BadRequest("Request inválido!");
            }
        }

        [HttpGet("AlunosNome")]
        public async Task<ActionResult<IAsyncEnumerable<Aluno>>> 
            GetAlunosByName([FromQuery] string nome)
        {

            try
            {
                var alunos = await _alunoService.GetAlunosByNome(nome);

                if (alunos.Count() == 0)
                {
                    return NotFound($"Não existem alunos com o critério {nome}");
                }

                return Ok(alunos);
            }
            catch
            {
                return BadRequest("Request inválido!");
            }

        }

        [HttpGet("{id:int}", Name = "GetAluno")]
        public async Task<ActionResult<Aluno>>
            GetAluno(int id)
        {

            try
            {
                var aluno = await _alunoService.GetAluno(id);

                if (aluno == null)
                {
                    return NotFound($"Não existe aluno com o id = {id}");
                }

                return Ok(aluno);
            }
            catch
            {
                return BadRequest("Request inválido!");
            }

        }

        [HttpPost]
        public async Task<ActionResult> Create(Aluno aluno)
        {
            try
            {
                await _alunoService.CreateAluno(aluno);
                return CreatedAtRoute(nameof(GetAluno), new { id = aluno.Id }, aluno);
            }
            catch
            {
                return BadRequest("Request inválido!");
            }
        }

        [HttpPut("AlterarAluno/{id:int}")]
        public async Task<ActionResult> Update(int id, [FromBody] Aluno aluno)
        {
            try
            {
                if(aluno.Id == id)
                {
                    await _alunoService.UpdateAluno(aluno);
                    return Ok($"Aluno com id = {id} foi atualizado com sucesso!");
                }
                else
                {
                    return BadRequest("Dados inconsistentes!");
                }
            }
            catch
            {
                return BadRequest("Request inválido!");
            }
        }

        [HttpDelete("DeletarAluno/{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var aluno = await _alunoService.GetAluno(id);
                if (aluno != null)
                {
                    await _alunoService.DeleteAluno(aluno);
                    return Ok($"Aluno de id = {id} foi excluído com sucesso!");
                }
                else {
                    return NotFound($"Aluno com id = {id} não encontrado!");
                }
            }
            catch
            {
                return BadRequest("Request inválido!");
            }
        }
    }
}
