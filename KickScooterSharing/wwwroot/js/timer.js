class Timer {

    constructor(startTime, productId) {

        this.stop = false;
        this.startTime = startTime;
        this.productId = productId;
    }

    setListener(elementId, timer) {
        $(elementId).click(function () {
            timer.stopTimer();
            order(timer.productId);
        });
    }

    countdown(distance) {

        var hours = Math.floor((distance % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
        var minutes = Math.floor((distance % (1000 * 60 * 60)) / (1000 * 60));
        var seconds = Math.floor((distance % (1000 * 60)) / 1000);
        return (hours + "hour(s), " + minutes + "minute(s), " + seconds + "second(s)");
    }

    startTimer(elementId) {
        var currentTime = new Date().getTime();

        var x = setInterval(() => {
            if (stop) {
                clearInterval(x);
                $('#order-menu').load('Home/OrderMenu', { 'id': super.productId });
                return;
            }
            var distance = currentTime - this.startTime;
            document.getElementById(elementId).innerHTML("Time : " + super.countdown(distance));

        }, 1000);
    }

    stopTimer() {
        this.stop = true;
    }
}    



function startTimer(startTime, productId) {

    var currentTime = new Date().getTime();

    var x = setInterval(() => {
        if (stop) {
            clearInterval(x);
            $('#order-menu').load('Home/OrderMenu', { 'id': super.productId });
            return;
        }
        var distance = currentTime - this.startTime;
        document.getElementById(elementId).innerHTML("Time : " + super.countdown(distance));

    }, 1000);
}

function countdown(distance) {

    var hours = Math.floor((distance % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
    var minutes = Math.floor((distance % (1000 * 60 * 60)) / (1000 * 60));
    var seconds = Math.floor((distance % (1000 * 60)) / 1000);
    return (hours + "hour(s), " + minutes + "minute(s), " + seconds + "second(s)");
}