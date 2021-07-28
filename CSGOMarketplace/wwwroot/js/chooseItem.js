window.onload = function () {
    let itemsRow = document.getElementById('itemsRow');
    var loadingText = document.createElement('a');
    loadingText.textContent = 'Loading your inventory...';
    loadingText.classList.add('text-center');
    itemsRow.appendChild(loadingText);
    let steamId = $("#providerKey").text();
    let endpoint = 'https://steamcommunity.com/profiles/' + steamId + '/inventory/json/730/2';
    console.log('working...');
    $.getJSON(endpoint, function (data) {
        let items = Object.keys(data.rgDescriptions).map(i => data.rgDescriptions[i]);
        let ids = Object.keys(data.rgInventory).map(i => data.rgInventory[i]);

        for (var i = 0; i < ids.length; i++) {
            for (var k = 0; k < items.length; k++) {
                if (ids[i].classid == items[k].classid) {
                    items[k].assetId = ids[i].id;
                }
            }
        }

        loadingText.classList.add('d-none');
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
            let iconUrl = `https://cdn.steamcommunity.com/economy/image/${item.icon_url_large}`;
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
            a.href = `/Items/Sell?s=${steamId}&a=${item.assetId}&d=${d}&iconurl=${item.icon_url_large}`;
            a.classList.add('btn');
            a.classList.add('btn-primary');
            a.textContent = 'Sell';
            cardCenterDiv.appendChild(h5);
            cardCenterDiv.appendChild(a);
            card.appendChild(img);
            card.appendChild(cardCenterDiv);
            outerCol.appendChild(card);
            itemsRow.appendChild(outerCol);
        })
    });
} 

