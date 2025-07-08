$(document).ready(function () {
    loadCategories();
    applyFilter();
   
    $("#filterButton").on("click", function () {
        applyFilter();
    });

    $("#resetButton").on("click", function () {
        resetFilters();
    });
});

function applyFilter(page = 1) {
    let model = {
        name: $("#name").val(),
        category: $("#category").val(),
        color: $("#color").val(),
        minPrice: $("#minPrice").val(),
        maxPrice: $("#maxPrice").val(),
        page: page,
        pageSize: 12
    };

    $.ajax({
        url: "/api/products/filter",
        type: "POST",
        data: model,
        contentType: "application/x-www-form-urlencoded",
        success: function (response) {
            
            if (response.success && response.data.length > 0) {
                reloadProductCard(response.data);
                renderPagination(response.totalItems, response.currentPage);
            } else {
                showNoProductsMessage();
                renderPagination(0, 0);
            }
        },
        error: function (xhr) {
             showNoProductsMessage();
        }
    });
}

function reloadProductCard(products) {
    const container = $("#productCardContainer");
    container.empty();

    if (products.length === 0) {
        container.html('<div class="alert alert-warning">No products found matching your criteria.</div>');
        return;
    }

    let html = '<div class="row">';
    products.forEach(p => {
        html += `
            <div class="col-md-4 mb-4">
                <div class="card border rounded shadow-sm">
                    <img src="${p.imageUrl}" class="card-img-top" />
                    <div class="card-body">
                        <h5 class="card-title text-center">${p.name}</h5>
                        <p class="text-center text-muted">${p.categoryName}</p>
                        <p class="text-center fw-bold">${p.price} ₺</p>
                        <a href="/Product/Details?productid=${p.productId}" class="btn btn-outline-info form-control">Details</a>
                    </div>
                </div>
            </div>`;
    });
    html += '</div>';

    container.html(html);
   
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

function renderPagination(totalPages, currentPage) {
    const container = $("#productCardContainer");
    const paginationContainerId = "paginationContainer";

    // Varsa önce temizle
    $(`#${paginationContainerId}`).remove();

    if (totalPages <= 1) return;

    let paginationHtml = `<div id="${paginationContainerId}" class="d-flex justify-content-center mt-4"><nav><ul class="pagination">`;

    for (let i = 1; i <= totalPages; i++) {
        let activeClass = i === currentPage ? "active" : "";
        paginationHtml += `
            <li class="page-item ${activeClass}">
                <button class="page-link" onclick="applyFilter(${i})">${i}</button>
            </li>`;
    }

    paginationHtml += `</ul></nav></div>`;

    container.append(paginationHtml);
}

function resetFilters() {
    $("#name").val("");
    $("#category").prop('selectedIndex', 0);
    $("#color").val("");
    $("#minPrice").val("");
    $("#maxPrice").val("");

    applyFilter();
}

function loadCategories() {
    $.ajax({
        url: "/api/products/categories",
        type: "GET",
        success: function (response) {
            if (response.success && response.data.length > 0) {
                const categoryDropdown = $("#category");
                categoryDropdown.empty();
                categoryDropdown.append(`<option value="">All Categories</option>`);
                response.data.forEach(cat => {
                    categoryDropdown.append(`<option value="${cat.name}">${cat.name}</option>`);
                });
            }
        },
        error: function (xhr) {
            console.error("❌ Category Load Error:", xhr.status, xhr.statusText);
        }
    });
}
