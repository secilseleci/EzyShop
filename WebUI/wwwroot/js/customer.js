let customersTable;




$(document).ready(function () {
    customersTable = $('#tblCustomers').DataTable({
        processing: true,
        serverSide: true,
        ajax: {
            url: '/Admin/Customer/GetPaginatedCustomers',
            type: 'GET',
        },
        columns: [
            { title: "Name", data: 'fullName', width: "25%" }, 
            { title: "Email", data: 'email', width: "25%" },
            { title: "Phone", data: 'phoneNumber', width: "25%" },
            {
                title: "Actions",
                data: 'id',
                width: "25%",
                render: function (data) {
                    return `<button onclick="deleteUser('${data}')" class="btn btn-danger btn-sm">Delete</button>`;
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



