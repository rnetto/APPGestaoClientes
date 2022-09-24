using APIGestaoClientes.DTO;
using APIGestaoClientes.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace APIGestaoClientes.Service
{
    public class ClienteService
    {
        internal bool ValidaCliente(ClienteDTO cliente)
        {
            char[] removChar = { ' ', '-', '*', '.', '!', '@', '#', '$', '%', '¨', '&', '(', ')', '_', '+', '{', '}', '?', ';', ':', '.', '>', '<', '|', '/' };
            var nome = cliente.Nome.TrimEnd(removChar);
            var cpf = cliente.CPF.TrimEnd(removChar);

            if (cpf.Length != 11)
                return false;

            if (nome.Length < 3)
                return false;

            return true;
        }

        internal async Task<ClienteDTO> GetCliente(ClienteDTO cliente)
        {
            var clienteR = new ClienteDTO();
            var query = String.Format(@"stp_Get_Cliente");
            using (var conn = new SqlConnection())
            {
                var command = new SqlCommand(query, conn);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("id_cliente", cliente.Id == null ? DBNull.Value : (int)cliente.Id));
                command.Parameters.Add(new SqlParameter("nome", cliente.Nome == null ? DBNull.Value : cliente.Nome));
                command.Parameters.Add(new SqlParameter("cpf", cliente.CPF == null ? DBNull.Value : cliente.CPF));
                command.Parameters.Add(new SqlParameter("sexo", cliente.Sexo == null ? DBNull.Value : cliente.Sexo));
                command.Parameters.Add(new SqlParameter("id_situacao_cliente", cliente.SituacaoCliente.Id == null ? DBNull.Value : cliente.SituacaoCliente.Id));
                command.Parameters.Add(new SqlParameter("id_tipo_cliente", cliente.TipoCliente.Id == null ? DBNull.Value : cliente.TipoCliente.Id));

                await conn.OpenAsync();

                try
                {
                    var retorno = await command.ExecuteReaderAsync();
                    while (await retorno.ReadAsync())
                    {
                        clienteR.Id = Convert.ToInt32(retorno["id_cliente"].ToString());
                        if (clienteR.Id != 0)
                        {
                            clienteR.Nome = retorno["nome"].ToString();
                            clienteR.CPF = retorno["cpf"].ToString();
                            clienteR.Sexo = retorno["sexo"].ToString();
                            clienteR.SituacaoCliente.Id = Convert.ToInt32(retorno["id_situacao_cliente"].ToString());
                            clienteR.SituacaoCliente.DescricaoSituacao = retorno["descricao_situacao_cliente"].ToString();
                            clienteR.TipoCliente.Id = Convert.ToInt32(retorno["id_tipo_cliente"].ToString());
                            clienteR.TipoCliente.DescricaoTipoCliente = retorno["descricao_tipo_cliente"].ToString();
                        }
                        else
                        {
                            string msg = retorno["id_status"].ToString() + " - " + retorno["status_code"].ToString();
                            throw new Exception(msg);
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

        internal async Task<List<ClienteDTO>> GetListaCliente(int? qtdItens, int? numPagina)
        {
            var clientesR = new List<ClienteDTO>();
            var query = String.Format(@"stp_Get_Cliente");
            using (var conn = new SqlConnection())
            {
                var command = new SqlCommand(query, conn);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("pagnum", numPagina == null ? DBNull.Value : (int)numPagina));
                command.Parameters.Add(new SqlParameter("itemsqtd", qtdItens == null ? DBNull.Value : qtdItens));

                await conn.OpenAsync();

                try
                {
                    var retorno = await command.ExecuteReaderAsync();
                    while (await retorno.ReadAsync())
                    {
                        clientesR.Add(new ClienteDTO
                        {
                            Id = Convert.ToInt32(retorno["id_cliente"].ToString()),
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
                    await retorno.CloseAsync();

                    if (clientesR.FirstOrDefault().Id == 0)
                    {
                        string msg = retorno["id_status"].ToString() + " - " + retorno["status_code"].ToString();
                        throw new Exception(msg);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

            return clientesR;
        }

        internal async Task<int> PostCliente(ClienteDTO cliente)
        {
            var clienteR = 0;
            var query = String.Format(@"stp_Post_Cliente");
            using (var conn = new SqlConnection())
            {
                var command = new SqlCommand(query, conn);
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
                    }

                    await retorno.CloseAsync();
                    
                    return clienteR;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw new Exception(ex.Message);                                       
                }
                finally
                {
                    conn.Dispose();
                    conn.Close();
                }
            }
        }

        internal async Task<int> PutCliente(ClienteDTO cliente)
        {
            var clienteR = 0;
            var query = String.Format(@"stp_Put_Cliente");
            using (var conn = new SqlConnection())
            {
                var command = new SqlCommand(query, conn);
                command.Parameters.Add(new SqlParameter("id_cliente", cliente.Id == null ? DBNull.Value : (int)cliente.Id));
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
                    }

                    await retorno.CloseAsync();

                    return clienteR;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw new Exception(ex.Message);
                }
                finally
                {
                    conn.Dispose();
                    conn.Close();
                }
            }
        }

        internal async Task DeleteCliente(int id, string cpf)
        {           
            var query = String.Format(@"stp_Delete_Cliente");
            using (var conn = new SqlConnection())
            {
                var command = new SqlCommand(query, conn);
                command.Parameters.Add(new SqlParameter("id_cliente", id));
                command.Parameters.Add(new SqlParameter("cpf", cpf));

                await conn.OpenAsync();
                var tran = conn.BeginTransaction(IsolationLevel.ReadUncommitted);
                command.Transaction = tran;

                try
                {
                    await command.ExecuteNonQueryAsync();
                    await conn.CloseAsync();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw new Exception(ex.Message);
                }
                finally
                {
                    conn.Dispose();
                    conn.Close();
                }
            }
        }
    }
}
