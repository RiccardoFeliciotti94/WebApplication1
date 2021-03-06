﻿"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/likeHub").build();



connection.on("SendLike", function (val) {
    
    var z = document.getElementById(val).children.item(0).textContent;
    var num = parseInt(z);
    num++;

    document.getElementById(val).children.item(0).textContent = num;
    document.getElementById(val).setAttribute("class", "btn btn-sm btn-primary btn-labeled test");
    
});

connection.on("DisLike", function (val) {
    var z = document.getElementById(val).children.item(0).textContent;
    var num = parseInt(z);
    num--;
    document.getElementById(val).children.item(0).textContent = num
    document.getElementById(val).setAttribute("class", "btn btn-sm btn-default btn-labeled test");
});

connection.start().then(function () {
}).catch(function (err) {
    return console.error(err.toString());
});

var buttons = document.getElementsByClassName("btn btn-sm btn-default btn-labeled test");
for (var i = 0; i < buttons.length; i++) {
    buttons[i].addEventListener("click", function (event) {
        var user = event.target.id;
        var email = event.target.name;

        var type = event.target.getAttribute('ltype');
        console.log(type);
        if (type == "Like") {
            connection.invoke("SendLike", user, email).catch(function (err) {
                return console.error(err.toString());
            });
            document.getElementById(user).setAttribute('ltype', 'DisLike');
        } else {
            connection.invoke("DisLike", user, email).catch(function (err) {
                return console.error(err.toString());
            });
      
            document.getElementById(user).setAttribute('ltype', 'Like');
        }

        event.preventDefault();
    });
};

var buttonclicked = document.getElementsByClassName("btn btn-sm btn-primary btn-labeled test");
for (var i = 0; i < buttonclicked.length; i++) {
    buttonclicked[i].addEventListener("click", function (event) {
        var user = event.target.id;
        var email = event.target.name;

        var type = event.target.getAttribute('ltype');
        if (type == "Like") {
            connection.invoke("SendLike", user, email).catch(function (err) {
                return console.error(err.toString());
            });
            document.getElementById(user).setAttribute('ltype', 'DisLike');

        } else {
            connection.invoke("DisLike", user, email).catch(function (err) {
                return console.error(err.toString());
            });
            document.getElementById(user).setAttribute('ltype', 'Like');
        }


        event.preventDefault();
    });
};
