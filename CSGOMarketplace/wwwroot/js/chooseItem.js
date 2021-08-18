window.onload = function () {
    var itemsRow = document.getElementById('itemsRow');
    document.getElementById("refresh").addEventListener('click', function() {
        sessionStorage.clear();
        itemsRow.innerHTML = '';
        loadInventory();

    })
    if (sessionStorage.getItem("inventory") == null) {
        loadInventory();
    } else {
        displayItems(JSON.parse(sessionStorage.getItem("inventory")));
    }

} 

function loadInventory() {
    let steamId = $("#providerKey").text();
    var loadingText = document.createElement('a');
    loadingText.textContent = 'Loading your inventory...';
    loadingText.classList.add('text-center');
    itemsRow.appendChild(loadingText);
    let endpoint = 'https://steamcommunity.com/profiles/' + steamId + '/inventory/json/730/2';
    console.log('working...');
    $.getJSON(endpoint, data => requestInv(data));
    loadingText.classList.add('d-none');
}

function requestInv(data) {
    console.log(data);
    let items = Object.keys(data.rgDescriptions).map(i => data.rgDescriptions[i]);
    let ids = Object.keys(data.rgInventory).map(i => data.rgInventory[i]);
    for (var i = 0; i < ids.length; i++) {
        for (var k = 0; k < items.length; k++) {
            if (ids[i].classid == items[k].classid) {
                items[k].assetId = ids[i].id;
            }
        }
    }
    sessionStorage.setItem("inventory", JSON.stringify(items));
    setTimeout(function () { sessionStorage.clear(); }, (10 * 60 * 10000));
    displayItems(items);
}

function displayItems(items) {
    let steamId = $("#providerKey").text();
    items.forEach(item => {
        if (!item.actions || !item.marketable) {
            return;
        }
        let d = item.actions[0].link.split('D');
        d = d[d.length - 1];

        let outerCol = document.createElement('div');
        outerCol.classList.add('col-md-3');
        let card = document.createElement('div');
        card.classList.add('card');
        card.classList.add('mb-2');
        let img = document.createElement('img');
        img.classList.add('card-img-top');
        let iconUrl = "https://api.steamapis.com/image/item/730/" + item.market_name;
        //if (item.icon_url_large == undefined) {
        //    iconUrl = `https://cdn.steamcommunity.com/economy/image/${item.icon_url}`;
        //} else {
        //    iconUrl = `https://cdn.steamcommunity.com/economy/image/${item.icon_url_large}`;
        //}
        img.src = iconUrl;
        img.alt = item.market_name;
        let cardCenterDiv = document.createElement('div');
        cardCenterDiv.classList.add('card-body');
        cardCenterDiv.classList.add('text-center');
        let h5 = document.createElement('h5');
        h5.classList.add('card-title');
        h5.classList.add('text-center');
        h5.textContent = item.market_name
        let a = document.createElement('a');
        a.href = `/Items/Sell?s=${steamId}&a=${item.assetId}&d=${d}&iconurl=${iconUrl}`;
        a.classList.add('btn');
        a.classList.add('btn-primary');
        a.textContent = 'Sell';
        cardCenterDiv.appendChild(h5);
        cardCenterDiv.appendChild(a);
        card.appendChild(img);
        card.appendChild(cardCenterDiv);
        outerCol.appendChild(card);
        itemsRow.appendChild(outerCol);
    });
}