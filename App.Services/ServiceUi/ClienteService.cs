using GestaoClientes.Models.DTOs;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace GestaoClientes.Services.ServiceUi
{
    public class ClienteService : ICliente
    {
        private readonly HttpClient _httpClient;
        public ClienteService(HttpClient httpClient)
        {
            _httpClient = httpClient;
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

        public async Task<GetDTO> GetListaCliente(ClienteDTOGet cliente, int? qtdItens, int? numPagina)
        {
            if (cliente == null)
                cliente = new ClienteDTOGet();

            var filtro = $"/api/cliente/get?qtdItens={qtdItens}&numPagina={numPagina}&nome={ cliente.Nome}" +
                         $"&cpf={ cliente.CPF}&sexo={cliente.Sexo}&tipoclienteid={cliente.TipoClienteId}" +
                         $"&situacaoclienteid={cliente.SituacaoClienteId}";
            try
            {
                return await _httpClient.GetFromJsonAsync<GetDTO>(filtro);
            }
            catch (Exception ex)
            {
                return new GetDTO() { MsgRetorno = ex.Message };
            }

        }

        public async Task<ClienteDTO> GetCliente(int? id)
        {
            var filtro = $"/api/cliente/getId?id={id}";
            if (id == null)
                return new ClienteDTO();

            try
            {
                var response = await _httpClient.GetFromJsonAsync<ClienteDTO>(filtro);
                return response;
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                return new ClienteDTO();
            }
        }

        public async Task<string> PostCliente(ClienteDTOPost clienteDTOPost)
        {
            var url = "api/cliente/post";
            try
            {
                var post = await _httpClient.PostAsJsonAsync(url, clienteDTOPost);

                if (post.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return null;
                }
                else
                    return post.StatusCode + " - " + post.Content.ReadFromJsonAsync<string>().ToString();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<string> PutCliente(int id, ClienteDTOPut clienteDTOPut)
        {
            var url = $"api/cliente/put/{id}";
            try
            {
                var put = await _httpClient.PutAsJsonAsync(url, clienteDTOPut);

                if (put.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return null;
                }
                else
                    return put.StatusCode + " - " + put.Content.ReadFromJsonAsync<string>().ToString();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        public async Task<string> DeleteCliente(int id, string cpf)
        {
            var url = $"api/cliente/delete/{id}/{cpf}";
            try
            {
                var del = await _httpClient.DeleteAsync(url);

                if (del.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return null;
                }
                else
                    return del.StatusCode + " - " + del.Content.ReadFromJsonAsync<string>().ToString();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }
    }
}
