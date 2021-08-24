var datatable;

$(document).ready(function () {
    loadDataTable(); //Primero cargo el datateble
    //Luego si presiono el boton Add, va nuevamente al index y ejecuta el Js de nuevo, pero esta vez abre el modal
    var id = document.getElementById("postId")
    if (id.value > 0) {
        
        $("#myModal").modal('show');
    }
});


function loadDataTable() {
    datatable = $('#tblData').DataTable({
        "ajax": {
            "url":"/Post/GetAll"    
        },
        "columns": [
            { "data": "id", "width": "5%" },
            { "data": "posts", "width":"20%" },
            { "data": "applicationUser.userName", "width": "10%" },
            { "data": "approvalDate", "width": "15%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                          <div class="form-row">  
                            <div>
                                <a href="/Post/Index/${data}" class="btn btn-success text-white" style="cursor:pointer;">
                                Add
                                </a>
                            </div>
                            <div>|</div>
                            <div>
                                <a href="/Post/SeeComments/${data}" class="btn btn-success text-white" style="cursor:pointer;">
                                See
                                </a>
                            </div>
                          </div>  
                           `;
                },"width":"5%"
            }
        ]
    });
}