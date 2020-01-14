using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProjetoLojaVirtual.Libraries.Filtro;
using ProjetoLojaVirtual.Libraries.Gerenciador.Pagamento.PagarMe;
using ProjetoLojaVirtual.Libraries.Json.Resolver;
using ProjetoLojaVirtual.Models;
using ProjetoLojaVirtual.Models.Constants;
using ProjetoLojaVirtual.Models.ProdutoAgregador;
using ProjetoLojaVirtual.Models.ViewModels.Pedido;
using ProjetoLojaVirtual.Repositories.Contracts;

namespace ProjetoLojaVirtual.Areas.Colaborador.Controllers
{
    [Area("Colaborador")]
    [ColaboradorAutorizacao]
    public class PedidoController : Controller
    {
        private IPedidoRepository _pedidoRepository;
        private IPedidoSituacaoRepository _pedidoSituacaoRepository;
        private GerenciarPagarMe _gerenciarPagarMe;
        private IProdutoRepository _produtoRepository;

        public PedidoController(IPedidoRepository pedidoRepository, IPedidoSituacaoRepository pedidoSituacaoRepository, GerenciarPagarMe gerenciarPagarMe, IProdutoRepository produtoRepository)
        {
            _pedidoRepository = pedidoRepository;
            _pedidoSituacaoRepository = pedidoSituacaoRepository;
            _gerenciarPagarMe = gerenciarPagarMe;
            _produtoRepository = produtoRepository;
        }

        public IActionResult Index(int? pagina, string codigoPedido, string cpf)
        {
            var pedidos = _pedidoRepository.ObterTodosPedido(pagina, codigoPedido, cpf);

            return View(pedidos);
        }

        public IActionResult Visualizar(int id)
        {
            Pedido pedido = _pedidoRepository.ObterPedido(id);

            var viewModel = new VisualizarViewModel() { Pedido = pedido };

            return View(viewModel);
        }

        public IActionResult NFE([FromForm]VisualizarViewModel viewModel, int id)
        {
            ValidarFormulario(nameof(viewModel.NFE));

            Pedido pedido = _pedidoRepository.ObterPedido(id);

            if (ModelState.IsValid)
            {
                string url = viewModel.NFE.NFE_URL;

                SalvarPedidoSitucao(id, url, PedidoSituacaoConstant.NF_EMITIDA);
                SalvarPedido(pedido, PedidoSituacaoConstant.NF_EMITIDA, url);

                return RedirectToAction(nameof(Visualizar), new { id = id });
            }
            ViewBag.MODAL_NFE = true;
            viewModel.Pedido = pedido;
            return View(nameof(Visualizar), viewModel);
        }
      
        public IActionResult RegistrarRastreamento([FromForm]VisualizarViewModel viewModel, int id)
        {
            ValidarFormulario(nameof(viewModel.CodigoRastreamento));

            Pedido pedido = _pedidoRepository.ObterPedido(id);

            if (ModelState.IsValid)
            {
                string codRastreamento = viewModel.CodigoRastreamento.Codigo;

                SalvarPedidoSitucao(id, codRastreamento, PedidoSituacaoConstant.EM_TRANSPORTE);
                SalvarPedido(pedido, PedidoSituacaoConstant.EM_TRANSPORTE, null, codRastreamento);

                return RedirectToAction(nameof(Visualizar), new { id = id });
            }
            ViewBag.MODAL_RASTREAMENTO = true;
            viewModel.Pedido = pedido;
            return View(nameof(Visualizar), viewModel);
        }

        public IActionResult RegistrarCancelamentoPedidoCartaoCredito([FromForm]VisualizarViewModel viewModel, int id)
        {
            ValidarFormulario(nameof(viewModel.CartaoCredito));

            Pedido pedido = _pedidoRepository.ObterPedido(id);

            if (ModelState.IsValid)
            {
                viewModel.CartaoCredito.FormPagamento = MetodoPagamentoConstant.CartaoCredito;
             
                _gerenciarPagarMe.EstornoCartaoCredito(pedido.TransactionId);             

                SalvarPedidoSitucao(id, viewModel.CartaoCredito, PedidoSituacaoConstant.ESTORNO);
                SalvarPedido(pedido, PedidoSituacaoConstant.ESTORNO);

                _produtoRepository.DevolverProdutoAoEstoque(pedido);

                return RedirectToAction(nameof(Visualizar), new { id = id });
            }
            ViewBag.MODAL_CARTAOCREDITO = true;
            viewModel.Pedido = pedido;
            return View(nameof(Visualizar), viewModel);
        }

        public IActionResult RegistrarCancelamentoPedidoBoletoBancario([FromForm]VisualizarViewModel viewModel, int id)
        {
            ValidarFormulario(nameof(viewModel.BoletoBancario));

            Pedido pedido = _pedidoRepository.ObterPedido(id);

            if (ModelState.IsValid)
            {
                viewModel.BoletoBancario.FormPagamento = MetodoPagamentoConstant.Boleto;
         
                _gerenciarPagarMe.EstornoBoletoBancario(pedido.TransactionId, viewModel.BoletoBancario);

                SalvarPedidoSitucao(id, viewModel.BoletoBancario, PedidoSituacaoConstant.ESTORNO);
                SalvarPedido(pedido, PedidoSituacaoConstant.ESTORNO);
               
                _produtoRepository.DevolverProdutoAoEstoque(pedido);

                return RedirectToAction(nameof(Visualizar), new { id = id });
            }
            ViewBag.MODAL_BOLETOBANCARIO = true;
            viewModel.Pedido = pedido;
            return View(nameof(Visualizar), viewModel);
        }

