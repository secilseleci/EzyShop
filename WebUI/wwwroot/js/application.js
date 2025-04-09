let applicationsTable;

$(document).ready(function () {
    applicationsTable = $('#tblApplications').DataTable({
        processing: true,
        serverSide: true,
        ajax: {
            url: '/Admin/Application/GetPaginatedApplications',
            type: 'GET',
            data: function (d) {
                d.statusFilter = $('#statusFilter').val();
            }
        },
        columns: [
            { title: "Name", data: 'fullName', width: "15%" },
            { title: "Email", data: 'email', width: "15%" },
            { title: "Phone", data: 'contactBusinessNumber', width: "10%" },
            { title: "Shop", data: 'shopName', width: "15%" },
            { title: "Address", data: 'shopAddress', width: "15%" },
            { title: "Tax Number", data: 'taxNumber', width: "10%" },
            {
                title: "Status",
                data: 'statusText',
                width: "10%",
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

                    return `<span class="badge bg-${badgeClass} text-uppercase">${data}</span>`;
                }
            },
            {
                title: "Actions",
                data: null,
                width: "20%",
                render: function (data, type, row) {
                    let buttons = `<button onclick="deleteApp('${row.id}')" class="btn btn-danger btn-sm me-1">🗑</button>`;

                    if (row.status.toLowerCase() === "pending") {
                        buttons += `<button onclick="approveApp('${row.id}')" class="btn btn-success btn-sm me-1">✔</button>`;
                        buttons += `<button onclick="rejectApp('${row.id}')" class="btn btn-warning btn-sm">✖</button>`;
                    }

                    return `<div class="d-flex flex-wrap gap-1">${buttons}</div>`;
                }
            }
        ]
    });

    $('#statusFilter').on('change', function () {
        applicationsTable.ajax.reload();
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
        error: function () {
            toastr.error("An error occurred. Unable to delete the application.");
        }
    });
}

function approveApp(applicationId) {
    $.ajax({
        url: `/Admin/Application/Approve`,
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
        error: function () {
            toastr.error("An error occurred while approving the application.");
        }
    });
}

function rejectApp(applicationId) {
    $.ajax({
        url: `/Admin/Application/Reject`,
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
        error: function () {
            toastr.error("An error occurred while rejecting the application.");
        }
    });
}
