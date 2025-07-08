let sellersTable;

 
$(document).ready(function () {
    sellersTable = $('#tblSellers').DataTable({
        processing: true,
        serverSide: true,
        ajax: {
            url: '/Admin/Seller/GetPaginatedSellers',
            type: 'GET',
        },
        columns: [
            { title: "Name", data: 'fullName', width: "30%" }, 
            { title: "Email", data: 'email', width: "20%" },
            { title: "Shop", data: 'shopName', width: "25%" },
            { title: "Phone", data: 'phoneNumber', width: "30%" },
            { title: "TaxNumber", data: 'taxNumber', width: "15%" },
            {
                title: "Status",
                data: 'statusText',
                width: "15%",
                render: function (data, type, row) {
                    let badgeClass = "";

                    switch (data.toLowerCase()) {
                        case "approved":
                            badgeClass = "success";  
                            break;
                        case "rejected":
                            badgeClass = "danger";  
                            break;
                        case "pending":
                            badgeClass = "warning";  
                            break;
                        default:
                            badgeClass = "secondary";  
                    }

                    return `<span class="badge bg-${badgeClass}">${data}</span>`;
                }
            },

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



function deleteUser(sellerId) {
    $.ajax({
        url: `/Admin/Seller/Delete`,
        type: 'POST',
        data: { sellerId },
        success: function (response) {
            if (response.success) {
                toastr.success(response.message);
                sellersTable.ajax.reload(null, false);
            } else {
                toastr.error(response.message);
            }
        },
        error: function (response) {
            toastr.error("An error occurred. Unable to delete the user.");
        }
    });
}



