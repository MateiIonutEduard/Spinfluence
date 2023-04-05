function AddPractice(CompanyEventId) {
    let practiceId = document.getElementById("CompanyEventId");
    practiceId.value = CompanyEventId;
}

function RemoveCompany(id) {
    let token = document.cookie.substring(6);

    $.ajax({
        url: `/Home/RemoveCompany/?id=${id}`,
        type: 'delete',
        beforeSend: function (xhr) {
            xhr.setRequestHeader("Authorization", `Bearer ${token}`);
        },
        success: (data, status) => {
            console.log('Company was removed successfully.');
        },
        statusCode: {
            404: function () {
                location.href = "/Home/";
            },
            200: function () {
                location.href = "/Home/";
            },
            403: function () {
                $('#warningModal').modal({ show: true });
            },
            401: function () {
                location.href = "/Account/Login/";
            }
        },
        error: (e) => {
            console.log(e);
        },
        async: true
    });
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
            console.log("Student practice was canceled.");
        },
        statusCode: {
            404: function () {
                location.href = "/Home/";
            },
            200: function () {
                location.href = "/Practice/";
            },
            401: function () {
                location.href = "/Account/Login/";
            }
        },
        error: (e) => {
            console.log(e);
        },
        async: true
    });
}