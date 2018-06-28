// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$('#submit-button').on('click', function (event) {

   
    event.preventDefault();

    let formData = $('#Create-Project-Form').serialize();

    

    $.ajax({
        url: $('#Create-Project-Form').attr('action'),
        type: 'POST',
        data: formData
    }).done(function (data) {
        debugger;
        window.location.href = data.redirect;
        console.log(data.title);
        debugger;

    }).fail(function () {
            debugger;
    });

});