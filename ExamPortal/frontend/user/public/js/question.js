function loadQuestions(data, startTime, duration, examName) {
    let imageURL, imageStatus
    const questionTemplate = document.querySelector('#question-template').innerHTML
    $('.showTest').text(examName)
    setTimeForTest(startTime, duration)
    $('#options').empty()
    const op = document.querySelector('#options')
    if (data[0].questionImage !== null) {
        imageURL = data[0].questionImage.substring(2, data[0].questionImage.length)
        imageStatus = true
    } else imageStatus = false

    if (data[0].answerType === "singleOption") {
        const html = Mustache.render(questionTemplate, { questions: data[0], types: true, url: imageURL, status: imageStatus })
        op.insertAdjacentHTML("beforeend", html)
    } else {
        const html = Mustache.render(questionTemplate, { questions: data[0], types: false, url: imageURL, status: imageStatus })
        op.insertAdjacentHTML("beforeend", html)
    }
    showPreviousTicks()
}

function loadPaginaton(questions) {
    const paginationTemplate = document.querySelector('#pagination-template').innerHTML
    const op = document.querySelector('.pagination-card')
    for (i = 0; i < questions.length; i++) {
        let j = i + 1
        const html = Mustache.render(paginationTemplate, { pages: j, id: questions[i]._id })
        op.insertAdjacentHTML("beforeend", html)
    }

}

