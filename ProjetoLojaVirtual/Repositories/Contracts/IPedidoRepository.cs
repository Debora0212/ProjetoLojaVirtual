﻿using ProjetoLojaVirtual.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace ProjetoLojaVirtual.Repositories.Contracts
{
    public interface IPedidoRepository
    {
        //CRUD
        void Cadastrar(Pedido pedido);
        void Atualizar(Pedido pedido);
        Pedido ObterPedido(int Id);
        IPagedList<Pedido> ObterTodosPedidoCliente(int? pagina, int clienteId);
    }
}