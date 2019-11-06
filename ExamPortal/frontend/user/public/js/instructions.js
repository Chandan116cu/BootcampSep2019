$(document).on('click', '.startTest', function() {
    $.ajax('http://localhost:3000/test/accessKey',{
        type:'GET',
        dataType:'JSON',
        headers:{
            examCode: localStorage.getItem('examCode'),
            token: localStorage.getItem('token')
        },
        data:{
            examCode: localStorage.getItem("examCode")
        },
        success: function(data){
            if(data.submitStatus){
                $('.error-msg').text("Test is already Submitted")
            }
            else{
                $(location).attr('href', './question.html')
            }
            
        },
        error: function(error){ }
    })
    
})

function checkTimeForTest(time){
    var x = setInterval(function(){
        var startDate = new Date(time).getTime()
        var presentDate = new Date().getTime()
        let startTime = new Date(time).getTime()
        let presentTime = new Date().getTime()
        let leftTestTime = startTime-presentTime
        var hours = Math.floor((leftTestTime % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
        var minutes = Math.floor((leftTestTime % (1000 * 60 * 60)) / (1000 * 60));
        var seconds = Math.floor((leftTestTime % (1000 * 60)) / 1000);
        document.getElementById("leftTime").innerHTML = "Time Left - "+hours+":"+minutes+":"+seconds
        if((presentTime>startTime) && (presentDate>=startDate)){
            clearInterval(x)
            $('.startTest').removeAttr("disabled")
            document.getElementById('leftTime').innerHTML = ""
        }
    },1000)

}

$(document).ready(function(){
    $.ajax('http://localhost:3000/test/accessKey',{
        type:'GET',
        dataType:'JSON',
        headers:{
            examCode: localStorage.getItem('examCode'),
            token: localStorage.getItem('token')
        },
        data:{
            examCode: localStorage.getItem("examCode")
        },
        success: function(data){
            checkTimeForTest(data.examData.examStartTime)
        },
        error: function(error){ }
    })
})