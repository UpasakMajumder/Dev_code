
$(document).ready(function () {

    $('.js-Country').find("select").addClass('js-CountrySelect');
    if ($('.js-UniState').is(':visible')) {
        $('.js-UniState').find("select").addClass('js-State');
    }

    $('.js-btnSmarty').click(function (event) {
        if ($('.js-Address').val().trim() != '') {
            $.ajax({
                url: 'https://us-street.api.smartystreets.com/street-address',
                data: {
                    'auth-token': htmlKey,
                    'street': $('.js-Address').val().trim(),
                    'city': $('.js-City').val().trim(),
                    'zipcode': $('.js-Zipcode').val().trim(),
                    'state': $('.js-State').find("select option:selected").eq(0).text(),
                    'country': $('.js-Country').find("select option:selected").eq(0).text(),
                    'candidates': 4
                },
                type: 'GET',
                dataType: 'json',
                success: function (data, status, xhr) {
                    if (data != '') {
                        var analysisData = data[0].analysis;
                        if (analysisData != '') {
                            var activeData = analysisData.active;
                            if (activeData.indexOf('Y') > -1) {
                                $('.js-errAddress').html('');
                                $('.js-errAddress').css('display', 'none');
                                return true;
                            }
                        }
                    }
                    else {
                        event.preventDefault();
                        $('.smarty-popup').css('display', 'none');
                        $('.smarty-popup').parent().empty();
                        $('.js-errAddress').html('Please enter valid address');
                        $('.js-errAddress').css('display', 'block');
                    }
                },
                fail: function (error, status, xhr) {
                    console.log(error);
                }
            });
        }
        else {
            $('.js-errAddress').html('Please enter  address');
            $('.js-errAddress').css('display', 'block');
        }
    });
});
var htmlKey = '34982985215906425'
var ss = jQuery.LiveAddress({
    autocomplete: 5,
    key: htmlKey,
    missingInputMessage: "Not enough input<br>",
    certifyMessage: "Use my address",
    verifySecondary: true,
    waitForStreet: true,
    debug: false,
    target: "US|INTERNATIONAL",
    addresses: [{
        address1: ".js-Address",
        locality: ".js-City",
        administrative_area: ".js-State",
        postal_code: ".js-Zipcode",
        country: ".js-CountrySelect"
    }],
});








