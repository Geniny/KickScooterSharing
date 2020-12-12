function order(id, minutes) {
    $.ajax({
        method: 'GET',
        url: 'Home/OrderProduct',
        dataType: 'HTML',
        data: { 'id': id, 'minutes': minutes },
        success: (data) => {
            console.log(data);
            if (data == null) {
                $('#smallModal').modal({
                    'focus': 'true'
                })
            }
            else {
                $('#order-menu').text(data);
            }
        }
    })
}

function book(id) {
    $('#order-menu').load('Home/BookProduct', { 'id': id });
}

function ajaxRequest(id, url, func) {
    
}