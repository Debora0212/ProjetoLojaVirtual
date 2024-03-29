﻿using Coravel.Invocable;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ProjetoLojaVirtual.Models;
using ProjetoLojaVirtual.Models.Constants;
using ProjetoLojaVirtual.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoLojaVirtual.Libraries.Gerenciador.Schedule.Invocable
{
    public class PedidoFinalizadoJob : IInvocable
    {
        private IPedidoRepository _pedidoRepository;
        private IPedidoSituacaoRepository _pedidoSituacaoRepository;
        private IConfiguration _configuration;
        private ILogger<PedidoFinalizadoJob> _logger;

        public PedidoFinalizadoJob(ILogger<PedidoFinalizadoJob> logger, IPedidoRepository pedidoRepository, IPedidoSituacaoRepository pedidoSituacaoRepository, IConfiguration configuration)
        {
            _pedidoRepository = pedidoRepository;
            _pedidoSituacaoRepository = pedidoSituacaoRepository;
            _configuration = configuration;
            _logger = logger;
        }
        public Task Invoke()
        {
            _logger.LogInformation("> PedidoFinalziadoJob: Iniciando");

            var pedidos = _pedidoRepository.ObterTodosPedidosPorSituacao(PedidoSituacaoConstant.ENTREGUE);
            foreach (var pedido in pedidos)
            {
                var pedidoSituacaoDB = pedido.PedidoSituacoes.FirstOrDefault(a => a.Situacao == PedidoSituacaoConstant.ENTREGUE);

                if (pedidoSituacaoDB != null)
                {
                    int tolerancia = _configuration.GetValue<int>("Finalizado:Days");

                    if (DateTime.Now >= pedidoSituacaoDB.Data.AddDays(tolerancia))
                    {
                        PedidoSituacao pedidoSituacao = new PedidoSituacao();
                        pedidoSituacao.PedidoId = pedido.Id;
                        pedidoSituacao.Situacao = PedidoSituacaoConstant.FINALIZADO;
                        pedidoSituacao.Data = DateTime.Now;
                        pedidoSituacao.Dados = string.Empty;

                        _pedidoSituacaoRepository.Cadastrar(pedidoSituacao);

                        pedido.Situacao = PedidoSituacaoConstant.FINALIZADO;
                        _pedidoRepository.Atualizar(pedido);
                    }
                }
            }
            _logger.LogInformation("> PedidoFinalizadoJob: Finalizado");

            return Task.CompletedTask;
        }
    }
}
