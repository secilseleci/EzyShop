$(document).ready(function () {
    $('#tblUsers').DataTable({
        ajax: {
            url: '/Admin/Users/GetPaginatedUsers',
            type: 'GET',
        },
        columns: [
            { data: 'name', "width": "20%",},
            
            { data: 'email', width: '25%'   },
         
            { data: 'phoneNumber', width: '25%' },

            { data: 'id',width: '20%',
              render: function (data)
                {
                    return `<button onclick="deleteUser('${data}')" class="btn btn-danger btn-sm">Delete</button> `;
                }
            }
        ]
    });
});


function deleteUser(userId) {
    $.ajax({
        url: `Admin/Users/DeleteUser`,
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



