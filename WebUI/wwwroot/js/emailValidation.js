$(document).ready(function () {
    $("#emailInput").on("blur", function () {
        let email = $(this).val().trim(); // Boşlukları engelle
        $(".email-error").remove(); // Önceki hata mesajlarını temizle

        if (email) {
            $.ajax({
                url: "/Application/CheckEmailAvailability",
                type: "GET",
                data: { email: email },
                success: function (response) {
                    if (!response.available) {
                        $("#emailInput").addClass("is-invalid");
                        $("#emailInput").after(`<div class="text-danger email-error">${response.message}</div>`);
                    } else {
                        $("#emailInput").removeClass("is-invalid");
                    }
                }
            });
        }
    });
});
