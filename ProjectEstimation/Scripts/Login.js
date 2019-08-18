// Auther   : Sivabalan S
// Date     : 17Aug'19
// Purpose  : Login screen control activity
// !---------------------------------------------------------------------!

$(document).ready(function () {
    $("#header").hide();
    function AJAX(url, method, formData, returnMethod, isReset) {
        $.ajax({
            url: url,
            type: method,
            data: JSON.stringify(formData),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            success: function (result) {
                if (returnMethod == "alert") {
                    if ("Loggin Successfully..!") {
                        window.location.href = '/Dashboard/Dashboard/';
                    } else {
                        alert(result);
                    }
                }
            },
            complete: function(){

            },
            error: function(){

            }
        });
    }

    //Login validate user creadentials
    function tryLogin() {
        var userName = $("#txtUserName").val();
        var password = $("#txtPassword").val();
        if (userName == "") {
            alert("Please Enter UserName");
        } else if (password == "") {
            alert("Please Enter Password")
        } else {
            var userCredentials = {
                userName: userName,
                password: password
            }
        }
        AJAX("Login/Login", "POST", userCredentials, "alert", true);
    }

    //Add New User
    function addNewUser() {
        var userName = $("#userEID").val();
        var password = $("#userPassword").val();
        if (userName == "") {
            alert("Please Enter UserName");
        } else if (password == "") {
            alert("Please Enter Password")
        } else {
            var userCredentials = {
                userName : userName,
                password : password
            }
        }
        AJAX("Login/AddNewUser", "POST", userCredentials, "alert", true);
    }

    //Add New Project Details
    function projectDetails() {
        var ext = $('#requirementDoc').val().split('.').pop().toLowerCase();
        if ($.inArray(ext, ['doc', 'docx']) == -1) {
            alert('invalid extension!');
        } else {
            var projectName = $("#txtProjectName").val();
            var adminPwd = $("#adminPassword").val();
            //var data = new FormData();
            //data.append("file", $('#requirementDoc').get(0).files);
            //data.append("projectName", projectName);
            //data.append("adminPwd", adminPwd);
            var data = new FormData($('#requirementDoc')[0]);
            if (projectName == "") {
                alert("Please Enter Project Name");
            } else if (adminPwd == "") {
                alert("Please Enter Admin Password");
            } else {
                $.ajax({
                    url: "addProjectDetails",
                    type: "POST",
                    enctype: 'multipart/form-data',
                    contentType: false,
                    processData: false,
                    cache: false,
                    data: data,
                    success: function () {

                    },
                    complete: function () {

                    },
                    error: function () {

                    }
                });
            }
        }
    }

    var selectors = "#btnaddNewUser, #newProjectDetails, #btnLogin";
    $(selectors).click(function () {
        switch ($(this).attr('id')) {
            case "btnaddNewUser":
                addNewUser();
                break;
            case "newProjectDetails":
                projectDetails();
                break;
            case "btnLogin":
                tryLogin();
                break;
        }
    })
});