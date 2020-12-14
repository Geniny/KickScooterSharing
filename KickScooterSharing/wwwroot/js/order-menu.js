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

function returnProduct(id) {
    $.ajax({
        method: 'GET',
        url: 'Home/ReturnProduct',
        dataType: 'HTML',
        data: { 'id': id },
        success: (data) => {
            console.log(data);
            if (data == "") {
                $('#order-menu').empty();
                $('#map').animate({ width: '100%' }, 500);
                $('#order-menu').animate({ width: '0%' }, 500, () => {
                    $('#order-menu').css('display', 'none');
                });
                $('#modal-info').html('<span class="text-success">You have finished! <br> thank you for using our services! <br> Have a good day!</span>');
                $('#order-menu').removeClass('opened');
                $('#smallModal').modal({
                    show: true
                });
            }
            else {
                $('#modal-info').html(data);
                $('#smallModal').modal({
                    show: true
                });
            }
        }
    })
}