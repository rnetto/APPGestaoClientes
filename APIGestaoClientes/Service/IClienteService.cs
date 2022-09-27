using GestaoClientes.Models.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APIGestaoClientes.Service
{
    public interface IClienteService
    {
        bool ValidaCliente(string nome, string cpf);
        Task<List<ClienteDTO>> GetListaCliente(ClienteDTOGet cliente, int? qtdItens, int? numPagina);
        Task<ClienteDTO> GetCliente(int? id, string cpf);
        Task<int> PostCliente(ClienteDTO cliente);
        Task<int> PutCliente(ClienteDTO cliente);
        Task DeleteCliente(int id, string cpf);
    }
}
