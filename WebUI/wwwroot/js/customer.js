let customersTable;

$(document).ready(function () {
    customersTable = $('#tblCustomers').DataTable({
        processing: true,
        serverSide: true,
        ajax: { url: '/Admin/Customer/GetPaginatedCustomers',type: 'GET'},
        columns: [
            { title: "Name", data: 'fullName', width: "25%" }, 
            { title: "Address", data: 'address', width: "25%" },
            { title: "Phone", data: 'phone', width: "25%" },
            {
                title: "Actions",
                data: 'id',
                width: "25%",
                render: function (data) {
                    return `<div class="w-75 btn-group" role="group">
                     <a onClick=deleteUser('${data}') class="btn btn-danger mx-2 rounded"> <i class="bi bi-trash-fill"></i></a>
                    </div>`
                }
            }
        ]
    });
});

function deleteUser(customerId) {
    $.ajax({
        url: `/Admin/Customer/Delete`,
        type: 'POST',
        data: { customerId },
        success: function (response) {
            if (response.success) {
                toastr.success(response.message);
                customersTable.ajax.reload(null, false);
            } else {
                toastr.error(response.message);
            }
        },
        error: function (response) {
            toastr.error("An error occurred. Unable to delete the user.");
        }
    });
}



