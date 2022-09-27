using System.Collections.Generic;

namespace GestaoClientes.Models.DTOs
{
    public class GetDTO
    {
        public int? NumPagina { get; set; }
        public int? ItensPagina { get; set; }
        public List<ClienteDTO> ListaClientes { get; set; }
        public string MsgRetorno { get; set; }
    }
}
