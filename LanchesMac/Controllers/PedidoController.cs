using LanchesMac.Models;
using LanchesMac.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LanchesMac.Controllers;

public class PedidoController : Controller
{
    private readonly IPedidoRepository _pedidoRepository;
    private readonly CarrinhoCompra _carrinhoCompra;

    public PedidoController(IPedidoRepository pedidoRepository, CarrinhoCompra carrinhoCompra)
    {
        _pedidoRepository = pedidoRepository;
        _carrinhoCompra = carrinhoCompra;
    }
    [Authorize]
    [HttpGet]
    public IActionResult Checkout()
    {
        return View();
	}
	[Authorize]
	[HttpPost]
    public IActionResult Checkout(Pedido pedido)
    {
        int totalItensPedido = 0;
        decimal precoTotalPedido = 0.0m;

        //obter os itens do carrinho de compra do cliente
        List<CarrinhoCompraItem> items = _carrinhoCompra.GetCarrinhoCompraItens();
        _carrinhoCompra.CarrinhoCompraItems = items;

        //verificar se existem itens de pedido
        if (_carrinhoCompra.CarrinhoCompraItems.Count == 0)
        {
            ModelState.AddModelError("", "Seu carrinho esta vazio, que tal incluir um lanche...");
        }

        //calcular o total de itens e o total do pedido
        foreach (var item in items)
        {
            totalItensPedido += item.Quantidade;
            precoTotalPedido += (item.Lanche.Preco * item.Quantidade);
        }

        //atribuir os valores obtidos ao pedido
        pedido.TotalItensPedido = totalItensPedido;
        pedido.PedidoTotal = precoTotalPedido;

        //valida os dados do pedido
        if (ModelState.IsValid)
        {
            //criar o pedido e os detalhes
            _pedidoRepository.CriarPedido(pedido);

            //definir mensagens ao cliente
            ViewBag.CheckoutCompletoMensagem = "Obrigado pelo seu pedido :)";
            ViewBag.Totalpedido = _carrinhoCompra.GetCarrinhoCompraTotal();

            //limpa o carringo do cliente
            _carrinhoCompra.LimparCarrinho();

            //exibe a view com dados do cliente e do pedido
            return View("~/Views/Pedido/CheckoutCompleto.cshtml", pedido);
        }
        return View(pedido);
    }
}
