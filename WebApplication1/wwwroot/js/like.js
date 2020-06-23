"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/likeHub").build();

//Disable send button until connection is established
//document.getElementById("sendButton").disabled = true;
/*connection.on("SendLike", function (val) {
    var z = document.getElementById("numberLike").textContent;
    var num = parseInt(z);
    num++;
    document.getElementById("numberLike").textContent = num;
});*/

connection.on("SendLike", function (val) {
    var z = document.getElementById(val).firstElementChild.textContent;
    var num = parseInt(z);
    num++;
    document.getElementById(val).firstElementChild.textContent = num;
});

connection.on("DisLike", function (val) {
    var z = document.getElementById(val).firstElementChild.textContent;
    var num = parseInt(z);
    num--;
    document.getElementById(val).firstElementChild.textContent = num;   
});

connection.start().then(function () {
    //document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

var buttons = document.getElementsByClassName("btn btn-info btn-rounded btn-labeled test");
for (var i = 0; i < buttons.length; i++) {
    buttons[i].addEventListener("click", function (event) {
        var user = event.target.id;
        var email = event.target.name;
        var type = event.target.lastChild.textContent.trim();
        if (type == "Like") {
            connection.invoke("SendLike", user, email).catch(function (err) {
                return console.error(err.toString());
            });
            document.getElementById(user).lastChild.textContent = "DisLike";
        }
        else {
            connection.invoke("DisLike", user,email).catch(function (err) {
                return console.error(err.toString());
            });
            document.getElementById(user).lastChild.textContent = "Like";
        }
        
        event.preventDefault();
    });
};
/*document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = document.getElementById("userName").value;
    connection.invoke("SendLike", user).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});*/