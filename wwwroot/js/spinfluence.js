function AddPractice(CompanyEventId) {
    let practiceId = document.getElementById("CompanyEventId");
    practiceId.value = CompanyEventId;
}

function RemovePractice(id) {
    let token = document.cookie.substring(6);

    $.ajax({
        url: `/Practice/RemovePractice/?id=${id}`,
        type: 'delete',
        beforeSend: function (xhr) {
            xhr.setRequestHeader("Authorization", `Bearer ${token}`);
        },
        success: (data, status) => {
            if (this.status == 200)
                location.href = "/Practice/";
            else if (this.status == 401)
                location.href = "/Account/Login/";
            else location.href = "/Home/";
        },
        error: () => {
            
        },
        async: true
    });
}