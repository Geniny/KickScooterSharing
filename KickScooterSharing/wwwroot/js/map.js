function initMap() {

    var scooters = $.getJSON('Home/GetProductLocations');
    var parkings = $.getJSON('Home/GetParkingLocations');

    map = new google.maps.Map(document.getElementById('map'), {
        mapId: 'c59998b5dbf7aeb4',
        center: { lat: 53.908408, lng: 27.574425 },
        zoom: 13
    });

    setMarkers(map, scooters, '/icons/kick-scooter.png', false);
    setMarkers(map, parkings, '/icons/parking.png', true);  
}

function setMarkers(map, markers, icon, isParking) {
    for (let i = 0; i < markers.length; i++) {
        const marker = new google.maps.Marker({
            position: new google.maps.LatLng(markers[i].Latitude, markers[i].Longitude),
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
            marker.set('id', markers[i].Id);

            marker.addListener("click", () => {
                if (!($("#order-menu").hasClass('opened'))) {
                    $('#order-menu').addClass('opened');
                    $('#map').animate({ width: '65%' }, 500);
                    $('#order-menu').animate({ width: '35%' }, 500, () => {
                        $('#order-menu').load('Home/OrderMenu', { 'id': marker.id })
                    });
                }
                map.setZoom(18);
                map.setCenter(marker.getPosition());
            });
        }
    }
}