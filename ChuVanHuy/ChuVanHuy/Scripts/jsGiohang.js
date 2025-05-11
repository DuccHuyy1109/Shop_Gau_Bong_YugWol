$(document).ready(function () {
    ShowCount();
    $('body').on('click', '.btnAddToCart', function (e) {
        e.preventDefault();
        var id = $(this).data('id');
        var quatity = 1;
        var tQuantity = $('#quantity_value').text();
        if (tQuantity != '') {
            quatity = parseInt(tQuantity);
        }
        

        alert(id + "  " + quatity);


        $.ajax({
            url: '/giohangcart/addtocart',
            type: 'POST',
            data: { id: id, quantity: quatity },
            success: function (rs) {
                if (rs.Success) {
                    $('#checkout_items').html(rs.Count)
                    alert(rs.msg);
                }
            }
        });
    });

    $('body').on('click', '.btnDeleteAll', function (e) {
        e.preventDefault();
        var id = $(this).data('id');
        var conf = confirm('Bạn muốn soá toàn bộ game khỏi giỏ hàng ?');
        if (conf == true) {
            DeleteAll();
        }
    });

    $('body').on('click', '.btnDelete', function (e) {
        e.preventDefault();
        var id = $(this).data('id');
        var conf = confirm('Bạn muốn soá game khỏi giỏ hàng ?');
        if (conf == true) {
            $.ajax({
                url: '/giohangcart/Delete',
                type: 'POST',
                data: { id: id },
                success: function (rs) {
                    if (rs.Success) {
                        $('#checkout_items').html(rs.Count)
                        $('#trow_' + id).remove();
                        LoadCart();
                    }
                }
            });
        }
        
    });
});

function ShowCount() {
    $.ajax({
        url: '/giohangcart/ShowCount',
        type: 'GET',
        success: function (rs) {
            $('#checkout_items').html(rs.Count)
        }
    });
}

function DeleteAll() {
    $.ajax({
        url: '/giohangcart/DeleteAll',
        type: 'POST',
        success: function (rs) {
            if (rs.Success) {
                LoadCart();
            }
        }
    });
}


function LoadCart() {
    $.ajax({
        url: '/giohangcart/Partial_Item_Cart',
        type: 'GET',
        success: function (rs) {
            $('#load_data').html(rs)
        }
    });
}
