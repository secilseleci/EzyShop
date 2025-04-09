let applicationsTable;

$(document).ready(function () {
    applicationsTable = $('#tblApplications').DataTable({
        processing: true,
        serverSide: true,
        ajax: {
            url: '/Admin/Application/GetPaginatedApplications',
            type: 'GET',
            data: function (d) {
                d.statusFilter = $('#statusFilter').val(); // 💥 filtre burada ekleniyor
            }
        },
        columns: [
            { title: "Name", data: 'fullName', width: "25%" },
            { title: "Email", data: 'email', width: "25%" },
            { title: "Phone", data: 'contactBusinessNumber', width: "25%" },
            { title: "Shop", data: 'shopName', width: "25%" },
            { title: "Address", data: 'shopAddress', width: "25%" },
            { title: "Tax Number", data: 'taxNumber', width: "25%" },
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
            }, {
                title: "Actions",
                data: 'id',
                width: "25%",
                render: function (data) {
                    return `<button onclick="deleteApp('${data}')" class="btn btn-danger btn-sm">Delete</button>`;
         
                }
            }
        ]
    });
}); 
function deleteApp(applicationId) {
$.ajax({
    url: `/Admin/Application/Delete`,
    type: 'POST',
    data: { applicationId },
    success: function (response) {
        if (response.success) {
            toastr.success(response.message);
           applicationsTable.ajax.reload(null, false);
        } else {
            toastr.error(response.message);
        }
    },
    error: function (response) {
        toastr.error("An error occurred. Unable to delete the application.");
    }
});
 
 
} $('#statusFilter').on('change', function () {
    applicationsTable.ajax.reload();
});
