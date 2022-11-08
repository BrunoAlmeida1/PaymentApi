using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DesafioPaymentApi.Models
{
    public class Venda
    {
        private String _item;
        private DateTime _dataFormatada;
        public Venda() { }
        public Venda(string item, StatusDaVenda status, int vendedorId)
        {
            _item = item;
            Status = status;
            VendedorId = vendedorId;
        }
        public int VendaId { get; set; }
        public string Itens
        {
            get => _item;
            set
            {
                if (!ContemLetras(value))
                    _item = "";
                else
                    _item = value;
            }
        }
        public StatusDaVenda Status { get; set; }
        public DateTime DataDaVenda
        {
            get => _dataFormatada;
            set
            {
                DateTime dataAtual = DateTime.Now;
                String dataString = dataAtual.ToString("dd/MM/yyyy hh:mm:ss");
                _dataFormatada = DateTime.Parse(dataString);
            }
        }
        public int VendedorId { get; set; }
        public Vendedor Vendedor { get; set; }

        private bool ContemLetras(string itens)
        {
            if (itens.Where(c => char.IsLetter(c)).Count() > 0)
                return true;
            else
                return false;
        }
    }
}