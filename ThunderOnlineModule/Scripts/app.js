//$.connection.hub.url = 'http://thunderasp.azurewebsites.net/signalr';
var printer = $.connection.zionHub;
$.connection.hub.start().done(function () {
    console.log("Conección Establecida");
});

function SendToPrint(billId) {
    printer.server.sendPrintBill(billId);
}

console.log("Si esta leyendo");
