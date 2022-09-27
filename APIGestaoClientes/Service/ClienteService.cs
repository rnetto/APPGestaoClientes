using GestaoClientes.Models.DTOs;
using GestaoClientes.Models.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace APIGestaoClientes.Service
{
    public class ClienteService : IClienteService
    {
        private readonly string _connectionString;
        public ClienteService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public bool ValidaCliente(string nome, string cpf)
        {
            if (nome == null || cpf == null)
                return false;

            char[] removChar = { ' ', '-', '*', '.', '!', '@', '#', '$', '%', '¨', '&', '(', ')', '_', '+', '{', '}', '?', ';', ':', '.', '>', '<', '|', '/' };
            var nomeTrim = nome.TrimEnd(removChar);
            var cpfTrim = cpf.TrimEnd(removChar);

            if (cpfTrim.Length != 11)
                return false;

            if (nomeTrim.Length < 3)
                return false;

            return true;
        }

        public async Task<List<ClienteDTO>> GetListaCliente(ClienteDTOGet cliente, int? qtdItens, int? numPagina)
        {
            var clientesR = new List<ClienteDTO>();
            var query = String.Format(@"stp_Get_Cliente");
            using (var conn = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(query, conn);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("nome", cliente.Nome == null ? DBNull.Value : cliente.Nome));
                command.Parameters.Add(new SqlParameter("cpf", cliente.CPF == null ? DBNull.Value : cliente.CPF));
                command.Parameters.Add(new SqlParameter("sexo", cliente.Sexo == null ? DBNull.Value : cliente.Sexo));
                command.Parameters.Add(new SqlParameter("id_situacao_cliente", cliente.SituacaoClienteId == null ? DBNull.Value : cliente.SituacaoClienteId));
                command.Parameters.Add(new SqlParameter("situacao_cliente_descricao", cliente.DescricaoSituacaoCliente == null ? DBNull.Value : cliente.DescricaoSituacaoCliente));
                command.Parameters.Add(new SqlParameter("id_tipo_cliente", cliente.TipoClienteId == null ? DBNull.Value : cliente.TipoClienteId));
                command.Parameters.Add(new SqlParameter("tipo_cliente_descricao", cliente.DescricaoTipoCliente == null ? DBNull.Value : cliente.DescricaoTipoCliente));
                command.Parameters.Add(new SqlParameter("pagnum", numPagina == null ? DBNull.Value : (int)numPagina));
                command.Parameters.Add(new SqlParameter("itemsqtd", qtdItens == null ? DBNull.Value : qtdItens));

                await conn.OpenAsync();

                try
                {
                    var retorno = await command.ExecuteReaderAsync();
                    if (retorno.FieldCount == 3)
                    {
                        while (await retorno.ReadAsync())
                        {
                            string msg;
                            if (retorno["id_status"].ToString() == "400")
                            {
                                msg = retorno["id_status"].ToString() + " - " + retorno["status_code"].ToString();
                                throw new Exception(msg);
                            }
                            else
                            {
                                clientesR = null;
                            }
                        }
                    }
                    else
                    {
                        while (await retorno.ReadAsync())
                        {
                            clientesR.Add(new ClienteDTO
                            {
                                IdCliente = Convert.ToInt32(retorno["id_cliente"].ToString()),
                                Nome = retorno["nome"].ToString(),
                                CPF = retorno["cpf"].ToString(),
                                Sexo = retorno["sexo"].ToString(),
                                SituacaoCliente = new SituacaoCliente
                                {
                                    Id = Convert.ToInt32(retorno["id_situacao_cliente"].ToString()),
                                    DescricaoSituacao = retorno["descricao_situacao_cliente"].ToString()
                                },
                                TipoCliente = new TipoCliente
                                {
                                    Id = Convert.ToInt32(retorno["id_tipo_cliente"].ToString()),
                                    DescricaoTipoCliente = retorno["descricao_tipo_cliente"].ToString()
                                }
                            });
                        }
                    }

                    await retorno.CloseAsync();

                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

            return clientesR;
        }
        
        public async Task<ClienteDTO> GetCliente(int? id, string cpf)
        {
            var clienteR = new ClienteDTO();
            var query = String.Format(@"stp_Get_Cliente");

            using (var conn = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(query, conn);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("id_cliente", id == null ? DBNull.Value : (int)id));
                command.Parameters.Add(new SqlParameter("cpf", cpf == null ? DBNull.Value : cpf));
                
                await conn.OpenAsync();

                try
                {
                    var retorno = await command.ExecuteReaderAsync();
                    while (await retorno.ReadAsync())
                    {
                        clienteR.IdCliente = Convert.ToInt32(retorno["id_cliente"].ToString());
                        if (clienteR.IdCliente != 0)
                        {
                            clienteR.Nome = retorno["nome"].ToString();
                            clienteR.CPF = retorno["cpf"].ToString();
                            clienteR.Sexo = retorno["sexo"].ToString();
                            clienteR.SituacaoCliente = new SituacaoCliente
                            {
                                Id = Convert.ToInt32(retorno["id_situacao_cliente"].ToString()),
                                DescricaoSituacao = retorno["descricao_situacao_cliente"].ToString()
                            };
                            clienteR.TipoCliente = new TipoCliente
                            {
                                Id = Convert.ToInt32(retorno["id_tipo_cliente"].ToString()),
                                DescricaoTipoCliente = retorno["descricao_tipo_cliente"].ToString()
                            };
                        }
                        else
                        {
                            string msg;
                            if (retorno["id_status"].ToString() == "400")
                            {
                                msg = retorno["id_status"].ToString() + " - " + retorno["status_code"].ToString();
                                throw new Exception(msg);
                            }
                            else
                            {
                                clienteR = null;
                            }

                        }
                    }
                    await retorno.CloseAsync();

                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

            return clienteR;
        }
        
        public async Task<int> PostCliente(ClienteDTO cliente)
        {
            var clienteR = 0;
            var statusCode = 0;
            var msg = "";

            var query = String.Format(@"stp_Post_Cliente");
            using (var conn = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(query, conn);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("nome", cliente.Nome == null ? DBNull.Value : cliente.Nome));
                command.Parameters.Add(new SqlParameter("cpf", cliente.CPF == null ? DBNull.Value : cliente.CPF));
                command.Parameters.Add(new SqlParameter("sexo", cliente.Sexo == null ? DBNull.Value : cliente.Sexo));
                command.Parameters.Add(new SqlParameter("id_situacao_cliente", cliente.SituacaoCliente.Id == null ? DBNull.Value : cliente.SituacaoCliente.Id));
                command.Parameters.Add(new SqlParameter("id_tipo_cliente", cliente.TipoCliente.Id == null ? DBNull.Value : cliente.TipoCliente.Id));

                await conn.OpenAsync();
                var tran = conn.BeginTransaction(IsolationLevel.ReadUncommitted);
                command.Transaction = tran;

                try
                {
                    var retorno = await command.ExecuteReaderAsync();

                    while (await retorno.ReadAsync())
                    {
                        clienteR = Convert.ToInt32(retorno["id_cliente"].ToString());
                        statusCode = Convert.ToInt32(retorno["id_status"].ToString());
                        msg = retorno["msg"].ToString();
                    }

                    await retorno.CloseAsync();

                    if (clienteR == 0 && statusCode == 400)
                    {
                        throw new Exception("Não foi possível inserir os dados do cliente. " + msg);
                    }

                    await tran.CommitAsync();

                    return clienteR;
                }
                catch (Exception ex)
                {
                    await tran.RollbackAsync();
                    throw new Exception(ex.Message);
                }
                finally
                {
                    await conn.DisposeAsync();
                    await conn.CloseAsync();
                }
            }
        }

        public async Task<int> PutCliente(ClienteDTO cliente)
        {
            var clienteR = 0;
            var msg = "";
            var statusCode = 0;

            var query = String.Format(@"stp_Put_Cliente");
            using (var conn = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(query, conn);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("id_cliente", cliente.IdCliente == null ? DBNull.Value : (int)cliente.IdCliente));
                command.Parameters.Add(new SqlParameter("nome", cliente.Nome == null ? DBNull.Value : cliente.Nome));
                command.Parameters.Add(new SqlParameter("cpf", cliente.CPF == null ? DBNull.Value : cliente.CPF));
                command.Parameters.Add(new SqlParameter("sexo", cliente.Sexo == null ? DBNull.Value : cliente.Sexo));
                command.Parameters.Add(new SqlParameter("id_situacao_cliente", cliente.SituacaoCliente.Id == null ? DBNull.Value : cliente.SituacaoCliente.Id));
                command.Parameters.Add(new SqlParameter("id_tipo_cliente", cliente.TipoCliente.Id == null ? DBNull.Value : cliente.TipoCliente.Id));

                await conn.OpenAsync();
                var tran = conn.BeginTransaction(IsolationLevel.ReadUncommitted);
                command.Transaction = tran;

                try
                {
                    var retorno = await command.ExecuteReaderAsync();

                    while (await retorno.ReadAsync())
                    {
                        clienteR = Convert.ToInt32(retorno["id_cliente"].ToString());
                        statusCode = Convert.ToInt32(retorno["id_status"].ToString());
                        msg = retorno["msg"].ToString();
                    }

                    await retorno.CloseAsync();

                    if (clienteR == 0 && statusCode == 400)
                    {
                        throw new Exception("Não foi possível alterar os dados do cliente. " + msg);
                    }

                    await tran.CommitAsync();

                    return clienteR;
                }
                catch (Exception ex)
                {
                    await tran.RollbackAsync();
                    throw new Exception(ex.Message);
                }
                finally
                {
                    await conn.DisposeAsync();
                    await conn.CloseAsync();
                }
            }
        }

        public async Task DeleteCliente(int id, string cpf)
        {
            var statusCode = 0;
            var msg = "";
            var query = String.Format(@"stp_Delete_Cliente");
            using (var conn = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(query, conn);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("id_cliente", id));
                command.Parameters.Add(new SqlParameter("cpf", cpf));

                await conn.OpenAsync();
                var tran = conn.BeginTransaction(IsolationLevel.ReadUncommitted);
                command.Transaction = tran;

                try
                {
                    var retorno = await command.ExecuteReaderAsync();
                    while (await retorno.ReadAsync())
                    {
                        statusCode = Convert.ToInt32(retorno["id_status"].ToString());
                        msg = retorno["msg"].ToString();
                    }
                    await retorno.CloseAsync();

                    if (statusCode == 400)
                    {
                        throw new Exception(msg);
                    }

                    await tran.CommitAsync();
                    await conn.CloseAsync();
                }
                catch (Exception ex)
                {
                    await tran.RollbackAsync();
                    throw new Exception(ex.Message);
                }
                finally
                {
                    await conn.DisposeAsync();
                    await conn.CloseAsync();
                }
            }
        }
    }
}
