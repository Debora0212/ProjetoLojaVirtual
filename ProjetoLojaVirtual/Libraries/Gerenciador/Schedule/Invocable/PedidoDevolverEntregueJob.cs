﻿using Coravel.Invocable;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ProjetoLojaVirtual.Models;
using ProjetoLojaVirtual.Models.Constants;
using ProjetoLojaVirtual.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoLojaVirtual.Libraries.Gerenciador.Schedule.Invocable
{
    public class PedidoDevolverEntregueJob : IInvocable
    {
        private IPedidoRepository _pedidoRepository;
        private IPedidoSituacaoRepository _pedidoSituacaoRepository;
        private ILogger<PedidoDevolverEntregueJob> _logger;

        public PedidoDevolverEntregueJob(ILogger<PedidoDevolverEntregueJob> logger, IPedidoRepository pedidoRepository, IPedidoSituacaoRepository pedidoSituacaoRepository)
        {
            _pedidoRepository = pedidoRepository;
            _pedidoSituacaoRepository = pedidoSituacaoRepository;
            _logger = logger;
        }

        public Task Invoke()
        {
            _logger.LogInformation("> PedidoDevolverEntregueJob: Iniciando");
        
            var pedidos = _pedidoRepository.ObterTodosPedidosPorSituacao(PedidoSituacaoConstant.DEVOLVER);

            foreach (var pedido in pedidos)
            {
                var result = new Correios.NET.Services().GetPackageTracking(pedido.FreteCodRastreamento);

                if (result.IsDelivered)
                {
                    PedidoSituacao pedidoSituacao = new PedidoSituacao();
                    pedidoSituacao.PedidoId = pedido.Id;
                    pedidoSituacao.Situacao = PedidoSituacaoConstant.DEVOLVER_ENTREGUE;
                    pedidoSituacao.Data = result.DeliveryDate.Value;
                    pedidoSituacao.Dados = JsonConvert.SerializeObject(result);

                    _pedidoSituacaoRepository.Cadastrar(pedidoSituacao);

                    pedido.Situacao = PedidoSituacaoConstant.DEVOLVER_ENTREGUE;
                    _pedidoRepository.Atualizar(pedido);
                }

            }
            _logger.LogInformation("> PedidoDevolverEntregueJob: Finalizado");

            return Task.CompletedTask;
        }
    }
}
