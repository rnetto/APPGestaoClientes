using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIGestaoClientes.DTO
{
    public class ClienteDTOPost
    {
        public string Nome { get; set; }
        public string CPF { get; set; }
        public int? TipoClienteId { get; set; }
        public string Sexo { get; set; }
        public int? SituacaoClienteId { get; set; }
    }
}
