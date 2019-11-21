function removeQuestion(id){

    console.log(id)
    let qsId = $("#"+id).parent().parent().attr('id')
    console.log(qsId)
    $.ajax("http://localhost:"+localStorage.getItem('server-port')+"/exam/question/"+qsId, {

        type: 'DELETE',
        dataType: 'json',
        contentType: "application/json",
        headers:{
            token:localStorage.getItem('token')
        },
        success: function(data) {
            location.reload(true)
            },
        error: function(error) {
           console.log(error)
            }
        }) 
}
function setQsId(id){
    console.log('id ',id)
    $("#delQ").attr('id', id)
}
function updateQues(id,type) {
    let opt1 = '',opt2= '', opt3= '', opt4 ='', answer =''
    let questionText= $('#addtestQuestion').val()
    let weightage = $('#addtestWeightage').val()
    if(type == 'multipleOption'){
       opt1 = $('#addtestOption1').val()
       opt2 = $('#addtestOption2').val()
       opt3 = $('#addtestOption3').val()
       opt4 = $('#addtestOption4').val()
       $.each($("input[name='option']:checked"), function () {
        if ($(this).val()) {
            answer += $(this).val() + " "
        }
     });
     answer = answer.trim()
    }else{
        opt1 = $("#addtestOption1G").val()
        opt2 = $("#addtestOption2G").val()
        opt3 = $("#addtestOption3G").val()
        opt4 = $("#addtestOption4G").val()
        answer = $("input[name='option1']:checked").val()
    }
    var formData = new FormData();
            formData.values('questionImage')

            var formData = new FormData();
            formData.append('questionText', questionText);
            formData.append('answer', answer);
            formData.append('option1', opt1);
            formData.append('option2', opt2);
            formData.append('option3', opt3);
            formData.append('option4', opt4);
            formData.append('weightage', weightage);
            formData.append('answerType', "multipleOption");
            formData.append('questionImage', $('input[type=file]')[0].files[0]);
            // return
    $.ajax("https://node-examportal.herokuapp.com/exam/question/" + id, {
        type: 'PATCH',
        dataType: 'json',
        contentType: false,
        processData: false,
        headers: {
            token: localStorage.getItem('token'),
            Authorization: "Bearer "+localStorage.getItem('token')

        },
        data: formData,
        success: function(data) {
            location.reload(true)
        },
        error: function(error) {
            console.log(error)
        }
    })
}

function editQuestion(id) {
    let qid = $("#" + id).parent().parent().attr('id')
    let pid = $("#" + qid).parent().parent().parent().parent().attr('id')
    $('#' + pid).hide()
    $.ajax("http://localhost:"+localStorage.getItem('server-port')+"/exam/question/" + qid, {
        type: 'GET',
        dataType: 'json',
        contentType: "application/json",
        headers: {
            token: localStorage.getItem('token'),
            Authorization: "Bearer "+localStorage.getItem('token')
        },
        success: function(data) {
            console.log(data);
            if(data.answerType== "multipleOption"){
                let arr = data.answer.split(' ')
                let editTemplate = $("#edit-question-template").html();
                $("#display-edit-form").append(Mustache.render(editTemplate, data))
                    let checkBox = $('input[type=checkbox][name=option]')
                    $.each(checkBox,(i,chk)=>{
                        if( arr.includes($(chk).val())){
                            $(chk).prop('checked',true)
                        }
                    })
                if( data.questionImage != null && data.questionImage != undefined){
                    document.getElementById("myImage").value = data.questionImage
                }
            }else if( data.answerType=="singleOption"){
                let editTemplate = $("#edit-single-option").html();
                $("#display-edit-form").append(Mustache.render(editTemplate, data))
                let radioBtn = $('input[type=radio][name=option1]')
                    $.each(radioBtn,(i,radio)=>{
                        if(radio.value == data.answer){
                        $(radio).prop('checked',true)
                   }
               })
                
            }
        },
        error: function(error) {
            console.log(error)
        }
    })
}
$(document).ready(function(){
    let examCode = localStorage.getItem('examCode')
    let url = "http://localhost:"+localStorage.getItem('server-port')+"/exam/"+ encodeURIComponent(examCode)+"/question"
    $.ajax(url, {
        type: 'GET',
        dataType: 'json',
        contentType: "application/json;charset=utf-8",
        headers: {
            token: localStorage.getItem('token'),
            Authorization: "Bearer "+localStorage.getItem('token')
        },
        success: function(data) {
            if( data.msg == 'No question'){
                alert("No question added in this exam")
                return
            }
            $.each(data, (index, item) => {
                let indexTemplate = $("#index-template").html();
                item.index = index + 1
                $("#question-Index").append(Mustache.render(indexTemplate, item))
                if( item.questionImage != null ){
                    item.status = true
                }else{
                    item.status = false
                }
                let questionContent = $("#question-template-body").html()
                item.index = index + 1
                $("#question-Display").append(Mustache.render(questionContent, item))

            })
        },
        error: function(error) {
            if( error.responseText =='Not Found'){
                alert("This exam has no Questions")
                $(location).attr('href','../views/exam.html')
            }
        }
    })
})