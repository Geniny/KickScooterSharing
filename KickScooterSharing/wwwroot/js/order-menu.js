function order(id) {
    $.ajax({
        method: 'GET',
        url: 'Home/OrderProduct',
        dataType: 'HTML',
        data: { 'id': id },
        success: (data) => {
            if (data == "") {
                $('#smallModal').modal({
                    show : true
                });
            }
            else {
                $('#order-menu').html(data);
            }
        }
    })
}

function book(id) {
    $('#order-menu').load('Home/BookProduct', { 'id': id });
}

function ajaxRequest(id, url, func) {

}