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

    $.ajax({
        url: actionUrl,
        type: "POST",
        data: formData,
        success: function (response) {
            toastr.success("Application updated successfully!");
            loadApplications("pending"); // AJAX çağrısından sonra güncelle
        },
        error: function () {
            toastr.error("An error occurred while updating the application.");
        }
    });
});
