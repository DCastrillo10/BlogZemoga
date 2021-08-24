var datatable;

$(document).ready(function () {
    loadDataTable();
    var id = document.getElementById("postId");
    if (id.value > 0) {
        $('#myModal').modal('show');
    }
});

function limpiar() {
    var postId = document.getElementById("postId");
    var postsPost = document.getElementById("postsPost");
    var postUserId = document.getElementById("postUserId");
    var postRegisterDate = document.getElementById("postRegisterDate");

    postId.value = 0;
    postsPost.value = "";
    postUserId.value = "";
    postRegisterDate.value = "";
}


function loadDataTable() {
    datatable = $('#tblData').DataTable({
        "ajax": {
            "url":"/Post/GetPostByUser"    
        },
        "columns": [
            { "data": "id", "width": "10%" },
            { "data": "posts", "width":"20%" },
            { "data": "applicationUser.userName", "width": "10%" },
            { "data": "status", "width": "10%" },
            { "data": "registerDate", "width": "20%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                            <div>
                                <a href="/Post/MyPost/${data}" class="btn btn-success text-white" style="cursor:pointer;">
                                Edit
                                </a>
                            </div>
                           `;
                },"width":"5%"
            }
        ]

    });
}