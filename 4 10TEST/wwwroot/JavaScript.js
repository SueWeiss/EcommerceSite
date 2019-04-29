$(() => {
    $('.addCart').on('click', function () {
        const amount = $('#amount').val()
        const productId = $(".addCart").val()


        $.post("/home/addToCart", { amount: amount, ProductId: productId }, function (string) {
            $('.modal-title').text(`${string} Added to your cart`)
            $('#cart-modal').modal();
        })
    })

    $('#cancel-button').on('click', function () {
        $('#cart-modal').modal('hide');
    })



    $('.CategoryName').on('click', function () {
        $('.products').remove();
        const catId = $(this).data('id');
        $.post("/home/JasonProducts", { Id: catId }, function (products) {
            products.forEach(product =>
                $('.myRow').append(`  <div class="col-sm-4 col-lg-4 col-md-4 products">
                    <div class="thumbnail">
                        <img src="/ProductImages/${product.imageName}" style="width: 800px;" />
                        <div class="caption">
                            <h4 class="pull-right">${product.price}</h4>
                            <h4>
                                <a href="/Home/products?id=${product.productId}">${product.name}</a>
                            </h4>
                            <p>${product.description}</p>
                        </div>
                    </div>
                </div>`))

        })
    })
})

    //$('.delete').on('click', function () {
    //    alert("foo")
    //    const Id = $(this).data('id');
    //    $.post("/home/deletefromcart", { ItemId: Id }, function () { }
    //    )
    //}
    //)