        public IActionResult RegistrarDevolucaoPedido([FromForm]VisualizarViewModel viewModel, int id)
        {
            ValidarFormulario(nameof(viewModel.Devolucao));

            Pedido pedido = _pedidoRepository.ObterPedido(id);

            if (ModelState.IsValid)
            { 
                SalvarPedidoSitucao(id, viewModel.Devolucao, PedidoSituacaoConstant.DEVOLVER);
                SalvarPedido(pedido, PedidoSituacaoConstant.DEVOLVER);

                return RedirectToAction(nameof(Visualizar), new { id = id });
            }
            ViewBag.MODAL_DEVOLVER = true;
            viewModel.Pedido = pedido;
            return View(nameof(Visualizar), viewModel);
        }

        public IActionResult RegistrarDevolucaoPedidoRejeicao([FromForm]VisualizarViewModel viewModel, int id)
        {
            ValidarFormulario(nameof(viewModel.DevolucaoMotivoRejeicao));

            Pedido pedido = _pedidoRepository.ObterPedido(id);

            if (ModelState.IsValid)
            {
                SalvarPedidoSitucao(id, viewModel.DevolucaoMotivoRejeicao, PedidoSituacaoConstant.DEVOLUCAO_REJEITADA);
                SalvarPedido(pedido, PedidoSituacaoConstant.DEVOLUCAO_REJEITADA);

                return RedirectToAction(nameof(Visualizar), new { id = id });
            }
            ViewBag.MODAL_DEVOLVER_REJEITAR = true;
            viewModel.Pedido = pedido;
            return View(nameof(Visualizar), viewModel);
        }

        public IActionResult RegistrarDevolucaoPedidoAprovadoCartaoCredito(int id)
        {
            Pedido pedido = _pedidoRepository.ObterPedido(id);
            if (pedido.Situacao == PedidoSituacaoConstant.DEVOLVER_ENTREGUE)
            {
                _gerenciarPagarMe.EstornoCartaoCredito(pedido.TransactionId);
                _produtoRepository.DevolverProdutoAoEstoque(pedido);

                SalvarPedidoSitucao(id, "", PedidoSituacaoConstant.DEVOLUCAO_ACEITA);
                SalvarPedidoSitucao(id, "", PedidoSituacaoConstant.DEVOLVER_ESTORNO);
                SalvarPedido(pedido, PedidoSituacaoConstant.DEVOLVER_ESTORNO);

                return RedirectToAction(nameof(Visualizar), new { id = id });
            }
            VisualizarViewModel viewModel = new VisualizarViewModel();
            viewModel.Pedido = pedido;
            return View(nameof(Visualizar), viewModel);
        }

        public IActionResult RegistrarDevolucaoPedidoAprovadoBoletoBancario([FromForm]VisualizarViewModel viewModel, int id)
        {
            ValidarFormulario(nameof(viewModel.BoletoBancario), "BoletoBancario.Motivo");

            Pedido pedido = _pedidoRepository.ObterPedido(id);
            if (ModelState.IsValid)
            {
                viewModel.BoletoBancario.FormPagamento = MetodoPagamentoConstant.Boleto;
                _gerenciarPagarMe.EstornoBoletoBancario(pedido.TransactionId, viewModel.BoletoBancario);
                _produtoRepository.DevolverProdutoAoEstoque(pedido);

                SalvarPedidoSitucao(id, "", PedidoSituacaoConstant.DEVOLUCAO_ACEITA);
                SalvarPedidoSitucao(id, viewModel.BoletoBancario, PedidoSituacaoConstant.DEVOLVER_ESTORNO);
                SalvarPedido(pedido, PedidoSituacaoConstant.DEVOLVER_ESTORNO);
               
                return RedirectToAction(nameof(Visualizar), new { id = id });
            }
            ViewBag.MODAL_DEVOLVERBOLETOBANCARIO = true;
            viewModel.Pedido = pedido;
            return View(nameof(Visualizar), viewModel);
        }

        private void ValidarFormulario(string formularioParaValidar, params string [] removerFormularios)
        {
            //ModelState -> Validações.

            //NFE == NFE = Validar.
            //Pedido == NFE = Não é para validar.

            var propriedades = new VisualizarViewModel().GetType().GetProperties();

            foreach (var propriedade in propriedades)
            {
                //Pedido != NFE = true -> Sai da validação
                //NFE != NFE = false -> Continua na validação
                if (propriedade.Name != formularioParaValidar)
                {
                    ModelState.Remove(propriedade.Name);
                }
            }

            foreach (string removerFormulario in removerFormularios)
            {
                ModelState.Remove(removerFormulario);
            }
        }

        private void SalvarPedidoSitucao(int pedidoId, object dados, string situacao)
        {
            var pedidoSituacao = new PedidoSituacao();
            pedidoSituacao.Data = DateTime.Now;
            pedidoSituacao.Dados = JsonConvert.SerializeObject(dados);
            pedidoSituacao.PedidoId = pedidoId;
            pedidoSituacao.Situacao = situacao;
            _pedidoSituacaoRepository.Cadastrar(pedidoSituacao);
        }

        private void SalvarPedido(Pedido pedido, string situacao, string nfe = null, string codRastreamento = null)
        {
            pedido.Situacao = situacao;
            if (nfe != null)
            {
                pedido.NFE = nfe;
            }
            if (codRastreamento != null)
            {
                pedido.FreteCodRastreamento = codRastreamento;
            }
            _pedidoRepository.Atualizar(pedido);
        }
    }
}