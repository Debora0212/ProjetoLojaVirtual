using ProjetoLojaVirtual.Database;
using ProjetoLojaVirtual.Models;
using ProjetoLojaVirtual.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoLojaVirtual.Repositories
{
    public class EnderecoEntregaRepository : IEnderecoEntregaRepository
    {
        private LojaVirtualContext _banco;

        public EnderecoEntregaRepository(LojaVirtualContext banco)
        {
            _banco = banco;
        }

        public void Atualizar(EnderecoEntrega endereco)
        {
            _banco.Update(endereco);
            _banco.SaveChanges();
        }

        public void Cadastrar(EnderecoEntrega endereco)
        {
            _banco.Add(endereco);
            _banco.SaveChanges();
        }

        public void Excluir(int Id)
        {
            EnderecoEntrega endereco = ObterEnderecoEntrega(Id);
            _banco.Remove(endereco);
            _banco.SaveChanges();
        }

        public EnderecoEntrega ObterEnderecoEntrega(int Id)
        {
            return _banco.EnderecosEntrega.Find(Id);
        }

        public IList<EnderecoEntrega> ObterTodosEnderecoEntregaCliente(int clienteId)
        {
            return _banco.EnderecosEntrega.Where(a => a.ClienteId == clienteId).ToList();
        }
    }
}
