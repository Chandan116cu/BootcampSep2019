function showpassword()
{
  var x = document.getElementById("password");
  if (x.type === "password") {
    x.type = "text";
  } else {
    x.type = "password";
  }
}
function ValidateEmail(email)
{
    var mailformat =/^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,4})+$/;
    
   
    if (mailformat.test(email) == false) 
            {
                return (false);
            }
            else
            {
                return (true);
            }
}
function ValidatePhoneno(phoneno)
{
    var numbersformat = /^[0-9]+$/;
    if (numbersformat.test(phoneno) == false) 
            {
                return (false);
            }
            else
            {
                return (true);
            }
}
function ValidateName(name)
{
    var lettersformat = /^[a-zA-Z-,]+(\s{0,1}[a-zA-Z-, ])*$/
    if (lettersformat.test(name) == false) 
            {
                return (false);
            }
            else
            {
                return (true);
            }
}
function ValidatePassword(Password)
{
    //6 to 15 characters which contain at least one lowercase letter, one uppercase letter, one numeric digit, and one special character
    var passwordformat=  /^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[^a-zA-Z0-9])(?!.*\s).{6,15}$/;
    if (passwordformat.test(Password) == false) 
            {
               return (false);
            }
            else
            {
                return (true);
            }
}
function logout() {
    localStorage.removeItem("token");
    localStorage.clear()
    window.location.replace("../../index.html");
}
$(document).ready(function() {
    $("#submit").click(function(e) {
        e.preventDefault();
        var email = document.getElementById("email").value;
        var name = document.getElementById("name").value;
        var password = document.getElementById("password").value;
        var accountype = document.getElementById("accountype").value;
        var phoneno = document.getElementById("phoneno").value;
        var collegename = document.getElementById("collegename").value;
        var flag = 1;
        if (email === ""||ValidateEmail(email)===false) {
            flag = 0;
            window.alert("Invalid Email Address");
        } 
       else if (name === ""||ValidateName(name)===false||name.toString().length>=20) {
            flag = 0;
            window.alert("Invalid Name");
        } else if (password === ""||ValidatePassword(password)===false) {
            flag = 0;
            window.alert("Password contains 6-15 characters which contain atleast one Uppercase,lowercase,digit and special chararcter");
        }
        else if (phoneno.toString().length != 10||ValidatePhoneno(phoneno)===false) {
            flag = 0;
            window.alert("Phone number must be valid");
        } else if (collegename === ""||ValidateName(collegename)===false||collegename.toString().length>=20) {
            flag = 0;
            window.alert("Invalid College Name");
        }
        if (flag == 1) {
            //console.log("hello buddy");
            $.ajax("http://localhost:"+localStorage.getItem('server-port')+"/examiner", {
                type: "POST",
                dataType: "json",
                contentType: "application/json",

                data: JSON.stringify({
                    "email": email,
                    "name": name,
                    "password": password,
                    "accountType": accountype,
                    "collegeName": collegename,
                    "phoneNumber": phoneno

                }),
                success: function(recent) {
                    console.log("..........");
                    console.log(recent.message);
                    if (recent.message == "user already exist") {
                        window.alert("User Already Exist");
                    } else {
                        window.alert("Account Created");
                        location.replace("../views/adminHome.html")
                    }

                },
                error: function() {
                    console.log("Something went wrong");
                }

            });
        }
        flag = 1;
    });
});