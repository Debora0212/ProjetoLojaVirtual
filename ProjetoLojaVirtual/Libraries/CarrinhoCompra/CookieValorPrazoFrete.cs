using Newtonsoft.Json;
using ProjetoLojaVirtual.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoLojaVirtual.Libraries.CarrinhoCompra
{
    public class CookieValorPrazoFrete
    {
        private string Key = "Carrinho.ValorPrazoFrete";
        private Cookie.Cookie _cookie;

        public CookieValorPrazoFrete(Cookie.Cookie cookie)
        {
            _cookie = cookie;
        }
        public void Cadastrar(List<ValorPrazoFrete> lista)
        {
            var jsonString = JsonConvert.SerializeObject(lista);
            _cookie.Cadastrar(Key, jsonString);
        }
        public List<ValorPrazoFrete> Consultar()
        {
            if (_cookie.Existe(Key))
            {
                string valor = _cookie.Consultar(Key);
                return JsonConvert.DeserializeObject<List<ValorPrazoFrete>>(valor);
            }
            else
            {
                return null;
            }
        }
        public void Remover()
        {
            _cookie.Remover(Key);
        }
    }
}
