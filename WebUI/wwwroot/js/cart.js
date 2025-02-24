function updateCartCount() {
    $.ajax({
        url: "/ShoppingCart/GetCartItemCount",
        type: "GET",
        success: function (response) {
            if (response.success) {
                $("#cartItemCount").text(response.count);
            }
        },
        error: function () {
            console.error("Cart item count update failed.");
        }
    });
}

function addToCart(productId) {
    let count = parseInt($("#productCount").val());
    if (count < 1 || count > 100) {
        toastr.error("The number can be minimum 1 and maximum 100.");
        return;
    }
    $.ajax({
        url: "/ShoppingCart/AddToCart",
        type: "POST",
        data: { productId: productId, count: count },
        success: function (response) {
            if (response.success) {
                toastr.success(response.message);
                updateCartCount();  // 🟢 Sepet sayısını güncelle
            } else {
                toastr.error(response.message);
            }
        },
        error: function () {
            toastr.error("Error occurred while adding the product to the cart.");
        }
    });
}

function updateQuantity(itemId, action) {
    $.ajax({
        url: "/ShoppingCart/UpdateQuantity",
        type: "POST",
        data: { itemId: itemId, action: action },
        success: function (response) {
            if (response.redirect) {
                window.location.href = response.redirect;  
                return;
            }
            if (response.success) {
                location.reload();
                updateCartCount();
            } else {
                toastr.error(response.message);
            }
        },
        error: function () {
            toastr.error("Error updating quantity.");
        }
    });
}


function removeItem(itemId) {
    $.ajax({
        url: "/ShoppingCart/RemoveItem",
        type: "POST",
        data: { itemId: itemId },
        success: function () {
            location.reload();
            updateCartCount(); // 🟢 Sepet sayısını güncelle
        },
        error: function () {
            toastr.error("Error removing item.");
        }
    });
}

// Sayfa yüklendiğinde sepet sayısını güncelle
$(document).ready(function () {
    updateCartCount();
});
