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
      $('[id$=txtPOSNumber]').val(posNumber);
}
function poscodeChange(thiselem) {
  var brandId = $('[id$=ddlBrand]').val() != 0 ? $('[id$=ddlBrand]').val() : '';
  var year = $('[id$=ddlYear]').val() != 0 ? $('[id$=ddlYear]').val().toString().substr(-2) : '';
  var posNumber = brandId + year + $(thiselem).val();
     $('[id$=txtPOSNumber]').val(posNumber);
}
