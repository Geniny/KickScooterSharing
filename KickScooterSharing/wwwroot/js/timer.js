class Timer{

    constructor(productId) {
        this.stop = false;
        this.productId = productId;
    }

    countdown(distance) {

        var hours = Math.floor((distance % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
        var minutes = Math.floor((distance % (1000 * 60 * 60)) / (1000 * 60));
        var seconds = Math.floor((distance % (1000 * 60)) / 1000);
        return (hours + "hour(s), " + minutes + "minute(s), " + seconds + "second(s)");
    }

    stopTimer() {
        this.stop = true;
    }
}

class BookTimer extends Timer {

    constructor(endTime, productId) {
        this.endTime = endTime;
        super(productId);
    }

    setListener(elementId) {
        $(elementId).click(function () {
            super.stopTimer();
            order(super.productId);
        });
    }

    startTimer(elementId) {
        var currentTime = new Date().getTime();
        var x = setInterval(() => {
            var distance = this.endTime - currentTime;
            if (distance < 0 || stop) {
                clearInterval(x);
                $('#order-menu').load('Home/OrderMenu', { 'id': super.productId });
                return;
            }
            
            document.getElementById(elementId).innerHTML("order (" + super.countdown(distance) + ")");

        }, 1000);
    }
}

class OrderTimer extends Timer {

    constructor(startTime) {
        this.startTime = startTime;
        super(productId);
    }

    setListener(elementId) {
        $(elementId).click(function () {
            super.startTimer();
            //
            // finish request
            //
        });
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
}