using GestaoClientes.Models.DTOs;
using System.Threading.Tasks;

namespace GestaoClientes.Services.ServiceUi
{
    public interface ICliente
    {
        bool ValidaCliente(string nome, string cpf);
        Task<GetDTO> GetListaCliente(ClienteDTOGet cliente, int? qtdItens, int? numPagina);
        Task<ClienteDTO> GetCliente(int? id);
        Task<string> PostCliente(ClienteDTOPost cliente);
        Task<string> PutCliente(int id, ClienteDTOPut cliente);
        Task<string> DeleteCliente(int id, string cpf);
    }
}
