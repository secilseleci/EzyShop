let categoriesTable;

$(document).ready(function () {
    categoriesTable = $('#tblCategories').DataTable({
        processing: true,
        serverSide: true,
        ajax: {url: '/Admin/Category/GetAllCategories',type: 'GET'},
        columns: [
            {
                title: "Image",
                data: 'imageUrl',
                "width": "15%",
                "render": function (data) {
                    return `<div>
                     <img src="${data}" style="width:100%; border-radius:5px; border:1px solid #ffffff" />
                    </div>`;
                }
            },
            { title: "Name", data: 'name', "width": "15%" },
            {
                title: "Actions",
                data: 'id',
                "width": "15%",
                "render": function (data) {
                    return `<div class="w-75 btn-group" role="group">
                     <a href="/Admin/Category/Edit?id=${data}" class="btn btn-warning mx-2 rounded mx-4"> <i class="bi bi-pencil-square"></i></a>               
                     <a onClick=deleteCategory('${data}') class="btn btn-danger mx-2 rounded"> <i class="bi bi-trash-fill"></i></a>
                    </div>`
                }
            },
        ]
    });
});

function deleteCategory(categoryId) {
    $.ajax({
        url: `/Admin/Category/Delete`,
        type: 'POST',
        data: { categoryId: categoryId },
        success: function (response) {
            if (response.success) {
                toastr.success(response.message);
                categoriesTable.ajax.reload(null, false);
            } else {
                toastr.error(response.message);
            }
        },
        error: function (response) {
            toastr.error("An error occurred. Unable to delete the user.");
        }
    });
}

 

