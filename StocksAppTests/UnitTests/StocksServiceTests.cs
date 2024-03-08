using Entities;
using ServiceContracts;
using ServiceContracts.DTOs;
using Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;
using RepositoryContracts;
using FluentAssertions;
using AutoFixture;

namespace StocksAppTests.UnitTests
{
    public class StocksServiceTests
    {
        private readonly IStocksService _stocksService;

        private readonly Mock<IStocksRepository> _stocksRepositoryMock;
        private readonly IStocksRepository _stocksRepository;

        private readonly IFixture _fixture;

        public StocksServiceTests()
        {
            _fixture = new Fixture();

            _stocksRepositoryMock = new Mock<IStocksRepository>();
            _stocksRepository = _stocksRepositoryMock.Object;

            _stocksService = new StocksService(_stocksRepository, null);
        }

        #region CreateBuyOrder

        [Fact]
        public async Task CreateBuyOrder_NullRequest_ToBeArgumentNullException()
        {
            BuyOrderRequest? buy_order_request = null;

            var buyOrder = _fixture.Build<BuyOrder>().Create();

            _stocksRepositoryMock.Setup(temp => temp.CreateBuyOrder(It.IsAny<BuyOrder>()));

            var action = async () => await _stocksService.CreateBuyOrder(buy_order_request);

            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task CreateBuyOrder_BelowMinimumQuantity_ToBeArgumentException()
        {
            BuyOrderRequest? buy_order_request = _fixture.Build<BuyOrderRequest>()
                .With(temp => temp.Quantity, Convert.ToUInt32(0))
                .Create();

            var buyOrder = buy_order_request.ToBuyOrder();

            _stocksRepositoryMock.Setup(temp => temp.CreateBuyOrder(It.IsAny<BuyOrder>())).ReturnsAsync(buyOrder);

            var action = async () => await _stocksService.CreateBuyOrder(buy_order_request);

            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task CreateBuyOrder_AboveMaximumQuantity_ToBeArgumentException()
        {
            BuyOrderRequest? buy_order_request = _fixture.Build<BuyOrderRequest>()
                .With(temp => temp.Quantity, Convert.ToUInt32(100001))
                .Create();

            var buyOrder = buy_order_request.ToBuyOrder();

            _stocksRepositoryMock.Setup(temp => temp.CreateBuyOrder(It.IsAny<BuyOrder>())).ReturnsAsync(buyOrder);

            var action = async () => await _stocksService.CreateBuyOrder(buy_order_request);

            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task CreateBuyOrder_BelowMinimumPrice_ToBeArgumentException()
        {
            BuyOrderRequest? buy_order_request = _fixture.Build<BuyOrderRequest>()
                .With(temp => temp.Price, Convert.ToUInt32(0))
                .Create();

            var buyOrder = buy_order_request.ToBuyOrder();

            _stocksRepositoryMock.Setup(temp => temp.CreateBuyOrder(It.IsAny<BuyOrder>())).ReturnsAsync(buyOrder);

            var action = async () => await _stocksService.CreateBuyOrder(buy_order_request);

            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task CreateBuyOrder_AboveMaximumPrice_ToBeArgumentException()
        {
            BuyOrderRequest? buy_order_request = _fixture.Build<BuyOrderRequest>()
                .With(temp => temp.Price, Convert.ToUInt32(10001))
                .Create();

            var buyOrder = buy_order_request.ToBuyOrder();

            _stocksRepositoryMock.Setup(temp => temp.CreateBuyOrder(It.IsAny<BuyOrder>())).ReturnsAsync(buyOrder);

            var action = async () => await _stocksService.CreateBuyOrder(buy_order_request);

            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task CreateBuyOrder_NullStockSymbol_ToBeArgumentException()
        {
            BuyOrderRequest? buy_order_request = _fixture.Build<BuyOrderRequest>()
                .With(temp => temp.StockSymbol, null as string)
                .Create();

            var buyOrder = buy_order_request.ToBuyOrder();

            _stocksRepositoryMock.Setup(temp => temp.CreateBuyOrder(It.IsAny<BuyOrder>())).ReturnsAsync(buyOrder);

            var action = async () => await _stocksService.CreateBuyOrder(buy_order_request);

            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task CreateBuyOrder_DateTimeOutOfRange_ToBeArgumentException()
        {
            BuyOrderRequest? buy_order_request = _fixture.Build<BuyOrderRequest>()
                .With(temp => temp.DateAndTimeOfOrder, DateTime.Parse("1999-12-31"))
                .Create();

            var buyOrder = buy_order_request.ToBuyOrder();

            _stocksRepositoryMock.Setup(temp => temp.CreateBuyOrder(It.IsAny<BuyOrder>())).ReturnsAsync(buyOrder);

            var action = async () => await _stocksService.CreateBuyOrder(buy_order_request);

            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task CreateBuyOrder_DetailsValid_ToBeSuccessful()
        {
            BuyOrderRequest? buy_order_request = _fixture.Create<BuyOrderRequest>();

            var buyOrder = buy_order_request.ToBuyOrder();

            _stocksRepositoryMock.Setup(temp => temp.CreateBuyOrder(It.IsAny<BuyOrder>())).ReturnsAsync(buyOrder);

            var buy_order_response_from_create = await _stocksService.CreateBuyOrder(buy_order_request);

            buyOrder.BuyOrderId = buy_order_response_from_create.BuyOrderId;
            BuyOrderResponse buyOrderResponse_expected = buyOrder.ToBuyOrderResponse();

            buy_order_response_from_create.BuyOrderId.Should().NotBe(Guid.Empty);
            buy_order_response_from_create.Should().Be(buyOrderResponse_expected);
        }

        #endregion

        #region CreateSellOrder

        [Fact]
        public async Task CreateSellOrder_NullRequest_ToBeArgumentNullException()
        {
            SellOrderRequest? sell_order_request = null;

            var sellOrder = _fixture.Build<SellOrder>().Create();

            _stocksRepositoryMock.Setup(temp => temp.CreateSellOrder(It.IsAny<SellOrder>()));

            var action = async () => await _stocksService.CreateSellOrder(sell_order_request);

            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task CreateSellOrder_BelowMinimumQuantity_ToBeArgumentException()
        {
            SellOrderRequest? sell_order_request = _fixture.Build<SellOrderRequest>()
                .With(temp => temp.Quantity, Convert.ToUInt32(0))
                .Create();

            var sellOrder = sell_order_request.ToSellOrder();

            _stocksRepositoryMock.Setup(temp => temp.CreateSellOrder(It.IsAny<SellOrder>())).ReturnsAsync(sellOrder);

            var action = async () => await _stocksService.CreateSellOrder(sell_order_request);

            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task CreateSellOrder_AboveMaximumQuantity_ToBeArgumentException()
        {
            SellOrderRequest? sell_order_request = _fixture.Build<SellOrderRequest>()
                .With(temp => temp.Quantity, Convert.ToUInt32(100001))
                .Create();

            var sellOrder = sell_order_request.ToSellOrder();

            _stocksRepositoryMock.Setup(temp => temp.CreateSellOrder(It.IsAny<SellOrder>())).ReturnsAsync(sellOrder);

            var action = async () => await _stocksService.CreateSellOrder(sell_order_request);

            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task CreateSellOrder_BelowMinimumPrice_ToBeArgumentException()
        {
            SellOrderRequest? sell_order_request = _fixture.Build<SellOrderRequest>()
                 .With(temp => temp.Price, Convert.ToUInt32(0))
                 .Create();

            var sellOrder = sell_order_request.ToSellOrder();

            _stocksRepositoryMock.Setup(temp => temp.CreateSellOrder(It.IsAny<SellOrder>())).ReturnsAsync(sellOrder);

            var action = async () => await _stocksService.CreateSellOrder(sell_order_request);

            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task CreateSellOrder_AboveMaximumPrice_ToBeArgumentException()
        {
            SellOrderRequest? sell_order_request = _fixture.Build<SellOrderRequest>()
                 .With(temp => temp.Price, Convert.ToUInt32(10001))
                 .Create();

            var sellOrder = sell_order_request.ToSellOrder();

            _stocksRepositoryMock.Setup(temp => temp.CreateSellOrder(It.IsAny<SellOrder>())).ReturnsAsync(sellOrder);

            var action = async () => await _stocksService.CreateSellOrder(sell_order_request);

            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task CreateSellOrder_NullStockSymbol_ToBeArgumentException()
        {
            SellOrderRequest? sell_order_request = _fixture.Build<SellOrderRequest>()
                 .With(temp => temp.StockSymbol, null as string)
                 .Create();

            var sellOrder = sell_order_request.ToSellOrder();

            _stocksRepositoryMock.Setup(temp => temp.CreateSellOrder(It.IsAny<SellOrder>())).ReturnsAsync(sellOrder);

            var action = async () => await _stocksService.CreateSellOrder(sell_order_request);

            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task CreateSellOrder_DateTimeOutOfRange_ToBeArgumentException()
        {
            SellOrderRequest? sell_order_request = _fixture.Build<SellOrderRequest>()
                 .With(temp => temp.DateAndTimeOfOrder, DateTime.Parse("1999-12-31"))
                 .Create();

            var sellOrder = sell_order_request.ToSellOrder();

            _stocksRepositoryMock.Setup(temp => temp.CreateSellOrder(It.IsAny<SellOrder>())).ReturnsAsync(sellOrder);

            var action = async () => await _stocksService.CreateSellOrder(sell_order_request);

            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task CreateSellOrder_DetailsValid_ToBeSuccessful()
        {
            SellOrderRequest? sell_order_request = _fixture.Create<SellOrderRequest>();

            var sellOrder = sell_order_request.ToSellOrder();

            _stocksRepositoryMock.Setup(temp => temp.CreateSellOrder(It.IsAny<SellOrder>())).ReturnsAsync(sellOrder);

            var sell_order_response_from_create = await _stocksService.CreateSellOrder(sell_order_request);

            sellOrder.SellOrderId = sell_order_response_from_create.SellOrderId;
            SellOrderResponse buyOrderResponse_expected = sellOrder.ToSellOrderResponse();

            sell_order_response_from_create.SellOrderId.Should().NotBe(Guid.Empty);
            sell_order_response_from_create.Should().Be(buyOrderResponse_expected);
        }

        #endregion

        #region GetAllBuyOrders

        [Fact]
        public async Task GetAllBuyOrders_GetAllList_ToBeEmpty()
        {
            var buyOrders = new List<BuyOrder>();

            _stocksRepositoryMock.Setup(temp => temp.GetBuyOrders()).ReturnsAsync(buyOrders);

            var buy_order_responses_from_get_all = await _stocksService.GetAllBuyOrders();

            buy_order_responses_from_get_all.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAllBuyOrders_CorrectOrders()
        {
            var buy_order_requests = new List<BuyOrder>()
            {
                _fixture.Create<BuyOrder>(),
                _fixture.Create<BuyOrder>()
            };
            var expected_buy_order_responses = buy_order_requests.Select(temp => temp.ToBuyOrderResponse());

            _stocksRepositoryMock.Setup(temp => temp.GetBuyOrders()).ReturnsAsync(buy_order_requests);

            var buy_orders_from_get_all = await _stocksService.GetAllBuyOrders();

            buy_orders_from_get_all.Should().BeEquivalentTo(expected_buy_order_responses);
        }

        #endregion

        #region GetAllSellOrders

        [Fact]
        public async Task GetAllSellOrders_GetAllList_ToBeEmpty()
        {
            var sellOrders = new List<SellOrder>();

            _stocksRepositoryMock.Setup(temp => temp.GetSellOrders()).ReturnsAsync(sellOrders);

            var sell_order_responses_from_get_all = await _stocksService.GetAllSellOrders();

            sell_order_responses_from_get_all.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAllSellOrders_CorrectOrders()
        {
            var sell_order_requests = new List<SellOrder>()
            {
                _fixture.Create<SellOrder>(),
                _fixture.Create<SellOrder>()
            };
            var expected_sell_order_responses = sell_order_requests.Select(temp => temp.ToSellOrderResponse());

            _stocksRepositoryMock.Setup(temp => temp.GetSellOrders()).ReturnsAsync(sell_order_requests);

            var sell_orders_from_get_all = await _stocksService.GetAllSellOrders();

            sell_orders_from_get_all.Should().BeEquivalentTo(expected_sell_order_responses);
        }

        #endregion
    }
}