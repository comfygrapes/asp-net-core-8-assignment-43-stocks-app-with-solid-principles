const apiKey = document.querySelector('#apiKey').value;
const stockSymbolElement = document.getElementById('stockSymbol');
const stockSymbol = stockSymbolElement.value;
const priceElement = document.querySelector('.price');
const socket = new WebSocket(`wss://ws.finnhub.io?token=${apiKey}`)

// Connection opened -> Subscribe
socket.addEventListener('open', function (event) {
    socket.send(JSON.stringify({ 'type': 'subscribe', 'symbol': `${stockSymbol}` }))
});

// Listen for messages
socket.addEventListener('message', function (event) {
    console.log('Message from server ', event.data);

const data = JSON.parse(event.data);
if (data.type === 'trade') {
        const latestTrade = data.data[0];
priceElement.textContent = latestTrade.p.toFixed(2);
    }
});

// Unsubscribe
var unsubscribe = function(symbol) {
    socket.send(JSON.stringify({ 'type': 'unsubscribe', 'symbol': symbol }))
}

window.addEventListener('beforeunload', function () {
    unsubscribe(stockSymbol);
socket.close();
});
