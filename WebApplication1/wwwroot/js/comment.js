"use strict";

var connectionComment = new signalR.HubConnectionBuilder().withUrl("/commentoHub").build();

connectionComment.on("SendCommento", function (data, nome, testo, imgg,id) {
    console.log("here");
    var li = document.createElement("li");
    var small = document.createElement("small");
    small.setAttribute('class', 'text-muted');
    small.textContent = data;

    var a1 = document.createElement("a");
    a1.setAttribute('href', '#');
    a1.setAttribute('class', 'text-main text-bold');
    a1.textContent = nome;

    var div1 = document.createElement("div");
    div1.setAttribute('class', 'comment-header');

    div1.appendChild(a1);
    div1.appendChild(small);

    var div2 = document.createElement("div");
    div2.setAttribute('class', 'media-body');
    var text = document.createTextNode(testo);
    div2.appendChild(div1);
    div2.appendChild(text);
    
   
    

    var img = document.createElement("img");
    img.setAttribute('alt', 'Profile Picture');
    img.setAttribute('class', 'img-circle img-xs');
    img.setAttribute('src', imgg);

    var a2 = document.createElement("a");
    a2.setAttribute('href', '#');
    a2.setAttribute('class', 'media-left');
    a2.appendChild(img);
    
    var div3 = document.createElement("div");
    div3.setAttribute('class', 'comment-content media');

    div3.appendChild(a2);
    div3.appendChild(div2);

    li.appendChild(div3);

    document.getElementById("SubCommentList "+id).appendChild(li);




});

connectionComment.start().then(function () {
}).catch(function (err) {
    return console.error(err.toString());
});

var buttonshide = document.getElementsByClassName("btn btn-sm btn-default hiden");
for (var i = 0; i < buttonshide.length; i++) {

    buttonshide[i].addEventListener("click", function (event) {
        
        var id = event.target.name;

        document.getElementById(id).style.display = "block";

        event.preventDefault();
    });

};