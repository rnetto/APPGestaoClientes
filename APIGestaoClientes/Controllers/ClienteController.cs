using APIGestaoClientes.DTO;
using APIGestaoClientes.Model;
using APIGestaoClientes.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace APIGestaoClientes.Controllers
{
    [Route("api/[controller]")]
    public class ClienteController : ControllerBase
    {
        private readonly IClienteService _clienteService;
        public ClienteController(IConfiguration configuration)
        {
            _clienteService = new ClienteService(configuration);
        }

        [HttpGet("get")]
        public async Task<IActionResult> ListaCliente(int? qtdItens, int? numPagina, ClienteDTOGet clienteDTOGet)
        {
            try
            {
                if (!qtdItens.HasValue || qtdItens > 100)
                {
                    qtdItens = 100;
                }

                if (!numPagina.HasValue)
                {
                    numPagina = 1;
                }

                var clienteDTO = new ClienteDTO()
                {
                    Nome = clienteDTOGet.Nome,
                    CPF = clienteDTOGet.CPF,
                    Sexo = clienteDTOGet.Sexo,
                    TipoCliente = new TipoCliente
                    { DescricaoTipoCliente = clienteDTOGet.DescricaoTipoCliente, Id = clienteDTOGet.TipoClienteId },
                    SituacaoCliente = new SituacaoCliente
                    { DescricaoSituacao = clienteDTOGet.DescricaoSituacaoCliente, Id = clienteDTOGet.SituacaoClienteId }
                };

                var listaCliente = await _clienteService.GetListaCliente(clienteDTOGet, qtdItens, numPagina);

                if (listaCliente == null)
                {
                    return NotFound("Não há clientes registrados na base de dados");
                }

                return Ok(new { listaCliente, qtdItens, numPagina });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("getId")]
        public async Task<IActionResult> BuscaCliente(int? id, string cpf)
        {
            try
            {
                if(cpf == null && id == null)
                {
                    throw new Exception("Favor inserir Id e/ou CPF válidos para realizar a busca.");
                }
                if(cpf != null && cpf.Length != 11)
                {
                    throw new Exception("Favor inserir CPF válido para realizar a busca.");
                }

                var cliente = await _clienteService.GetCliente(id, cpf);

                if (cliente == null)
                {
                    return NotFound("Cliente não encontrado na base de dados");
                }

                return Ok(cliente);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("post")]
        public async Task<IActionResult> InsereCliente(ClienteDTOPost clienteDTOPost)
        {
            try
            {
                if (_clienteService.ValidaCliente(clienteDTOPost.Nome, clienteDTOPost.CPF))
                {
                    var clienteDTO = new ClienteDTO()
                    {
                        Nome = clienteDTOPost.Nome,
                        CPF = clienteDTOPost.CPF,
                        Sexo = clienteDTOPost.Sexo,
                        TipoCliente = new TipoCliente
                        { DescricaoTipoCliente = null, Id = clienteDTOPost.TipoClienteId },
                        SituacaoCliente = new SituacaoCliente
                        { DescricaoSituacao = null, Id = clienteDTOPost.SituacaoClienteId }
                    };
                
                    var cliente = await _clienteService.PostCliente(clienteDTO);

                    return CreatedAtAction(
                        nameof(BuscaCliente),
                        new { Id = cliente });
                }
                else
                {
                    return BadRequest("Inserção não realizada. Campos inválidos (NOME, CPF).");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPut("put/{id}")]
        public async Task<IActionResult> AtualizaCliente(int id, ClienteDTOPut clienteDTOPut)
        {
            try
            {
                if (id != clienteDTOPut.IdCliente)
                {
                    return BadRequest("Os Id's informados não são correspondentes.");
                }

                if (_clienteService.ValidaCliente(clienteDTOPut.Nome, clienteDTOPut.CPF))
                {
                    var clienteDTO = new ClienteDTO()
                    {
                        IdCliente = id,
                        Nome = clienteDTOPut.Nome,
                        CPF = clienteDTOPut.CPF,
                        Sexo = clienteDTOPut.Sexo,
                        TipoCliente = new TipoCliente
                        { DescricaoTipoCliente = null, Id = clienteDTOPut.TipoClienteId },
                        SituacaoCliente = new SituacaoCliente
                        { DescricaoSituacao = null, Id = clienteDTOPut.SituacaoClienteId }
                    };
                    
                        var cliente = await _clienteService.PutCliente(clienteDTO);
                        return Ok("Alteração realizada com sucesso");
                }
                else
                {
                    return BadRequest("Atualização não realizada. Campos inválidos (NOME, CPF).");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("delete/{id}/{cpf}")]
        public async Task<IActionResult> ApagaCliente(int id, string cpf)
        {
            try
            {
                if (cpf.Length != 11)
                {
                    return BadRequest("CPF inválido.");
                }

                await _clienteService.DeleteCliente(id, cpf);

                return Ok("Registro do cliente apagado com sucesso!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
