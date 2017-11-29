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
        //postal_code: "#ZIP",
        country: ".js-Country"
    }],
    //agent: "website:plugin-demo@latest|"
});