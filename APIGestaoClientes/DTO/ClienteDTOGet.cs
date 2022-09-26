
namespace APIGestaoClientes.DTO
{
    public class ClienteDTOGet
    {
        public string Nome { get; set; }
        public string CPF { get; set; }
        public string Sexo { get; set; }
        public int? TipoClienteId { get; set; }
        public string DescricaoTipoCliente { get; set; }

        public int? SituacaoClienteId { get; set; }
        public string DescricaoSituacaoCliente { get; set; }
    }
}

