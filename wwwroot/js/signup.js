$(document).ready(() => {
    $(document).on("submit", "#signup", (e) => {
        e.preventDefault();
        var buffer = new FormData();
        buffer.append('username', $("#name").val());
        buffer.append('password', $("#pass").val());
        buffer.append('address', $("#address").val());

        buffer.append('logo', $('#logo').get(0).files[0]);
        buffer.append('grantType', document.getElementById("grantType").value);

        $.ajax({
            url: '/Account/Signup',
            type: 'post',
            data: buffer,
            cache: false,
            contentType: false,
            processData: false,
            success: data => {
                setTimeout(() => {
                    location.href = '/Home/Index';
                }, 1000);
            },
            async: true
        });
    });
});