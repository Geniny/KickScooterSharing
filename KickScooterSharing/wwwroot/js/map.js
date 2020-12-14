function initMap() {

    map = new google.maps.Map(document.getElementById('map'), {
        mapId: 'c59998b5dbf7aeb4',
        center: { lat: 53.908408, lng: 27.574425 },
        zoom: 13
    });

    $.getJSON('Home/GetParkingLocations', function (data) {
        setMarkers(map, data, '/icons/parking.png', true);
    });

    loadMarkers(map);
}

function loadMarkers(map) {
    var markers = [];

    $.getJSON('Home/GetProductLocations', function (data) {
        markers = setMarkers(map, data, '/icons/kick-scooter.png', false);
    });

    var interval = setInterval(() => {

        if (markers.length > 0) {
            removeMarkers(markers);
        }
        $.getJSON('Home/GetProductLocations', function (data) {
            markers = setMarkers(map, data, '/icons/kick-scooter.png', false);
        });
    }, 10000);
}

function removeMarkers(markers) {
    for (let i = 0; i < markers.length; i++) {
        markers[i].setMap(null);
    }
    markers.length = 0;
}

function setMarkers(map, markers, icon, isParking) {
    var markersArray = [];
    for (let i = 0; i < markers.length; i++) {
        const marker = new google.maps.Marker({
            position: new google.maps.LatLng(markers[i].latitude, markers[i].longitude),
            icon: icon,
            map: map
        });

        if (isParking) {
            marker.addListener("click", () => {
                map.setZoom(16);
                map.setCenter(marker.getPosition());
            });
        }
        else {
            marker.set('id', markers[i].productId);

            marker.addListener("click", () => {
                if (!($("#order-menu").hasClass('opened'))) {
                    $('#order-menu').addClass('opened');
                    $('#map').animate({ width: '65%' }, 500);
                    $('#order-menu').animate({ width: '35%' }, 500, () => {
                        $('#order-menu').load('Home/OrderMenu', { 'id': marker.id }).css("display", "block");
                    });

                }
                else {
                    $('#order-menu').load('Home/OrderMenu', { 'id': marker.id });
                }
                map.setZoom(18);
                map.setCenter(marker.getPosition());
            });
        }
        markersArray.push(marker);
    }
    return markersArray;
}