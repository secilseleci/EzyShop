$(document).ready(function () {
    $(".filter-btn").on("click", function () {
        $(".filter-btn").removeClass("active");
        $(this).addClass("active");
        var status = $(this).data("status");
        loadApplications(status);
    });
});

function loadApplications(status) {
    $.ajax({
        url: "/Admin/GetSellerApplications?status=" + status,
        type: "GET",
        success: function (data) {
            $("#sellerApplicationsTable").html(data);
        },
        error: function () {
            toastr.error("Başvurular yüklenirken hata oluştu!");
            $("#sellerApplicationsTable").html('<div class="alert alert-danger text-center">Başvurular yüklenemedi!</div>');

        }
    });
}
$(document).on("submit", ".approve-form, .reject-form", function (e) {
    e.preventDefault(); // Sayfanın yenilenmesini engelle
    var form = $(this);
    var actionUrl = form.attr("action");
    var formData = form.serialize();

    Swal.fire({
        title: 'Are you sure?',
        text: "Do you want to approve/reject this application?",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, proceed!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: actionUrl,
                type: "POST",
                data: formData,
                success: function (response) {
                    console.log(response);
                    if (response.success) {
                        toastr.success(response.message);
                        loadApplications("all"); // Başvuruları güncelle
                    } else {
                        toastr.error(response.message);
                    }
                },
                error: function () {
                    toastr.error("An error occurred while updating the application.");
                }
            });
        }
    });
});
