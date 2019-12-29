﻿using AutoMapper;
using Coravel.Invocable;
using Newtonsoft.Json;
using PagarMe;
using ProjetoLojaVirtual.Libraries.Gerenciador.Pagamento.PagarMe;
using ProjetoLojaVirtual.Models;
using ProjetoLojaVirtual.Models.Constants;
using ProjetoLojaVirtual.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoLojaVirtual.Libraries.Gerenciador.Schedule.Invocable
{
    public class PedidoPagamentoSituacaoJob : IInvocable
    {
        private GerenciarPagarMe _gerenciarPagarMe;
        private IPedidoRepository _pedidoRepository;
        private IPedidoSituacaoRepository _pedidoSituacaoRepository;
        private IMapper _mapper;

        public PedidoPagamentoSituacaoJob(GerenciarPagarMe gerenciarPagarMe, IPedidoRepository pedidoRepository, IPedidoSituacaoRepository pedidoSituacaoRepository, IMapper mapper)
        {
            _gerenciarPagarMe = gerenciarPagarMe;
            _pedidoRepository = pedidoRepository;
            _mapper = mapper;
            _pedidoSituacaoRepository = pedidoSituacaoRepository;
        }

        public Task Invoke()
        {
            var pedidosRealizados = _pedidoRepository.ObterTodosPedidosRealizados();
            foreach (var pedido in pedidosRealizados)
            {
                string situacao = null;
                var transaction = _gerenciarPagarMe.ObterTransacao(pedido.TransactionId);

                //TODO - Limitar Vencimento Boleto para 2 Dias úteis.

                //TODO - Após 7 dias o boleto não foi pago, rejeitar a transação.

                //TODO - Colocar 7 dias no arquivo de configuração....
                if (transaction.Status == TransactionStatus.WaitingPayment && transaction.PaymentMethod == PaymentMethod.Boleto && DateTime.Now > pedido.DataRegistro.AddDays(5))
                {
                    situacao = PedidoSituacaoConstant.PAGAMENTO_NAO_REALIZADO;
                }

                if (transaction.Status == TransactionStatus.Refused)
                {
                    situacao = PedidoSituacaoConstant.PAGAMENTO_REJEITADO;
                    //TODO - Retornar para o estoque os produtos desse carrinho.
                }

                if (transaction.Status == TransactionStatus.Authorized || transaction.Status == TransactionStatus.Paid)
                {
                    situacao = PedidoSituacaoConstant.PAGAMENTO_APROVADO;
                }

                if (situacao != null)
                {
                    TransacaoPagarMe transacaoPagarMe = _mapper.Map<Transaction, TransacaoPagarMe>(transaction);
                    transacaoPagarMe.Customer.Gender = (pedido.Cliente.Sexo == "M") ? Gender.Male : Gender.Female;

                    PedidoSituacao pedidoSituacao = new PedidoSituacao();
                    pedidoSituacao.PedidoId = pedido.Id;
                    pedidoSituacao.Situacao = situacao;
                    pedidoSituacao.Data = transaction.DateUpdated.Value;
                    pedidoSituacao.Dados = JsonConvert.SerializeObject(transacaoPagarMe);

                    _pedidoSituacaoRepository.Cadastrar(pedidoSituacao);

                    pedido.Situacao = situacao;
                    _pedidoRepository.Atualizar(pedido);
                }
            }

            Debug.WriteLine("--PedidoPagamentoSituacaoJob - Executado!--");

            return Task.CompletedTask;
        }
    }
}
