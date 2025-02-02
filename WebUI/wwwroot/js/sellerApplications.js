$(document).ready(function () {
    $(".filter-btn").on("click", function () {
        var status = $(this).data("status");
        loadApplications(status);
    });
});

function loadApplications(status) {
    $.ajax({
        url: "/Application/GetSellerApplications?status=" + status,
        type: "GET",
        success: function (data) {
            $("#sellerApplicationsTable").html(data);
        },
        error: function () {
            toastr.error("Başvurular yüklenirken hata oluştu!");
        }
    });
}
