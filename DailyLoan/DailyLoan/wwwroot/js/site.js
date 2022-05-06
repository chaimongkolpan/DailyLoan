var pathname = window.location.pathname;
var con = 0, ale = 0;
var audio = new Audio('/alert.mp3');
$(document).ready(function () {
    $.ajax({
        type: "GET",
        url: root + "/GetAlert",
        contentType: "application/json; charset=utf-8;",
        dataType: "json",
        success: function (data) {
            if (data != undefined) {
                if (data.length == 3) {
                    if (data[0] > 0) {
                        ale = data[0];
                        $("#alertcount").text(data[0]);
                        $("#alertcount").show();
                        if (data[2] == 1)audio.play();
                    }
                    if (data[1] > 0) {
                        con = data[1];
                        $("#contractcount").text(data[1]);
                        $("#contractcount").show();
                        if (data[2] == 1)audio.play();
                    }
                }
            }
        },
        error: function (e) { console.log(e) }
    });
    setInterval(function () {
        $.ajax({
            type: "GET",
            url: root + "/GetAlert",
            contentType: "application/json; charset=utf-8;",
            dataType: "json",
            success: function (data) {
                if (data != undefined) {
                    if (data.length == 3) {
                        if (data[0] > 0) {
                            if (ale < data[0]&&data[2]==1) audio.play();
                            ale = data[0];
                            $("#alertcount").text(data[0]);
                            $("#alertcount").show();
                        }
                        if (data[1] > 0) {
                            if (con < data[1] && data[2] == 1) audio.play();
                            con = data[1];
                            $("#contractcount").text(data[1]);
                            $("#contractcount").show();
                        }
                    }
                }
            },
            error: function (e) { console.log(e) }
        });
    }, 5000);
    if (pathname == '/Management/User') {
        $('#userlink').addClass('active');
    }
    if (pathname == '/SearchUser') {
        $('#userlink').addClass('active');
    }
    if (pathname == '/Pay/Customer') {
        $('#customerlink').addClass('active');
    }
    if (pathname == '/SearchCustomerPage') {
        $('#customerlink').addClass('active');
    }
    if (pathname == '/Pay/Contract') {
        $('#contractlink').addClass('active');
    }
    if (pathname == '/SearchContract') {
        $('#contractlink').addClass('active');
    }
    if (pathname == '/Pay/History') {
        $('#historylink').addClass('active');
    }
    if (pathname == '/SearchHistory') {
        $('#historylink').addClass('active');
    }
    if (pathname == '/Pay/setting_system') {
        $('#settinglink').addClass('active');
        $('#settingsystemlink').addClass('active');
    }
    if (pathname == '/Pay/setting_home') {
        $('#settinglink').addClass('active');
        $('#settinghomelink').addClass('active');
    }
    if (pathname == '/Pay/setting_daily') {
        $('#settinglink').addClass('active');
        $('#settingdailylink').addClass('active');
    }
    if (pathname == '/Pay/setting_monthly') {
        $('#settinglink').addClass('active');
        $('#settingmonthlylink').addClass('active');
    }
    if (pathname == '/Pay/MonthlyReport') {
        $('#monthlyreportlink').addClass('active');
    }
    if (pathname == '/Pay/AdminReportDaily') {
        $('#adminreportdailylink').addClass('active');
    }

    if (pathname == '/Pay/Collector') {
        $('#collectlink').addClass('active');
    }
    if (pathname == '/SearchCollector') {
        $('#collectlink').addClass('active');
    }
    if (pathname == '/Pay/DailyReport') {
        $('#dailyreportlink').addClass('active');
    }
});