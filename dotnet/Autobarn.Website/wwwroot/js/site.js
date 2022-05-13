// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

$(document).ready(connectToSignalR);

function connectToSignalR() {
    var conn = new signalR.HubConnectionBuilder().withUrl("/hub").build();
    conn.on("PopUpMagicNotification", showNewVehicleNotification);
    conn.start().then(function () {
        console.log('SignalR is running! Hooray!');
    }).catch(function (err) {
        console.log(err);
    });
}

function showNewVehicleNotification(user, message) {
    console.log(`Message from ${user}`);
    var data = JSON.parse(message);
    var html = `<div>New vehicle! 
${data.Make} ${data.Model}, 
${data.Color}, ${data.Year}. Price ${data.Price}${data.CurrencyCode}<br />
<a href="/vehicles/details/${data.Registration}">click for more...</a></div>`;
    const $div = $(html);
    $div.css("background-color", data.Color);
    const $target = $("div#signalr-notifications");
    $target.prepend($div);
    window.setTimeout(function () {
        $div.fadeOut(500, function() { $div.remove() });
    }, 2000);
}