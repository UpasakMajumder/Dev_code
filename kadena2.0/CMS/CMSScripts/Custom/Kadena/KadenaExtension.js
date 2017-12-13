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
   var datepickerpath='';
  var datePicker=$('[id$=hdnDatepickerUrl]');
  if(typeof datePicker !="undefined")
  {
    datepickerpath=$(datePicker).val();
  }
  
  $('[id$=txtExpDate]').datepicker({
    showOn: "both",
    buttonImage: datepickerpath,
    buttonImageOnly: false,
    minDate: 0
  });
});

$('[id$=productImage]').on('change', function () {
  $('.product-img').hide();
});
//end
//Product Campaign js https://png.icons8.com/?id=3344&size=280
$(function () {
  var datepickerpath = '';
  var datePicker=$('[id$=hdnDatepickerUrl]');
  if(typeof datePicker !="undefined")
  {
    datepickerpath=$(datePicker).val();
  }
 
  $('[id$=txtExpireDate]').datepicker({
    showOn: "both",
    buttonImage: datepickerpath,
    buttonImageOnly: false,
    minDate: 0
  });
});

$('[id$=productImage]').on('change', function () {
  $('.product-img').hide();
});
//end