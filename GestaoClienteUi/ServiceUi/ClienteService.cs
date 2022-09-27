using GestaoClientes.Models.DTOs;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace GestaoClienteUi.ServiceUi
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
            var filtro = $"/api/cliente/get?qtdItens={qtdItens}&numPagina={numPagina}&nome={ cliente.Nome}" +
                         $"&cpf={ cliente.CPF}&sexo={cliente.Sexo}&tipoclienteid={cliente.TipoClienteId}" +
                         $"&situacaoclienteid={cliente.SituacaoClienteId}";

            return await _httpClient.GetFromJsonAsync<GetDTO>(filtro);
        }

        public async Task<ClienteDTO> GetCliente(int? id, string cpf)
        {
            var filtro = $"/api/cliente/getId?id={id}&cpf={cpf}";

            return await _httpClient.GetFromJsonAsync<ClienteDTO>(filtro);
        }

        public async Task<string> PostCliente(ClienteDTOPost clienteDTOPost)
        {
            var url = "api/cliente/post";            

            var post = await _httpClient.PostAsJsonAsync(url, clienteDTOPost);


            if(post.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return post.Content.ReadFromJsonAsync<string>().ToString();
            }
            else
                return post.StatusCode + post.Content.ReadFromJsonAsync<string>().ToString();
        }

        public async Task<string> PutCliente(int id,ClienteDTOPut clienteDTOPut)
        {
            var url = $"api/cliente/put/{id}";

            var put = await _httpClient.PutAsJsonAsync(url, clienteDTOPut);

            if (put.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return put.Content.ReadFromJsonAsync<string>().ToString();
            }
            else
                return put.StatusCode + put.Content.ReadFromJsonAsync<string>().ToString();
        }

        public async Task<string> DeleteCliente(int id, string cpf)
        {
            var url = $"api/cliente/delete/{id}/{cpf}";

            var delete = await _httpClient.DeleteAsync(url);
            var content = await delete.Content.ReadFromJsonAsync<string>();

            return content;
        }
    }
}
