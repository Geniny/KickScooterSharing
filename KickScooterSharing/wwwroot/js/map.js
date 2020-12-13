function initMap() {


    map = new google.maps.Map(document.getElementById('map'), {
        mapId: 'c59998b5dbf7aeb4',
        center: { lat: 53.908408, lng: 27.574425 },
        zoom: 13
    });

    $.getJSON('Home/GetProductLocations', function (data) { setMarkers(map, data, '/icons/kick-scooter.png', false); });
    $.getJSON('Home/GetParkingLocations', function (data) { setMarkers(map, data, '/icons/parking.png', true);  });  
}

function setMarkers(map, markers, icon, isParking) {
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
    }
}