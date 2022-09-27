
namespace models.models
{
    public class Cliente
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string CPF { get; set; }
        public TipoCliente TipoCliente { get; set; }
        public string Sexo { get; set; }
        public SituacaoCliente SituacaoCliente { get; set; }
    }
}
