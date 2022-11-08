using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DesafioPaymentApi.Context;
using DesafioPaymentApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;

namespace DesafioPaymentApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VendaController : ControllerBase
    {
        private readonly VendaContext _context;

        public VendaController(VendaContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult CadastrarVendedor(Vendedor vendedor)
        {
            _context.Add(vendedor);
            _context.SaveChanges();

            return Ok(vendedor);
        }

        [HttpPut]
        public IActionResult RegistrarVenda(Venda venda)
        {
            var statusZero = (StatusDaVenda)Enum.Parse(typeof(StatusDaVenda), "0");

            _context.Add(venda);

            if (venda.Itens == "")
            {
                var mensagem = "O campo itens não pode ser vazio.";
                return Ok(mensagem);
            }
            else
            {
                _context.SaveChanges();
                _context.Vendas.Find(venda.VendaId).Status = statusZero;
                _context.SaveChanges();
            }
            return Ok(venda);
        }

        [HttpGet("{id}")]
        public IActionResult ObterVendaPorId(int id)
        {
            if (_context.Vendas.Find(id) == null)
                return NotFound();

            var vendaComVendedor = _context.Vendas.Include(x => x.Vendedor).Where(x => x.VendaId == id);

            return Ok(vendaComVendedor);
        }

        [HttpPatch("{id}")]
        public IActionResult AtualizarStatus(int id, [FromBody] JsonPatchDocument<Venda> patchVenda)
        {
            if (patchVenda == null)
                return BadRequest();

            var vendaDatabase = _context.Vendas.Find(id);
            if (vendaDatabase == null)
                return NotFound();

            var statusOriginal = vendaDatabase.Status;
            var itemOriginal = vendaDatabase.Itens;
            var vendedorOriginal = vendaDatabase.VendedorId;

            patchVenda.ApplyTo(vendaDatabase, ModelState);
            _context.SaveChanges();

            var statusAtualizado = vendaDatabase.Status;
            var itemAtualizado = vendaDatabase.Itens;
            var vendedorAtualizado = vendaDatabase.VendedorId;

            StatusDaVenda statusZero = (StatusDaVenda)Enum.Parse(typeof(StatusDaVenda), "0");
            StatusDaVenda status1 = (StatusDaVenda)Enum.Parse(typeof(StatusDaVenda), "1");
            StatusDaVenda status2 = (StatusDaVenda)Enum.Parse(typeof(StatusDaVenda), "2");
            StatusDaVenda status3 = (StatusDaVenda)Enum.Parse(typeof(StatusDaVenda), "3");
            StatusDaVenda status4 = (StatusDaVenda)Enum.Parse(typeof(StatusDaVenda), "4");

            //O código abaixo impede que sejam feitas alterações nos campos Itens e Vendedor
            if (itemAtualizado != itemOriginal)
                {
                    vendaDatabase.Itens = itemOriginal;
                    _context.SaveChanges();
                    var mensagem = $"O campo Itens não pode ser alterado";

                    return Ok(new { mensagem });
                }
            if (vendedorAtualizado != vendedorOriginal)
                {
                    vendaDatabase.VendedorId = vendedorOriginal;
                    _context.SaveChanges();
                    var mensagem = $"O campo Vendedor não pode ser alterado";

                    return Ok(new { mensagem });
                }

            //O código abaixo garante que o status será atualizado de acordo com o status atual
            if (statusOriginal.Equals(statusZero))
            {
                if (statusAtualizado != status1 && statusAtualizado != status4)
                {
                    vendaDatabase.Status = statusZero;
                    _context.SaveChanges();
                    var mensagem = $"O status {statusOriginal} só pode ser alterado para {status1} ou {status4}.";

                    return Ok(new { mensagem });
                }
            }
            else if (statusOriginal.Equals(status1))
            {
                if (statusAtualizado != status2 && statusAtualizado != status4)
                {
                    vendaDatabase.Status = status1;
                    _context.SaveChanges();
                    var mensagem = $"O status {statusOriginal} só pode ser alterado para {status2} ou {status4}.";

                    return Ok(new { mensagem });
                }
            }
            else if (statusOriginal.Equals(status2))
            {
                if (statusAtualizado != status3)
                {
                    vendaDatabase.Status = status2;
                    _context.SaveChanges();
                    var mensagem = $"O status {statusOriginal} só pode ser alterado para {status3}.";

                    return Ok(new { mensagem });
                }
            }
            else
            {
                vendaDatabase.Status = statusOriginal;
                _context.SaveChanges();
                var mensagem = $"Este status não pode ser alterado.";

                return Ok(new { mensagem });
            }

            _context.SaveChanges();
            return Ok(vendaDatabase);
        }
    }
}