$(document).ready(function () {
    loadDataTable();
});


function loadDataTable() {
    dataTable = $('#tblProduct').DataTable({
        "ajax": { url: '/Product/GetSellerProducts' },
       
        "columns": [
            {
                data: 'imageUrl',
                "width": "10%",
                "render": function (data) {
                    if (!data) {
                    return
                    `<div >
                    <img src="${data}" style="width:100%; border-radius:5px; border:1px solid #ffffff; "/>
                    </div>`;
                }
            },
            { data: 'categoryName', "width": "10%" },
            { data: 'name', "width": "15%" },
            { data: 'description', "width": "10%" },
            { data: 'color', "width": "10%" },
            { data: 'price', "width": "5%" },
            { data: 'stock', "width": "5%" },
            {
                data: 'isActive',
                "width": "10%",
                "render": function (data, type, row) {
                    let btnClass = data ? "btn-success" : "btn-danger";
                    let btnText = data ? "Active" : "Inactive";
                    return `<button onClick="toggleProductStatus('${row.id}', ${data})" class="btn btn-sm ${btnClass}">
                                ${btnText}
                            </button>`;
                }
            },
            {
                data: 'id',
                "width": "10%",
                "render": function (data) {
                    return `<div class="btn-group">
                     <a href="/Product/Edit?id=${data}" class="btn btn-sm btn-warning mx-2"> <i class="bi bi-pencil-square"></i></a>               
                     <button onClick="Delete('/Product/Delete/${data}')" class="btn btn-sm btn-danger mx-2"> <i class="bi bi-trash-fill"></i></button>
                    </div>`;
                }
            }
        ]
    });
}

function Delete(url) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    if (data.success) {
                        toastr.success("Product deleted successfully!");
                        dataTable.ajax.reload();  // Listeyi yenile
                    } else {
                        toastr.error(data.message);
                    }
                },
                error: function () {
                    toastr.error("Error while deleting product");
                }
            });
        }
    });
}
function toggleProductStatus(productId, currentStatus) {
    let newStatus = !currentStatus;  
    $.ajax({
        url: `/Product/ToggleStatus/${productId}`,
        type: "POST",
        data: JSON.stringify({ isActive: newStatus }),
        contentType: "application/json",
        success: function () {
            dataTable.ajax.reload();  
        },
        error: function () {
            alert("Ürün durumu değiştirilemedi!");
        }
    });
}
