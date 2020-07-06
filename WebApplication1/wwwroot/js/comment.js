"use strict";

var buttonshide = document.getElementsByClassName("btn btn-sm btn-default hiden");
for (var i = 0; i < buttonshide.length; i++) {

    buttonshide[i].addEventListener("click", function (event) {
        
        var id = event.target.name;

        document.getElementById(id).style.display = "block";

        event.preventDefault();
    });

};