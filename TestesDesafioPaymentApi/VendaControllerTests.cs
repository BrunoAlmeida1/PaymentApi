using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DesafioPaymentApi.Controllers;
using DesafioPaymentApi.Models;
using Microsoft.AspNetCore.JsonPatch;

namespace TestesDesafioPaymentApi
{
    public class VendaControllerTests
    {
        VendaController _vendaController;
        [Fact]
        public void DeveRegistrar2VendedoresERetornar2EOCampoNomeDoPrimeiro()
        {
            var factory = new ConnectionFactory();
            var context = factory.CreateContextForSQLite();
            _vendaController = new VendaController(context);

            var vendedor = new Vendedor("testSqlite", "000.000.000-00", "sqlite@email.com", "0000-0000");
            _vendaController.CadastrarVendedor(vendedor);

            var vendedor2 = new Vendedor("testSqlite2", "000.000.000-02", "sqlite2@email.com", "0000-0002");
            _vendaController.CadastrarVendedor(vendedor2);
            context.SaveChanges();

            var vendedoresCount = context.Vendedores.Count();
            Assert.Equal(2, vendedoresCount);

            var singleVendedor = context.Vendedores.FirstOrDefault();
            Assert.Equal("testSqlite", singleVendedor.Nome);
        }

        [Fact]
        public void DeveRegistrar2VendasERetornar2EOCampoItensDaPrimeira()
        {
            var factory = new ConnectionFactory();
            var context = factory.CreateContextForSQLite();
            _vendaController = new VendaController(context);

            var vendedor = new Vendedor("testSqlite", "000.000.000-00", "sqlite@email.com", "0000-0000");
            _vendaController.CadastrarVendedor(vendedor);

            var venda = new Venda("ItemTest", 0, 1);
            _vendaController.RegistrarVenda(venda);

            var venda2 = new Venda("ItemTest2", 0, 1);
            _vendaController.RegistrarVenda(venda2);
            context.SaveChanges();

            var vendasCount = context.Vendas.Count();
            Assert.Equal(2, vendasCount);

            var singleVenda = context.Vendas.FirstOrDefault();
            Assert.Equal("ItemTest", singleVenda.Itens);
        }

        [Fact]
        public void DeveObterAVendaDeId2EretornarTrue()
        {
            var factory = new ConnectionFactory();
            var context = factory.CreateContextForSQLite();
            _vendaController = new VendaController(context);

            var vendedor = new Vendedor("testSqlite", "000.000.000-00", "sqlite@email.com", "0000-0000");
            _vendaController.CadastrarVendedor(vendedor);

            var venda = new Venda("ItemTest", 0, 1);
            _vendaController.RegistrarVenda(venda);

            StatusDaVenda status = (StatusDaVenda)Enum.Parse(typeof(StatusDaVenda), "1");

            var venda2 = new Venda("ItemTest2", status, 1);
            _vendaController.RegistrarVenda(venda2);
            context.SaveChanges();

            var resultado = _vendaController.ObterVendaPorId(2);
            var vendaComVendedor = (resultado as OkObjectResult).Value as IEnumerable<Venda>;

            Assert.NotEmpty(vendaComVendedor);
        }

        [Fact]
        public void RegistraUmaVendaComStatus1MasDeveRetornarStatusZero()
        {
            var factory = new ConnectionFactory();
            var context = factory.CreateContextForSQLite();
            _vendaController = new VendaController(context);

            var vendedor = new Vendedor("testSqlite", "000.000.000-00", "sqlite@email.com", "0000-0000");
            _vendaController.CadastrarVendedor(vendedor);

            StatusDaVenda status = (StatusDaVenda)Enum.Parse(typeof(StatusDaVenda), "1");

            var venda2 = new Venda("ItemTest", status, 1);

            _vendaController.RegistrarVenda(venda2);
            context.SaveChanges();

            var resultado = _vendaController.ObterVendaPorId(1);
            var vendaComVendedor = (resultado as OkObjectResult).Value as IEnumerable<Venda>;

            var statusDaVenda = vendaComVendedor.ElementAtOrDefault(0).Status;
            StatusDaVenda statusZero = (StatusDaVenda)Enum.Parse(typeof(StatusDaVenda), "0");

            Assert.Equal(statusZero, statusDaVenda);
        }

