using System.Text.Json.Serialization;

namespace DesafioPaymentApi.Models
{
    public enum StatusDaVenda
    {
        AguardandoPagamento,
        PagamentoAprovado,
        EnviadoParaTransportadora,
        Entregue,
        Cancelada
    }
}