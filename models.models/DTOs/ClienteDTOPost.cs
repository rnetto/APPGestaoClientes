﻿
namespace models.models.DTOs
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
