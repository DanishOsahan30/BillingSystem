$(document).ready(function () {
    // Plus Button Click
    $(document).on('click', '.plus', function () {
        console.log("Clicking plus")
        var row = $(this).closest('tr');
        var input = row.find('.quantity-input');
        var newValue = parseInt(input.val()) + 1;
        input.val(newValue);
        updateTotalPrice();
    });

    // Minus Button Click
    $(document).on('click', '.minus', function () {
        console.log("Clicking minus")
        var row = $(this).closest('tr');
        var input = row.find('.quantity-input');
        var newValue = parseInt(input.val()) - 1;
        if (newValue < 0) newValue = 0;
        input.val(newValue);
        updateTotalPrice();
    });

    $('#customer_dropdown').change(function () {
        var selectedCustomer = $(this).val();
        $('#customer_name').val(selectedCustomer);
    });
    $('#submit-btn').click(function () {
        // Check if any product is selected
        var totalUniqueProducts = parseInt($('#total-unique-products').text());
        if (totalUniqueProducts === 0) {
            alert('Please select at least one product.');
            return false; // Prevent form submission
        }
    });
    function updateTotalPrice() {
        var totalPrice = 0;
        var totalUniqueProducts = 0;
        var totalQuantity = 0;
        var selectedProducts = {}; // Object to store unique product names

        $('#product-table tbody tr').each(function () {
            var row = $(this);
            var quantity = parseInt(row.find('.quantity-input').val());
            var productName = row.find('td:eq(0)').text(); // Assuming the product name is in the first column
            var productPrice = parseFloat(row.find('td:eq(2)').text());

            if (quantity > 0) {
                totalQuantity += quantity;
                totalPrice += parseFloat(row.find('td:eq(2)').text()) * quantity; // Assuming the price is in the third column
                selectedProducts[productName] = { name: productName.trim(), price: parseInt(productPrice), quantity: quantity }; // Store the product name to count unique products
            }
        });

        // Count the unique products
        totalUniqueProducts = Object.keys(selectedProducts).length;
        var selectedProductsArray = Object.values(selectedProducts);

        $('#selected-products').val(JSON.stringify(selectedProductsArray));

        var taxPercentage = parseInt($('#tax').val());
        var discountPercentage = parseInt($('#discount').val());

        var grandTotal = calculateGrandTotal(totalPrice, taxPercentage, discountPercentage);

        $('#total').text(totalPrice.toFixed(2));
        $('#total-unique-products').text(totalUniqueProducts);
        $('#total-quantity').text(totalQuantity);
        $('#grand-total').text(grandTotal.toFixed(2));
        $('#total').val(totalPrice.toFixed(2));
        $('#grand-total').val(grandTotal.toFixed(2))

        // Adding values to input fields
        $('#totalAmount').val(parseInt(totalPrice));
        $('#productCount').val(parseInt(totalUniqueProducts));
        $('#totalQuantity').val(parseInt(totalQuantity));
        $('#total-unique-products').val(totalUniqueProducts);
        $('#total-quantity').val(totalQuantity);
    }


    function calculateGrandTotal(totalPrice, taxPercentage, discountPercentage) {
        var taxAmount = totalPrice * (taxPercentage / 100);
        var discountAmount = totalPrice * (discountPercentage / 100);
        var grandTotal = totalPrice + taxAmount - discountAmount;

        //adding values to input fields
        $('#grandTotal').val(parseInt(grandTotal));
        $('#discount_applied').val(parseInt(discountPercentage));
        $('#tax_applied').val(parseInt(taxPercentage))
        return grandTotal;
    }

    // Event listener for tax and discount inputs
    $('#tax, #discount').on('input', function () {
        updateTotalPrice();
    });
});