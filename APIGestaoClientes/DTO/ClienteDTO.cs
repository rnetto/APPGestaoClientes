using APIGestaoClientes.Model;

namespace APIGestaoClientes.DTO
{
    public class ClienteDTO
    {
        public int? IdCliente { get; set; }
        public string Nome { get; set; }
        public string CPF { get; set; }
        public TipoCliente TipoCliente { get; set; }
        public string Sexo { get; set; }
        public SituacaoCliente SituacaoCliente { get; set; }
    }
}
