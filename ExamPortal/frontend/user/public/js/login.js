$(document).on('click', '#loginButton', function() {
        let email = $('#inputEmail').val()
        let password = $('#inputPassword').val()
        $.ajax("http://localhost:"+localStorage.getItem('server-port')+'/login', {
            type: 'POST',
            dataType: 'JSON',
            contentType: "application/json;charset=utf-8",
            beforeSend: function() {
                $('.main').animate({ opacity: 0.4 })
                $('.mod').fadeIn()
                $('.spinner').show()
            },
            data: JSON.stringify({
                'email': email,
                'password': password
            }),
            success: function(data) {
                localStorage.setItem('token', data.token)
                if (data.accountType == "Examiner")
                    $(location).attr('href', '../../examiner/views/examiner.html')
                else if (data.accountType == "Student")
                    $(location).attr('href', './accessKey.html')
                else {
                    $(location).attr('href', '../../admin/views/adminHome.html')
                }
            },
            error: function(data) {
                $('.spinner').hide()
                console.log(data)
                window.alert(data.responseJSON.message)
                location.reload();
            }

        })
    })
    // })