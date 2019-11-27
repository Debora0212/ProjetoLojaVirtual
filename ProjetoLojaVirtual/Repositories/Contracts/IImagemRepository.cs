using ProjetoLojaVirtual.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoLojaVirtual.Repositories.Contracts
{
    public interface IImagemRepository
    {
        void CadastrarImagens(List<Imagem> ListaImagens, int ProdutoId);
        void Cadastrar(Imagem imagem);
        void Excluir(int Id);
        void ExcluirImagemDoProduto(int ProdutoId);

    }
}
