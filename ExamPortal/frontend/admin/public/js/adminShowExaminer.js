function deleteexaminer(id)
{
  $("#modelid").attr('id',id)
}
function logout()
{
   localStorage.removeItem("token");
   window.location.replace("../../user/views/login.html");
}
function samepage()
{
  window.location.replace("../views/adminShowExaminer.html");
}

function loadSetupExaminerPage(data){
  $('#performance').empty()
  $.get('./adminSetupExaminer.html',function(template){
    var rendered = Mustache.render(template,{data:data})
    $('#targetPage').html(rendered)
  })
}


$(document).ready(function () {
  // const tok =localStorage.getItem('token');
  // if(tok == null)
  // {
  //   location.replace("../../index.html")
  // }
  //$.ajax('http://localhost:'+localStorage.getItem('server-port')+'/exam/accessKey', {
  $.ajax("http://localhost:"+localStorage.getItem('server-port')+"/examiner", {
    type: "GET",
    dataType: 'JSON',
    contentType: "application/json;charset=utf-8",
    headers:{
      token: localStorage.getItem('token')
    },
    success: function (recent) {
      display(recent);
      // console.log(recent);
    },
    error: function (error) {
      console.log(error)
      console.log("Something went wrong");
    }

  });

  function display(recent) {
    const displaytemplate = document.querySelector("#index-template").innerHTML;
    const html = Mustache.render(displaytemplate, { data: recent })
    const performance = document.querySelector("#performance");
    performance.insertAdjacentHTML("beforeend", html)
  }
  $(document).on('click', '.deleteButton', function () {
    let id = $(this).attr('id')
    console.log(id);
    $.ajax("http://localhost:"+localStorage.getItem('server-port')+"/examiner/"+id, {
      type: "DELETE",
      dataType: "json",
      contentType: "application/json",
      data: JSON.stringify(
        {
          "_id": id
        }
      ),
      success: function (recent) {
       // display(recent);
        window.location.replace("adminShowExaminer.html")
      },
      error: function () {
        console.log("Something went wrong");
      }

    })
  })

  $(document).on('click', '.viewButton', function () {
    let id = $(this).attr('id')
    $.ajax("http://127.0.0.1:"+localStorage.getItem('server-port')+"/examiner/id", {
      type: "GET",
      dataType: "json",
      contentType: "application/json",
      data: {
        "id": id
      },
      success: function (recent) {
        console.log(recent);
        //loadSetupExaminerPage(recent)
      },
      error: function () {
        console.log("Something went wrong");
      }

    })
  })
})