        [Fact]
        public void DeveTentarAtualizarVendaParaStatus3MasRetornarTrueParaStatusZero()
        {
            var factory = new ConnectionFactory();
            var context = factory.CreateContextForSQLite();
            _vendaController = new VendaController(context);

            var vendedor = new Vendedor("testSqlite", "000.000.000-00", "sqlite@email.com", "0000-0000");
            _vendaController.CadastrarVendedor(vendedor);

            var venda = new Venda("ItemTest", 0, 1);
            _vendaController.RegistrarVenda(venda);
            context.SaveChanges();

            StatusDaVenda status3 = (StatusDaVenda)Enum.Parse(typeof(StatusDaVenda), "3");
            StatusDaVenda statusZero = (StatusDaVenda)Enum.Parse(typeof(StatusDaVenda), "0");

            var patchVenda = new JsonPatchDocument<Venda>().Replace(x => x.Status, status3);
            _vendaController.AtualizarStatus(1, patchVenda);
            context.SaveChanges();

            var vendaDatabase = context.Vendas.FirstOrDefault();

            Assert.Equal(statusZero, vendaDatabase.Status);
        }

        [Fact]
        public void DeveAtualizarVendaDeStatusZeroParaStatusUm()
        {
            var factory = new ConnectionFactory();
            var context = factory.CreateContextForSQLite();
            _vendaController = new VendaController(context);

            var vendedor = new Vendedor("testSqlite", "000.000.000-00", "sqlite@email.com", "0000-0000");
            _vendaController.CadastrarVendedor(vendedor);

            var venda = new Venda("ItemTest", 0, 1);
            _vendaController.RegistrarVenda(venda);
            context.SaveChanges();

            StatusDaVenda status1 = (StatusDaVenda)Enum.Parse(typeof(StatusDaVenda), "1");

            var patchVenda = new JsonPatchDocument<Venda>().Replace(x => x.Status, status1);
            _vendaController.AtualizarStatus(1, patchVenda);
            context.SaveChanges();

            var vendaDatabase = context.Vendas.FirstOrDefault();

            Assert.Equal(status1, vendaDatabase.Status);
        }

        [Fact]
        public void DeveTentarAtualizarItensDeVendaMasRetornarTrueParaSeuValorOriginal()
        {
            var factory = new ConnectionFactory();
            var context = factory.CreateContextForSQLite();
            _vendaController = new VendaController(context);

            var vendedor1 = new Vendedor("vendedor1", "000.000.000-00", "sqlite@email.com", "0000-0000");
            _vendaController.CadastrarVendedor(vendedor1);

            var venda = new Venda("Item1", 0, 1);
            _vendaController.RegistrarVenda(venda);
            context.SaveChanges();

            var patchVenda = new JsonPatchDocument<Venda>().Replace(x => x.Itens, "Item2");
            _vendaController.AtualizarStatus(1, patchVenda);
            context.SaveChanges();

            var vendaDatabase = context.Vendas.FirstOrDefault();

            Assert.Equal("Item1", vendaDatabase.Itens);
        }

        [Fact]
        public void DeveTentarAtualizarVendedorIdDeVendaMasRetornarTrueParaSeuValorOriginal()
        {
            var factory = new ConnectionFactory();
            var context = factory.CreateContextForSQLite();
            _vendaController = new VendaController(context);

            var vendedor1 = new Vendedor("vendedor1", "000.000.000-00", "sqlite@email.com", "0000-0000");
            _vendaController.CadastrarVendedor(vendedor1);

            var vendedor2 = new Vendedor("vendedor2", "000.000.000-00", "sqlite@email.com", "0000-0000");
            _vendaController.CadastrarVendedor(vendedor2);

            var venda = new Venda("Item1", 0, 1);
            _vendaController.RegistrarVenda(venda);
            context.SaveChanges();

            var patchVenda = new JsonPatchDocument<Venda>().Replace(x => x.VendedorId, 2);
            _vendaController.AtualizarStatus(1, patchVenda);
            context.SaveChanges();

            var vendaDatabase = context.Vendas.FirstOrDefault();

            Assert.Equal(1, vendaDatabase.VendedorId);
        }
    }
}