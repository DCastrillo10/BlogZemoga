var datatable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    datatable = $('#tblData').DataTable({
        "ajax": {
            "url": "/Post/GetCommentsById"
        },
        "columns": [
            { "data": "id", "width": "5%" },
            { "data": "postId", "width": "5%" },
            { "data": "comments", "width": "20%" },
            { "data": "status", "width": "10%" },
            { "data": "registerDate", "width": "15%" }
        ]
    });    
}