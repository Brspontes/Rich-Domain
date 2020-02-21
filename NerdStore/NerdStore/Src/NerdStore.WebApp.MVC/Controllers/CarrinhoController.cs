using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NerdStore.Catalogo.Application.Services;
using NerdStore.Core.Bus;
using NerdStore.Core.Messages.ComunMessages.Notifications;
using NerdStore.Vendas.Application.Commands;

namespace NerdStore.WebApp.MVC.Controllers
{
    public class CarrinhoController : ControllerBase
    {
        private readonly IProdutoAppService produtoAppService;
        private readonly IMediatrHandler mediatr;

        public CarrinhoController(IProdutoAppService produtoAppService, IMediatrHandler mediatr,
            INotificationHandler<DomainNotification> notificationHandler) : base (notificationHandler, mediatr)
        {
            this.produtoAppService = produtoAppService;
            this.mediatr = mediatr;
        }

        public IActionResult Index()    
        {
            return View();
        }

        [HttpPost]
        [Route("meu-carrinho")]
        public async Task<IActionResult> AdiconarItem(Guid id, int quantidade)
        {
            var produto = await produtoAppService.ObterPorId(id);
            if (produto is null) return BadRequest();

            if(produto.QuantidadeEstoque < quantidade)
            {
                TempData["Erro"] = "Produto com estoque insuficiente";
                return RedirectToAction("ProdutoDetalhe", "Vitrine", new { id });
            }

            var command = new AdicionarItemPedidoCommand(ClienteId, produto.Id, produto.Nome, quantidade, produto.Valor);

            await mediatr.EnviarComando(command);

            if (OperacaoValida())
            {
                RedirectToAction("Index");
            }

            TempData["Erros"] = ObterMensagensErro();
            return RedirectToAction("ProdutoDetalhe", "Vitrine", new { id });
        }
    }
}