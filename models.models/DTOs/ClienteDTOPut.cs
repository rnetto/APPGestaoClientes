
namespace models.models.DTOs
{
    public class ClienteDTOPut
    {
        public int IdCliente { get; set; }
        public string Nome { get; set; }
        public string CPF { get; set; }
        public string Sexo { get; set; }
        public int? TipoClienteId { get; set; }
        public int? SituacaoClienteId { get; set; }
    }
}
