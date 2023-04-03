$(document).ready(() => {
    $(document).on("submit", "#register-form", (e) => {
        e.preventDefault();

        var buffer = {
            'address': $('#email').val()
        };

        $.ajax({
            url: '/Account/Recover',
            type: 'post',
            data: buffer,
            success: () => {
                setTimeout(() => {
                    location.href = '/Account/Login';
                }, 1000);
            },
            async: true
        });
    });
});