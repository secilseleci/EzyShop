$(document).ready(function () {
    handleEmailCheck();
    handleShopNameCheck();
    handleTaxNumberCheck();
    handlePhoneNumberCheck();
});

function handleEmailCheck() {
    $("#emailInput").on("blur", function () {
        let email = $(this).val().trim();
        $(".email-error").remove();

        if (email) {
            $.ajax({
                url: "/Validation/CheckEmailAvailability",
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
}

function handleShopNameCheck() {
    $("#shopNameInput").on("blur", function () {
        let shopName = $(this).val().trim();
        $(".shopName-error").remove();

        if (shopName) {
            $.ajax({
                url: "/Validation/CheckShopNameAvailability",
                type: "GET",
                data: { shopName: shopName },
                success: function (response) {
                    if (!response.available) {
                        $("#shopNameInput").addClass("is-invalid");
                        $("#shopNameInput").after(`<div class="text-danger shopName-error">${response.message}</div>`);
                    } else {
                        $("#shopNameInput").removeClass("is-invalid");
                    }
                }
            });
        }
    });
}

function handleTaxNumberCheck() {
    $("#taxNumberInput").on("blur", function () {
        let taxNumber = $(this).val().trim();
        $(".taxNumber-error").remove();

        if (taxNumber) {
            $.ajax({
                url: "/Validation/CheckTaxNumberAvailability",
                type: "GET",
                data: { taxNumber: taxNumber },
                success: function (response) {
                    if (!response.available) {
                        $("#taxNumberInput").addClass("is-invalid");
                        $("#taxNumberInput").after(`<div class="text-danger taxNumber-error">${response.message}</div>`);
                    } else {
                        $("#taxNumberInput").removeClass("is-invalid");
                    }
                }
            });
        }
    });
}


function handlePhoneNumberCheck() {
    $("#phoneInput").on("blur", function () {
        let phoneNumber = $(this).val().trim();
        $(".phone-error").remove();

        if (phoneNumber) {
            $.ajax({
                url: "/Validation/CheckPhoneNumberAvailability",
                type: "GET",
                data: { phoneNumber: phoneNumber },
                success: function (response) {
                    if (!response.available) {
                        $("#phoneInput").addClass("is-invalid");
                        $("#phoneInput").after(`<div class="text-danger phone-error">${response.message}</div>`);
                    } else {
                        $("#phoneInput").removeClass("is-invalid");
                    }
                }
            });
        }
    });
}