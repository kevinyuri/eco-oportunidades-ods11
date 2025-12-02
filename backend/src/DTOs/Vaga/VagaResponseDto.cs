namespace TrabalhoCapacitacao.DTOs.Vaga
{
    public class VagaResponseDto
    {
        public string Id { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public string? Empresa { get; set; }
        public string? Local { get; set; }
        public string? TipoContrato { get; set; }
        public DateTime DataPublicacao { get; set; }
        public string Bairro { get; set; }
        public string ZonaDaCidade { get; set; }
        public bool EhVagaVerde { get; set; }
        public bool AceitaRemoto { get; set; }
    }

}
