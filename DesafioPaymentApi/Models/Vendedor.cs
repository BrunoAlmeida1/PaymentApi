using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DesafioPaymentApi.Models
{
    public class Vendedor
    {
        public Vendedor() {}
        public Vendedor(string nome, string cpf, string email, string fone)
        {
            Nome = nome;
            Cpf = cpf;
            Email = email;
            Fone = fone;
        }
        public int VendedorId { get; set; }
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public string Email { get; set; }
        public string Fone { get; set; }
        public ICollection<Venda> Vendas { get; set; }
    }
}