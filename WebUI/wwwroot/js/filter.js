$(document).ready(function () {
    $("#filterButton").on("click", function () {
        applyFilter();
    });
    $("#resetButton").on("click", function () {
        resetFilters();
    });
});

function applyFilter() {
    let name = $("#name").val();
    let category = $("#category").find(":selected").val();
    let color = $("#color").val();
    let minPrice = $("#minPrice").val();
    let maxPrice = $("#maxPrice").val();


    $.ajax({
        url: "/product/getfilteredproducts",
        type: "GET",
        data: { name, category, color, minPrice, maxPrice },
        success: function (response) {
            console.log("✅ Gelen Ürünler:", response.data);

            if (response.success && response.data.length > 0) {
                reloadProductCard(response.data, name, category, color, minPrice, maxPrice);
            } else {
                showNoProductsMessage();
            }
        },
        error: function (xhr) {
            console.error("❌ AJAX ERROR:", xhr.responseText);
            showNoProductsMessage();
        }
    });
}

function reloadProductCard(products, name, category, color, minPrice, maxPrice) {

    $.ajax({
        url: "/home/renderproductcard",
        type: "GET",
        data: { name, category, color, minPrice, maxPrice },
        success: function (data) {
            let container = $("#productCardContainer");

            if (container.length === 0) {
                return;
            }

            container.empty();
            container.html(data);
        },
        error: function (xhr, status, error) {
            console.error("❌ AJAX ERROR:", xhr.responseText);
        }
    });
}

function showNoProductsMessage() {
    let container = $("#productCardContainer");
    container.empty();
    if ($(".alert-warning").length === 0) {  
        container.append(`
            <div class="row">
                <div class="col-12 text-center">
                    <div class="alert alert-warning">
                        No products found matching your criteria.
                    </div>
                </div>
            </div>
        `);
    }
}
function resetFilters() {
    $("#name").val("");
    $("#category").prop('selectedIndex', 0);
    $("#color").val("");
    $("#minPrice").val("");
    $("#maxPrice").val("");

    applyFilter();
}