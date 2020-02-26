using MediatR;
using NerdStore.Core.Bus;
using NerdStore.Core.Messages;
using NerdStore.Core.Messages.ComunMessages.Notifications;
using NerdStore.Vendas.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NerdStore.Vendas.Application.Commands
{
    public class PedidoCommandHandler : 
        IRequestHandler<AdicionarItemPedidoCommand, bool>,
        IRequestHandler<RemoverItemPedidoCommand, bool>,
        IRequestHandler<AtualizarItemPedidoCommand, bool>,
        IRequestHandler<AplicarVoucherPedidoCommand, bool>

    {
        private readonly IPedidoRepository pedidoRepository;
        private readonly IMediatrHandler mediatrHandler;

        public PedidoCommandHandler(IPedidoRepository pedidoRepository, IMediatrHandler mediatrHandler)
        {
            this.pedidoRepository = pedidoRepository;
            this.mediatrHandler = mediatrHandler;
        }

        public async Task<bool> Handle(AdicionarItemPedidoCommand message, CancellationToken cancellationToken)
        {
            if (!ValidarComando(message)) return false;

            var pedido = await pedidoRepository.ObterPedidoRascunhoPorClienteId(message.ClienteId);
            var pedidoItem = new PedidoItem(message.ProdutoId, message.Nome, message.Quantidade, message.ValorUnitario);

            if(pedido is null)
            {
                pedido = Pedido.PedidoFactory.NovoPedidoRascunho(message.ClienteId);
                pedido.AdicionarItem(pedidoItem);

                pedidoRepository.Adicionar(pedido);
            }
            else
            {
                var pedidoItemExistente = pedido.PedidoItemExistente(pedidoItem);
                pedido.AdicionarItem(pedidoItem);

                if (pedidoItemExistente)
                {
                    pedidoRepository.AtualizarItem(pedido.PedidoItems.FirstOrDefault(p => p.ProdutoId == pedidoItem.ProdutoId));
                }
                else
                {
                    pedidoRepository.AdicionarItem(pedidoItem);
                }

                //pedido.AdicionarEvento(new PedidoAtualizadoEvent(pedido.ClienteId, pedido.Id, pedido.ValorTotal));
            }

            return await pedidoRepository.UnitOfWork.Commit();
        }

        public Task<bool> Handle(AtualizarItemPedidoCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Handle(AplicarVoucherPedidoCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task<bool> IRequestHandler<RemoverItemPedidoCommand, bool>.Handle(RemoverItemPedidoCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        private bool ValidarComando(Command message)
        {
            if (message.EhValido()) return true;

            foreach (var error in message.ValidationResult.Errors)
            {
                mediatrHandler.PublicarNotificacao(
                    new DomainNotification(message.MessageType, error.ErrorMessage));
            }

            return false;
        }
    }
}
