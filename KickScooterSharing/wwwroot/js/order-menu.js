function order(id) {
    ajaxRequest(id, 'Home/OrderProduct', () => {

    });
}

function book(id) {
    ajaxRequest(id, 'Home/BookProduct', () => {

    });
}

function ajaxRequest(id, url, func) {
    $.ajax({
        method: 'POST',
        url: url,
        data: { 'id': id },
        success: func()
    })
}