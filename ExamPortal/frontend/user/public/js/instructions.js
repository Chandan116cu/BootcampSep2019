$(document).on('click', '.startTest', function() {
    $.ajax("http://localhost:"+localStorage.getItem('server-port')+'/exam/accessKey', {
        type: 'GET',
        dataType: 'JSON',
        headers: {
            examCode: localStorage.getItem('examCode'),
            token: localStorage.getItem('token'),
            Authorization: "Bearer "+localStorage.getItem('token')
        },
        data: {
            examCode: localStorage.getItem("examCode")
        },
        success: function() {
                $(location).attr('href', './question.html')
            },
        error: function(error) { console.log(error)}
    })

})

function checkTimeForTest(time,duration) {
    var x = setInterval(function() {
        var startDate = new Date(time).getDate()
        var presentDate = new Date().getDate()
        let startTime = new Date(time).getTime()
        let presentTime = new Date().getTime()
        let leftTestTime = startTime - presentTime
        var hours = Math.floor((leftTestTime % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
        var minutes = Math.floor((leftTestTime % (1000 * 60 * 60)) / (1000 * 60));
        var seconds = Math.floor((leftTestTime % (1000 * 60)) / 1000);
        
        document.getElementById("leftTime").innerHTML = "Time to Start Exam - " + hours + "h " + minutes + "m " + seconds+"s"
        var testEndTime = startTime + duration
        if (presentDate == startDate){
            if(presentTime>= startTime && presentTime <= testEndTime){
                clearInterval(x);
                $('.startTest').removeAttr('disabled')
                document.getElementById('leftTime').innerHTML = ""
            }
            else if(presentTime > testEndTime){
                clearInterval(x);
                $('.startTest').attr('disabled')
                $('.error-msg').text("Test link expired")
                $('#leftTime').text("")
            }
        }
        else if(presentDate > startDate){
            clearInterval(x)
            $('.startTest').attr('disabled')
            $('.error-msg').text("Test link expired")
            document.getElementById('leftTime').innerHTML = ""
        }
        else{
            clearInterval(x)
            $('.startTest').attr('disabled')
            $('.error-msg').text("Test not started yet")
        }
    }, 1000)

}

$(document).ready(function() {
    var token = localStorage.getItem('token')
    if(token == null){
        $(location).attr('href','./login.html')
    }
    $.ajax("http://localhost:"+localStorage.getItem('server-port')+'/exam/accessKey', {
        type: 'GET',
        dataType: 'JSON',
        headers: {
            examCode: localStorage.getItem('examCode'),
            token: localStorage.getItem('token'),
            Authorization: "Bearer "+localStorage.getItem('token')
        },
        success: function(data) {
            var minToMilliSec = data.examData.examDuration * 60000;
            checkTimeForTest(data.examData.examStartTime,minToMilliSec)
            document.getElementById('username').innerHTML = "Hie "+localStorage.getItem('name')
        },
        error: function(error) {
            if(error.status === 401){
                location.replace('./examPortal.html')
            }
        }
    })
})

function logout() {
    localStorage.removeItem("token")
    location.replace("./examPortal.html")
}
