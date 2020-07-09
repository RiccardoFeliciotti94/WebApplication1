"use strict";

var connectionComment = new signalR.HubConnectionBuilder().withUrl("/commentoHub").withAutomaticReconnect().build();

//Big malloppone
connectionComment.on("SendCommento", function (idCom, idMes, data, nome, email, testo, imgg) {
    console.log("here");
    var li = document.createElement("li");

    var div1lv1 = document.createElement("div");
    div1lv1.setAttribute('class', 'comments media-block');

    var img = document.createElement("img");
    img.setAttribute('alt', 'Profile Picture');
    img.setAttribute('class', 'img-circle img-sm');
    img.setAttribute('src', imgg);

    var a1 = document.createElement("a");
    a1.setAttribute('href', '#');
    a1.setAttribute('class', 'media-left');

    a1.appendChild(img);
    div1lv1.appendChild(a1);

    var div1lv2 = document.createElement("div");
    div1lv2.setAttribute('class', 'media-body');

    var div1lv3 = document.createElement("div");
    div1lv3.setAttribute('class', 'comment-header');

    var pnome = document.createElement("p");
    pnome.setAttribute('class', 'media-heading box-inline text-main text-bold');
    pnome.textContent = nome;

    var pdata = document.createElement("p");
    pdata.setAttribute('class', 'text-muted text-sm');
    pdata.textContent = data;

    div1lv3.appendChild(pnome);
    div1lv3.appendChild(pdata);
    div1lv2.appendChild(div1lv3);

    var ptesto = document.createElement("p");
    ptesto.textContent = testo;
    div1lv2.appendChild(ptesto);

    var i = document.createElement("i");
    i.setAttribute('class', 'icon-lg demo-pli-right-4');

    var a2 = document.createElement("a");
    a2.setAttribute('class', 'btn btn-sm btn-default hiden');
    a2.setAttribute('name', idCom);

    a2.appendChild(i);
    a2.appendChild(document.createTextNode('Commenta'));

    a2.addEventListener("click", function (event) {
        var id = event.target.name;
        document.getElementById(id).style.display = "block";
        event.preventDefault();
    });

    div1lv2.appendChild(a2);

    var form = document.createElement("form");
    form.style.display = "none";
    form.id = idCom;
    form.setAttribute('class', 'form-horizontal postsubcommento');
    form.setAttribute('method', 'post');
    form.setAttribute('enctype', 'multipart/form-data');

    var input1 = document.createElement("input");
    input1.setAttribute('type', 'hidden');
    input1.setAttribute('value', idMes);

    var input2 = document.createElement("input");
    input2.setAttribute('type', 'hidden');
    input2.setAttribute('value', email);

    var input3 = document.createElement("input");
    input3.setAttribute('type', 'hidden');
    input3.setAttribute('value', idCom);

    var divform1 = document.createElement("div");
    divform1.setAttribute('class', 'panel-body');

    var textarea = document.createElement("textarea");
    textarea.setAttribute('placeholder', 'Message');
    textarea.setAttribute('rows', '1');
    textarea.setAttribute('class', 'form-control');

    divform1.appendChild(textarea);

    var divform2 = document.createElement("div");
    divform2.setAttribute('class', 'panel-footer text-right');

    var butto = document.createElement("button");
    butto.setAttribute('class', 'btn btn-default');
    butto.setAttribute('type', 'button');
    butto.textContent = "Send Message";

    butto.addEventListener("click", function (event) {
        
        connectionComment.invoke("SendSubCommento", email, textarea.value, idMes, idCom).catch(function (err) {
            return console.error(err.toString());
        });

        textarea.value = "";
        form.style.display = "none";
    });
    divform2.appendChild(butto);

    form.appendChild(input1);
    form.appendChild(input2);
    form.appendChild(input3);
    form.appendChild(divform1);
    form.appendChild(divform2);

    div1lv2.appendChild(form);

    var div2lv3 = document.createElement("div");
    div2lv3.setAttribute('class', 'comment-body');

    var ul = document.createElement("ul");
    ul.id = "SubCommentList " + idCom;
    ul.style.listStyle = "none";

    div2lv3.appendChild(ul);
    div1lv2.appendChild(div2lv3);

    div1lv1.appendChild(div1lv2);

    li.appendChild(div1lv1); 
    document.getElementById("CommentoList " + idMes).appendChild(li);
  
});

connectionComment.on("SendSubCommento", function (data, nome, testo, imgg, id) {
   
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

    document.getElementById("SubCommentList " + id).appendChild(li);

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

var formPostSubCommento = document.getElementsByClassName("form-horizontal postsubcommento");
for (var i = 0; i < formPostSubCommento.length; i++) {

    formPostSubCommento[i].children[4].firstElementChild.addEventListener("click", function (event) {
        var form = event.target.parentElement.parentElement;
        var idMes = form.children[0].getAttribute('value');
        var email = form.children[1].getAttribute('value');
       
        var idref = form.children[2].getAttribute('value');
        var test = form.children[3].firstElementChild.value;

        connectionComment.invoke("SendSubCommento", email, test, idMes,idref).catch(function (err) {
            return console.error(err.toString());
        });
        form.children[3].firstElementChild.value = "";
        form.style.display = "none";
    });
};

var formPostCommento = document.getElementsByClassName("form-horizontal postcommento");
for (var i = 0; i < formPostCommento.length; i++) {

    formPostCommento[i].children[3].firstElementChild.addEventListener("click", function (event) {
        var form = event.target.parentElement.parentElement;
        var idMes = form.children[0].getAttribute('value');
        var email = form.children[1].getAttribute('value');
        var test = form.children[2].firstElementChild.value;

        connectionComment.invoke("SendCommento", email, test, idMes).catch(function (err) {
            return console.error(err.toString());
        });
        form.children[2].firstElementChild.value = "";
    });
};