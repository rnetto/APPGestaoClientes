using APIGestaoClientes.DTO;
using APIGestaoClientes.Model;
using APIGestaoClientes.Service;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APIGestaoClientes.Controllers
{
    public class ClienteController : ControllerBase
    {
        private readonly ClienteService _clienteService;

        [HttpGet]
        public async Task<IActionResult> ListaCliente(int? qtdItens, int? numPagina)
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

                var listaCliente = await _clienteService.GetListaCliente(qtdItens, numPagina);

                return Ok(new { listaCliente, qtdItens, numPagina });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> BuscaCliente(ClienteDTO clienteDTO, int? id)
        {
            try
            {
                if (id != null && (id != clienteDTO.Id))
                    return BadRequest("Os Id's informados não são correspondentes.");

                var cliente = await _clienteService.GetCliente(clienteDTO);

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

        [HttpPost]
        public async Task<IActionResult> InsereCliente(ClienteDTO clienteDTO)
        {
            try
            {
                if (_clienteService.ValidaCliente(clienteDTO))
                {
                    var cliente = await _clienteService.PostCliente(clienteDTO);

                    return CreatedAtAction(
                        nameof(BuscaCliente),
                        new { id = cliente });
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

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizaCliente(int id, ClienteDTO clienteDTO)
        {
            try
            {
                if (id != clienteDTO.Id)
                {
                    return BadRequest("Os Id's informados não são correspondentes.");
                }

                if (_clienteService.ValidaCliente(clienteDTO))
                {
                    var clienteExiste = await _clienteService.GetCliente(clienteDTO);
                    if (clienteExiste.Id != 0)
                    {
                        var cliente = await _clienteService.PutCliente(clienteDTO);
                        return Ok("Alteração realizada com sucesso");
                    }
                    else
                        return NotFound("Id não encontrado na base de dados.");
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

        [HttpDelete("{id}/{cpf}")]
        public async Task<IActionResult> ApagaCliente(int id, string cpf)
        {
            try
            {
                if (cpf.Length != 11)
                {
                    return BadRequest("Os Id's informados não são correspondentes.");
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