function setTimeForTest(time, duration) {
    let testStartTime = new Date(time).getTime()
    let testEndTime = new Date(testStartTime + duration * 60000).getTime()
    var x = setInterval(function() {
        let testPresentTime = new Date().getTime()
        let leftTestTime = testEndTime - testPresentTime
        var hours = Math.floor((leftTestTime % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
        var minutes = Math.floor((leftTestTime % (1000 * 60 * 60)) / (1000 * 60));
        var seconds = Math.floor((leftTestTime % (1000 * 60)) / 1000);
        document.getElementById("showTime").innerHTML = hours + "h " + minutes + "m " + seconds + "s ";
        if (leftTestTime < 0) {
            clearInterval(x);
            $(location).attr('href', './endTest.html')
            localStorage.clear()
        }
    }, 1000)
}

function showPreviousTicks() {
    let keys = Object.keys(localStorage)
    for (let i = 0; i < keys.length; i++) {
        if (keys[i].length > 20 || keys[i].length == 3) {
            let values = localStorage.getItem(keys[i])
            values = values.split(',')
            for (j = 0; j < values.length; j++) {
                $(`input[name=${keys[i]}][value=${values[j]}]`).prop('checked', true)
            }
        }
    }
}

function loadFullWindow() {
    document.fullscreenEnabled = document.fullscreenEnabled || document.mozFullScreenEnabled || document.documentElement.webkitRequestFullScreen;
    if (document.fullscreenEnabled) {
        let element = document.documentElement
        if (element.requestFullscreen) {
            element.requestFullscreen();
        } else if (element.mozRequestFullScreen) {
            element.mozRequestFullScreen();
        } else if (element.webkitRequestFullScreen) {
            element.webkitRequestFullScreen(Element.ALLOW_KEYBOARD_INPUT);
        }
    }
}

function exitHandler() {
    if (!document.fullscreenElement && !document.webkitIsFullScreen && !document.mozFullScreen && !document.msFullscreenElement) {
        $('#modalEndTest').trigger("click")
    }
}

$(window).on('load', function() {
    $('#fullScreenModal').modal('show')
})

$(document).on('click', '#goFullWindow', function() {
    loadFullWindow()
    $('#fullScreenModal').modal('hide')
})

$(document).ready(function() {
    document.addEventListener('fullscreenchange', exitHandler);
    document.addEventListener('webkitfullscreenchange', exitHandler);
    document.addEventListener('mozfullscreenchange', exitHandler);
    document.addEventListener('MSFullscreenChange', exitHandler);

    const tok = localStorage.getItem('token');
    if (tok == null) {
        location.replace("./examPortal.html")
    }
    $('#nextQuestion').attr('value', 0)
    $('#previousQuestion').attr({ 'value': 0, 'disabled': true })
    
    $.ajax("http://localhost:"+localStorage.getItem('server-port')+'/question', {
        type: 'GET',
        dataType: 'JSON',
        contentType: "application/json;charset=utf-8",
        headers: {
            examCode: localStorage.getItem('examCode'),
            token: localStorage.getItem('token'),
            Authorization: "Bearer "+localStorage.getItem('token')
        },
        data: {
            pageNumber: $('#nextQuestion').attr('value')
        },
        success: function(data) {
            data.duration = parseInt(data.duration)
            loadQuestions(data.questions, data.startTime, data.duration, data.examName)
            loadPaginaton(data.allQuestions)
            if (data.lastQuestionStatus === true) {
                $('#nextQuestion').attr('disabled', true)
            }
            
        },
        error: function(err) {
            console.log(err)
        }
    })
})

$(document).on('click', '#submitAnswer', function() {
    let questionId = $(this).parent().parent().parent().parent().children().children().children().attr('id')
    let examCode = $(this).parent().parent().parent().parent().children().children().children().children().html()
    let value = []
    
    $.each($(`input[name=${questionId}]:checked`), function() {
        value.push($(this).val())
    })
    if (value.length === 0) {
        return
    }

    dataToSend = {
        code: localStorage.getItem('examCode'),
        checkedOption: value,
        qId: questionId
    }
    console.log(dataToSend)
    $.ajax("http://localhost:"+localStorage.getItem('server-port')+'/question', {
        type: 'POST',
        dataType: 'JSON',
        contentType: "application/json;charset=utf-8",
        headers: {
            token: localStorage.getItem('token'),
            Authorization: "Bearer "+localStorage.getItem('token')
        },
        data: JSON.stringify(dataToSend),
        success: function(data) {
            $('#' + questionId + ".circle").css('background-color', "green")
        },
        error: function(error) {
            console.log(error)
        }
    })
})

$(document).on('click', '#nextQuestion', function() {
    let page = parseInt($('#nextQuestion').attr('value'))
    $('#nextQuestion').attr('value', page + 1)
    if ($('#nextQuestion').attr('value') != 0) {
        $('#previousQuestion').removeAttr("disabled");
    }
    $.ajax("http://localhost:"+localStorage.getItem('server-port')+'/question', {
        type: 'GET',
        dataType: 'JSON',
        contentType: "application/json;charset=utf-8",
        headers: {
            examCode: localStorage.getItem('examCode'),
            token: localStorage.getItem('token'),
            Authorization: "Bearer "+localStorage.getItem('token')
        },
        data: {
            pageNumber: $('#nextQuestion').attr('value')
        },
        success: function(data) {
            data.duration = parseInt(data.duration)
            loadQuestions(data.questions, data.startTime, data.duration, data.examName)
            if (data.lastQuestionStatus === true) {
                $('#nextQuestion').attr('disabled', true)
            }
        },
        error: function(err) {
            console.log(err)
        }
    })
})

$(document).on('click', '#previousQuestion', function() {
    let pageNumber = parseInt($('#nextQuestion').attr('value')) - 1
    $('#nextQuestion').attr('value', pageNumber)
    if (pageNumber == 0) {
        $('#previousQuestion').attr({ 'value': 0, 'disabled': true })
    }
    $.ajax("http://localhost:"+localStorage.getItem('server-port')+'/question', {
        type: 'GET',
        dataType: 'JSON',
        headers: {
            examCode: localStorage.getItem('examCode'),
            token: localStorage.getItem('token'),
            Authorization: "Bearer "+localStorage.getItem('token')
        },
        data: {
            pageNumber: pageNumber
        },
        success: function(data) {
            if (data.lastQuestionStatus === false) {
                $('#nextQuestion').removeAttr('disabled')
            }
            loadQuestions(data.questions, data.startTime, data.duration)
        },
        error: function(err) {
            console.log(err)
        }
    })
})

$(document).on('click', '#modalEndTest', function() {
    dataToSend = {
        code: localStorage.getItem("examCode")
    }
    $.ajax("http://localhost:"+localStorage.getItem('server-port')+'/exam/endTest', {
        type: 'POST',
        dataType: 'JSON',
        contentType: "application/json;charset=utf-8",
        headers: {
            examCode: localStorage.getItem('examCode'),
            token: localStorage.getItem('token'),
            Authorization: "Bearer "+localStorage.getItem('token')
        },
        data: JSON.stringify(dataToSend),
        success: function(data) {
            localStorage.clear()
            $(location).attr('href', './endTest.html')
        },
        error: function(error) {
            console.log(error)
        }
    })
})

$(document).on('click', '#resetRadio', function() {
    let questionId = $(this).parent().parent().parent().parent().children().children().children().attr('id')
    $(`input[name=${questionId}]:checked`).prop("checked", false)
    localStorage.removeItem(questionId);
})

$(document).on('click', "input", function() {
    let questionId = $(this)[0].name
    let value = []
    $.each($(`input[name=${questionId}]:checked`), function() {
        value.push($(this).val())
    })
    localStorage.setItem(questionId, value)
    $('#' + questionId + ".circle").css('background-color', "green")
    $('#' + questionId + ".circle").css('color', "white")
})

$(document).on('click', '.circle', function() {
    let upcomingPage = parseInt($(this).children().html()) - 1
    $.ajax("http://localhost:"+localStorage.getItem('server-port')+'/question', {
        type: 'GET',
        dataType: 'JSON',
        headers: {
            examCode: localStorage.getItem('examCode'),
            token: localStorage.getItem('token'),
            Authorization: "Bearer "+localStorage.getItem('token')
        },
        data: {
            pageNumber: upcomingPage
        },
        success: function(data) {
            $('#nextQuestion').attr('value', data.pageNumber)

            if (data.pageNumber === 0) $('#previousQuestion').attr("disabled", true)
            else $('#previousQuestion').removeAttr("disabled")

            if (data.lastQuestionStatus) $('#nextQuestion').attr("disabled", true)
            else $('#nextQuestion').removeAttr("disabled")

            loadQuestions(data.questions, data.startTime, data.duration)
        },
        error: function(error) {
            console.log(error)
        }
    })
})

$('#fullScreenModal').modal({ backdrop: 'static', keyboard: false })

$(document).keypress(function(e) {
    return false
})