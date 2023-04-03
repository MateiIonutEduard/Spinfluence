$(document).ready(() => {
    $(document).on("submit", "#signin", (e) => {
        e.preventDefault();

        var buffer = {
            'address': $("#address").val(),
            'password': $("#password").val()
        };

        $.ajax({
            url: '/Account/Login',
            type: 'post',
            data: buffer,
            success: data => {
                setTimeout(() => {
                    location.href = '/Home/Index';
                }, 1000);
            },
            error: () => {
                setTimeout(() => {
                    location.href = '/Account/Recover';
                }, 1000);
            },
            async: true
        });
    });
});