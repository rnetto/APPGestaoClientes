using System.ComponentModel.DataAnnotations;

namespace GestaoClientes.Models.DTOs
{
    public class ClienteFormDTO
    {
        public int? IdCliente { get; set; }
        [Required(ErrorMessage ="Campo 'NOME' é brigatório."), MinLength(3, ErrorMessage = "Inválido, mínimo {1}."), MaxLength(250, ErrorMessage = "Inválido, máximo {1}.")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "Campo 'CPF' é brigatório."), MinLength(11, ErrorMessage = "Inválido, mínimo {1}."), MaxLength(11, ErrorMessage = "Inválido, máximo {1}.")]
        public string CPF { get; set; }
        public string Sexo { get; set; }
        public int? IdTipoCliente{ get; set; }
        public int? IdSituacaoCliente { get; set; }
    }
}
