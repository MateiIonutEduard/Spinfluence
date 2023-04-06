$(document).ready(function () {
    $('#description').summernote();
});

var list = new Array();

function GoHome() {
    list = [];
    location.href = '/Home/';
}

function AddEvent() {
    let obj = {
        name: $('#eventName').val(),
        beginDate: $('#eventBeginDate').val(),
        endDate: $('#eventEndDate').val(),
        seats: $('#eventSeats').val()
    };

    if (obj.name !== '' && obj.beginDate !== '' && obj.endDate !== '' && obj.seats !== '') {
        // append html container to table body
        $('#body').append(`<tr><td>${obj.name}</td><td>${obj.beginDate}</td><td>${obj.endDate}</td><td>${obj.seats}</td><td><button class='btn text-danger' onclick='RemoveEvent(${list.length})'><i class='fa fa-trash' aria-hidden='true'></i></button></td></tr>`);
        list.push(obj);

        $('#eventName').val('');
        $('#eventBeginDate').val('');

        $('#eventEndDate').val('');
        $('#eventSeats').val('');
    }
}

function RemoveEvent(id) {
    if (list.length) {
        list.splice(id, 1);
        $('#body').empty();

        for (let k = 0; k < list.length; k++)
            $('#body').append(`<tr><td>${list[k].name}</td><td>${list[k].beginDate}</td><td>${list[k].endDate}</td><td>${list[k].seats}</td><td><button class='btn text-danger' onclick='RemoveEvent(${k})'><i class='fa fa-trash' aria-hidden='true'></i></button></td></tr>`);
    }
}

function AddChilds() {
    let childs = $("#body").children();
    
    if (list.length === 0) {
        for (let k = 0; k < childs.length; k++) {
            let obj = {
                name: childs[k].cells[0].innerText,
                beginDate: childs[k].cells[1].innerText,
                endDate: childs[k].cells[2].innerText,
                seats: childs[k].cells[3].innerText
            };

            list.push(obj);
        }
    }
}

function NewCompany() {

}

function EditCompany() {
    AddChilds();
    let formData = new FormData();

    let name = $('#name').val();
    let description = $('#description').summernote('code');

    formData.append('name', name);
    formData.append('description', description);
    formData.append('entries', JSON.stringify(list));

    $.ajax({
        url: '/Home/Create',
        type: 'post',
        data: formData,
        cache: false,
        contentType: false,
        processData: false,
        success: () => {
            console.log("Company was edited successfully.");
        },
        statusCode: {
            200: () => {
                setTimeout(() => {
                    location.href = "/Home/";
                }, 500);
            }
        },
        error: () => {
            console.log('Company edited successfully.');
        },
        async: true
    });
}

function RemoveItem(id) {

    AddChilds();
    if (list.length) {
        list.splice(id, 1);
        $('#body').empty();

        for (let k = 0; k < list.length; k++)
            $('#body').append(`<tr><td>${list[k].name}</td><td>${list[k].beginDate}</td><td>${list[k].endDate}</td><td>${list[k].seats}</td><td><button class='btn text-danger' onclick='RemoveItem(${k})'><i class='fa fa-trash' aria-hidden='true'></i></button></td></tr>`);
    }
}

function AddTableEvent() {
    AddChilds();

    let obj = {
        name: $('#eventName').val(),
        beginDate: $('#eventBeginDate').val(),
        endDate: $('#eventEndDate').val(),
        seats: $('#eventSeats').val()
    };

    if (obj.name !== '' && obj.beginDate !== '' && obj.endDate !== '' && obj.seats !== '') {
        // append html container to table body
        $('#body').append(`<tr><td>${obj.name}</td><td>${obj.beginDate}</td><td>${obj.endDate}</td><td>${obj.seats}</td><td><button class='btn text-danger' onclick='RemoveItem(${list.length})'><i class='fa fa-trash' aria-hidden='true'></i></button></td></tr>`);

        list.push(obj);
        console.log(list);
        $('#eventName').val('');
        $('#eventBeginDate').val('');

        $('#eventEndDate').val('');
        $('#eventSeats').val('');
    }
}

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