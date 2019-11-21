$(document).ready(function() {
    const tok = localStorage.getItem('token');
    if (tok == null) {
        location.replace("../../index.html")
    }

})

function showEdit() {
    $("#showEditDiv").fadeIn("slow");
    $.ajax("http://localhost:"+localStorage.getItem('server-port')+"/loggedIn", {
        type: 'GET',
        dataType: 'JSON',
        headers: {
            "token": localStorage.getItem('token'),
            'Authorization': 'Bearer '+localStorage.getItem('token')

        },
        success: function(data) {

            changeInputFields(data)

        },
        error: function(error) {
            console.log('not working')
        }
    })
}

function showName() {
    $.ajax("http://localhost:"+localStorage.getItem('server-port')+"/loggedIn", {
        type: 'GET',
        dataType: 'JSON',
        headers: {
            "token": localStorage.getItem('token'),
            'Authorization': 'Bearer '+localStorage.getItem('token')

        },
        success: function(data) {
            console.log(data)
            document.getElementById('span').innerHTML = 'Welcome ' + data.name + '! &nbsp; &nbsp; '
            localStorage.setItem("loggedInName", data.name)
        },
        error: function(error) {
            console.log('not working')
        }
    })

}

function changeInputFields(data) {
    document.getElementById('loggedInEmail').value = data.email;
    document.getElementById('loggedInName').value = data.name;
    document.getElementById('loggedInPhone').value = data.phoneNumber;
    document.getElementById('loggedInCollege').value = data.collegeName;

}
function editDetails() {
    var email = document.getElementById('loggedInEmail').value
    var name = document.getElementById('loggedInName').value
    var phone = document.getElementById('loggedInPhone').value
    var college = document.getElementById('loggedInCollege').value
    var pass = document.getElementById('loggedInPassword').value

    $.ajax("http://localhost:"+localStorage.getItem('server-port')+"/examiner", {
        type: 'PATCH',
        dataType: 'JSON',   
        headers: {
            "token": localStorage.getItem('token'),
             'Authorization': 'Bearer '+localStorage.getItem('token')

        },
        data: JSON.stringify({
            "email": email,
            "name": name,
            "phoneNumber": phone,
            "collegeName": college,
            "password": pass
        }),
        
        success: function(data) {
            window.alert('User Details Updated !')
            hideEditDetails()
            showName()


        },
        error: function(error) {
            // console.log('not updated')
            window.alert('Not Updated')
        }
    })
}

function hideEditDetails() {
    $("#showEditDiv").fadeOut("slow");
}

function logout() {
    localStorage.removeItem("token")
    localStorage.clear()
    location.replace("../../index.html")
}