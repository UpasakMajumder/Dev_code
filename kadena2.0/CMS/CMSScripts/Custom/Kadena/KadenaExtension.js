

$(document).ready(function () {
  var brand = $('[id$=ddlBrand]').val();
  var year = $('[id$=ddlYear]').val();
  var POScategory = $('[id$=ddlCategory]').val();
  var POScode = $('[id$=txtPOSCode]').val();
  var POSNumber = $('[id$=txtPOSNumber]').val();
  
  $('[id$=ddlBrand]').change(function () {
    var brandId = $('[id$=ddlBrand]').val() != 0 ? $('[id$=ddlBrand]').val() : '';
    var year = $('[id$=ddlYear]').val() != 0 ? $('[id$=ddlYear]').val().toString().substr(-2) : '';
    var pos_number = brandId + year + $('[id$=txtPOSCode]').val();
    if (pos_number.length <= 8) {
      $('[id$=txtPOSNumber]').val(pos_number);
    }
  });
  $('[id$=ddlYear]').change(function () {
    var brandId = $('[id$=ddlBrand]').val() != 0 ? $('[id$=ddlBrand]').val() : '';
    var year = $('[id$=ddlYear]').val() != 0 ? $('[id$=ddlYear]').val().toString().substr(-2) : '';
    var pos_number = brandId + year + $('[id$=txtPOSCode]').val();
    if (pos_number.length <= 8) {
      $('[id$=txtPOSNumber]').val(pos_number);
    }
  });
  $('[id$=txtPOSCode]').change(function () {
    var brandId = $('[id$=ddlBrand]').val() != 0 ? $('[id$=ddlBrand]').val() : '';
    var year = $('[id$=ddlYear]').val() != 0 ? $('[id$=ddlYear]').val().toString().substr(-2) : '';
    var pos_number = brandId + year + $('[id$=txtPOSCode]').val();
    if (pos_number.length <= 8) {
      $('[id$=txtPOSNumber]').val(pos_number);
    }
  });
});

//Calendar js
 $(function () {
        $('[id$=txtExpDate]').datepicker({
            showOn: "both",
            buttonImage: "https://png.icons8.com/?id=3344&size=280",
            buttonImageOnly: false
        });
    });

    $('[id$=productImage]').on('change', function () {
        $('.product-img').hide();
    });