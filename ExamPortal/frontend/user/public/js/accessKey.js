$(document).ready(function(){
    $.ajax("http://localhost:"+localStorage.getItem('server-port')+'/loggedIn',{
        type: 'GET',
        dataType: 'JSON',
        headers:{
            "token":localStorage.getItem('token'),
            Authorization: "Bearer "+localStorage.getItem('token')
        },
        success: function(data){
            localStorage.setItem('name',data.name)
            document.getElementById('username').innerHTML = "Hie, "+data.name
        }
    })

    $(".inputBox").keypress(function(event){
        if(event.which == 13){
            event.preventDefault();
            $("#checkAccessKey").trigger("click");
        }
    })
})
$(document).on('click', '#checkAccessKey', function() {
    const tok = localStorage.getItem('token');

    if (tok == null) {
        location.replace("./login.html")
    }
    $.ajax("http://localhost:"+localStorage.getItem('server-port')+'/exam/accessKey', {
        type: 'POST',
        dataType: 'JSON',
        contentType: "application/json;charset=utf-8",
        headers:{
            token: localStorage.getItem('token'),
            Authorization: "Bearer "+localStorage.getItem('token')
        },
        data: JSON.stringify({
            examCode: $(".inputBox").val()
        }),
        success: function(data) {
            localStorage.setItem('examCode', $(".inputBox").val())
            $(location).attr('href', '../views/instructions.html')
        },
        error: function(error) {
            $('.error-msg').text("Wrong Access key")
        }
    })
})

function logout() {
    localStorage.removeItem("token")
    location.replace("./examPortal.html")
}