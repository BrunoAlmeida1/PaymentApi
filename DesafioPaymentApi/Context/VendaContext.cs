using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DesafioPaymentApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DesafioPaymentApi.Context
{
    public class VendaContext : DbContext
    {
        public VendaContext() {}
        public VendaContext(DbContextOptions<VendaContext> options) : base(options) {}

        public DbSet<Venda> Vendas { get; set; }
        public DbSet<Vendedor> Vendedores { get; set; }
    }
}