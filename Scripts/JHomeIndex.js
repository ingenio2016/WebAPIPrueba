tinymce.init({
    selector: 'textarea',
    language: 'es',
    encoding: 'UTF-8',
    menu: {},
    toolbar: 'undo redo | alignleft aligncenter alignright',
    setup: function (ed) {
        var messagesCount = 0;
        ed.on('keyup', function (e) {
            var count = CountCharacters();
            messagesCount = 1 + parseInt(count / 161);
            document.getElementById("character_count").innerHTML = "Caracteres: " + count;
            document.getElementById("messages_count").innerHTML = "Mensajes: " + messagesCount;
        });
    }
});

function CountCharacters() {
    var body = tinymce.get("Message").getBody();
    var content = tinymce.trim(body.innerText || body.textContent);
    return content.length;
};