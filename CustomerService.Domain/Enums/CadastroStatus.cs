namespace CustomerService.Domain.Enums
{
    public enum CadastroStatus
    {
        Pendente = 0,
        PropostaCreditoSolicitada = 1,
        PropostaCreditoAprovada = 2,
        PropostaCreditoRejeitada = 3,
        CartaoSolicitado = 4,
        CartaoEmitido = 5,
        Ativo = 6,
        Inativo = 7,
        Falha = 8
    }
}