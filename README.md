# API de Vendas

## Estrutura

### Models
- Possui duas classes, Venda e Vendedor.
- Possui um enum que armazena os status das vendas.

### Controller
Contém os métodos:

- HttpPost: CadastrarVendedor.
- HttpPut: RegistrarVenda.
- HttpGet: ObterVendaPorId.
- HttpPatch: AtualizarStatus.

### Estrutura de Testes
No projeto TestesDesafioPaymentApi instalamos a biblioteca xUnit para a realização de testes sobre o nosso projeto principal. Todos os testes realizados obteram sucesso.
