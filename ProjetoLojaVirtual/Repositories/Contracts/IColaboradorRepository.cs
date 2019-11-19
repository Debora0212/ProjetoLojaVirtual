using ProjetoLojaVirtual.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace ProjetoLojaVirtual.Repositories.Contracts
{
    public interface IColaboradorRepository
    {
        Colaborador Login(string Email, string Senha);

        void Cadastrar(Colaborador colaborador);
        void Atualizar(Colaborador colaborador);
        void AtualizarSenha(Colaborador colaborador);
        void Excluir(int Id);

        Colaborador ObterColaborador(int Id);
        List<Colaborador> ObterColaboradorPorEmail(string email);
        IPagedList<Colaborador> ObterTodosColaboradores(int? pagina);

    }
}
