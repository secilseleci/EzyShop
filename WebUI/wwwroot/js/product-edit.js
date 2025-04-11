

//SnapShotla update için değişiklik kontrolü
let originalData;
$(document).ready(function () {
    originalData = {
        name: $('#Name').val(),
        color: $('#Color').val(),
        price: $('#Price').val(),
        stock: $('#Stock').val(),
        categoryId: $('#CategoryId').val(),
        isActive: $('#IsActive').is(':checked')
    };

     $('form').on('submit', function (e) {
        const currentData = {
            name: $('#Name').val(),
            color: $('#Color').val(),
            price: $('#Price').val(),
            stock: $('#Stock').val(),
            categoryId: $('#CategoryId').val(),
            isActive: $('#IsActive').is(':checked')
        };

         const changed = Object.keys(originalData).some(key => originalData[key] != currentData[key]);

        if (!changed) {
            e.preventDefault();  
            toastr.warning("No changes detected. Nothing to update.");
        } else {
            // Görsel olarak geri bildirim (isteğe bağlı)
            $('#btnSubmit').prop('disabled', true);
            $('#btnSpinner').removeClass('d-none');
            $('#btnText').text('Updating...');
        }
    });
});
//SnapShotla update için değişiklik kontrolü

 