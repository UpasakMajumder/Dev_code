function brandChange(thiselem) {
    var brandId = $(thiselem).val() != 0 ? $(thiselem).val() : '';
    var year = $('[id$=ddlYear]').val() != 0 ? $('[id$=ddlYear]').val().toString().substr(-2) : '';
    var posNumber = brandId + year + $('[id$=txtPOSCode]').val();
    if (posNumber.length <= 8) {
      $('[id$=txtPOSNumber]').val(posNumber);
    }
  }
  function yearChange(thiselem) {
    var brandId = $('[id$=ddlBrand]').val() != 0 ? $('[id$=ddlBrand]').val() : '';
    var year = $(thiselem).val() != 0 ? $(thiselem).val().toString().substr(-2) : '';
    var posNumber = brandId + year + $('[id$=txtPOSCode]').val();
    if (posNumber.length <= 8) {
      $('[id$=txtPOSNumber]').val(posNumber);
    }
  }
  function poscodeChange(thiselem) {
    var brandId = $('[id$=ddlBrand]').val() != 0 ? $('[id$=ddlBrand]').val() : '';
    var year = $('[id$=ddlYear]').val() != 0 ? $('[id$=ddlYear]').val().toString().substr(-2) : '';
    var posNumber = brandId + year + $(thiselem).val();
    if (posNumber.length <= 8) {
      $('[id$=txtPOSNumber]').val(posNumber);
    }
  }
//Calendar js
 $(function () {
    var datepickerpath='{% GetResourceString("Kadena.ProductFrom.DatePickerPath") |(user)cvoqa2@gmail.com|(hash)1d8a6996cfbb69221c225e60734d8f11773095a0e5fc7f9c9ade2a0fc338b985%}';
   alert(datepickerpath);
        $('[id$=txtExpDate]').datepicker({
            showOn: "both",
            buttonImage: datepickerpath,
            buttonImageOnly: false
        });
    });

    $('[id$=productImage]').on('change', function () {
        $('.product-img').hide();
    });
//end
//Product Campaign js https://png.icons8.com/?id=3344&size=280
  $(function () {
    var imagepath='{% GetResourceString("Kadena.ProductFrom.CalImagePath") |(user)cvoqa2@gmail.com|(hash)d46a5fe5aa1c79d0439eff5364becbb721ef5a1e38340b94a69d7226be80cb38%}';
        $('[id$=txtExpireDate]').datepicker({
            showOn: "both",
            buttonImage: imagepath,
            buttonImageOnly: false
        });
    });

    $('[id$=productImage]').on('change', function () {
        $('.product-img').hide();
    });
//end