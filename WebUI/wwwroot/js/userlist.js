$(document).ready(function () {
    $('#tblUsers').DataTable({
        ajax: {
            url: '/Admin/GetAllUsers',
            type: 'GET',
            datatype: 'json'
        },
        columns: [
            {
                data: 'name',
                "width": "20%",
                render: function (data, type, row) {
                    if (row.roles.includes('Admin')) {
                        return `<div class="w-100" role="group">
                                <span class="btn btn-success disabled">${data}</span>
                                </div>`
                    } else {
                        return data;
                    }
                } },
            {
                data: 'email',
                width: '25%' 
            },
            {
                data: 'roles',
                width: '15%',
                render: function (data) {
                    return data.map(role => `<span class="badge bg-info">${role}</span>`).join(' ');
                }
            },
            {
                data: 'id',
                width: '20%',
                render: function (data) {
                    return `
                        <div class="btn-group">
                            <button onclick="resetPassword('${data}')" class="btn btn-warning btn-sm">Reset Password</button>
                            <button onclick="deleteUser('${data}')" class="btn btn-danger btn-sm">Delete</button>
                        </div>
                    `;
                }
            }
        ]
    });
});

function resetPassword(userId) {
    $.ajax({
        url: `/Admin/ResetUserPassword`,
        type: 'POST',
        data: { userId },   
        success: function (response) {
            if (response.success) {
                toastr.success("Password successfully reset. An email has been sent.");
            } else {
                toastr.error(response.message || "Failed to reset password. Please try again.");
            }
        },
        error: function (error) {
            toastr.error("An error occurred while resetting the password.");
            console.error("Error:", error);
        }
    });
}

function deleteUser(userId) {
    $.ajax({
        url: `/Admin/DeleteUser`,
        type: 'POST',
        data: { userId },
        success: function (response) {
            if (response.success) {
                 toastr.success("User successfully deleted.");
                 $('#tblUsers').DataTable().ajax.reload();
            } else {
                toastr.error("Failed to delete the user. Please try again.");
            }
        },
        error: function (error) {
            toastr.error("An error occurred. Unable to delete the user.");
            console.error("Error:", error);
        }
    });
}


 
