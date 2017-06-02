/* eslint-disable */
export default {
  "success": true,
  "payload": {
    "deliveryAddresses": {
      "title": "Delivery",
      "description": "Products will be delivered to selected address by",
      "addAddressLabel": "New address",
      "items": [
        {
          "street": [
            "Test Address line 1"
          ],
          "city": "COLLIERVILLE",
          "state": "TN",
          "zip": "38017",
          "id": 1,
          "checked": false
        },
        {
          "street": [
            "House number 123"
          ],
          "city": "Red Watter Village",
          "state": "",
          "zip": "56161",
          "id": 2,
          "checked": false
        },
        {
          "street": [
            "Sun street 1"
          ],
          "city": "Tokyo",
          "state": "",
          "zip": "13228",
          "id": 3,
          "checked": false
        },
        {
          "street": [
            "Actum Hyper Company"
          ],
          "city": "Prague",
          "state": "",
          "zip": "14000",
          "id": 4,
          "checked": false
        }
      ]
    },
    "deliveryMethods": {
      "title": "Delivery",
      "description": "Select delivery carrier and option",
      "items": [
        {
          "id": 1,
          "title": "FedEx",
          "icon": "fedex-delivery",
          "opened": false,
          "disabled": false,
          "pricePrefix": null,
          "price": null,
          "datePrefix": null,
          "date": null,
          "items": [
            {
              "id": 5,
              "title": "Int'l Economy",
              "checked": false,
              "pricePrefix": null,
              "price": "0",
              "datePrefix": null,
              "date": null,
              "disabled": false
            },
            {
              "id": 6,
              "title": "Int'l Ground",
              "checked": false,
              "pricePrefix": null,
              "price": "0",
              "datePrefix": null,
              "date": null,
              "disabled": false
            }
          ]
        },
        {
          "id": 2,
          "title": "FedEx Customer",
          "icon": "fedex-delivery",
          "opened": false,
          "disabled": false,
          "pricePrefix": null,
          "price": null,
          "datePrefix": null,
          "date": null,
          "items": [
            {
              "id": 2,
              "title": "FedEx customer price",
              "checked": false,
              "pricePrefix": null,
              "price": "0",
              "datePrefix": null,
              "date": null,
              "disabled": false
            }
          ]
        },
        {
          "id": 3,
          "title": "UPS",
          "icon": "ups-delivery",
          "opened": false,
          "disabled": false,
          "pricePrefix": null,
          "price": null,
          "datePrefix": null,
          "date": null,
          "items": [
            {
              "id": 4,
              "title": "Ground",
              "checked": false,
              "pricePrefix": null,
              "price": "9.4",
              "datePrefix": null,
              "date": null,
              "disabled": false
            },
            {
              "id": 7,
              "title": "NextDayStd",
              "checked": false,
              "pricePrefix": null,
              "price": "30.58",
              "datePrefix": null,
              "date": null,
              "disabled": false
            }
          ]
        },
        {
          "id": 4,
          "title": "UPS Customer",
          "icon": "ups-delivery",
          "opened": false,
          "disabled": false,
          "pricePrefix": null,
          "price": null,
          "datePrefix": null,
          "date": null,
          "items": [
            {
              "id": 8,
              "title": "UPS customer price",
              "checked": false,
              "pricePrefix": null,
              "price": "0",
              "datePrefix": null,
              "date": null,
              "disabled": false
            }
          ]
        },
        {
          "id": 5,
          "title": "USPS",
          "icon": "usps-delivery",
          "opened": false,
          "disabled": false,
          "pricePrefix": null,
          "price": null,
          "datePrefix": null,
          "date": null,
          "items": [
            {
              "id": 1,
              "title": "1st class",
              "checked": false,
              "pricePrefix": null,
              "price": "0",
              "datePrefix": null,
              "date": null,
              "disabled": false
            },
            {
              "id": 3,
              "title": "First Class",
              "checked": false,
              "pricePrefix": null,
              "price": "0",
              "datePrefix": null,
              "date": null,
              "disabled": false
            }
          ]
        },
        {
          "id": 6,
          "title": "USPS Customer",
          "icon": "usps-delivery",
          "opened": false,
          "disabled": false,
          "pricePrefix": null,
          "price": null,
          "datePrefix": null,
          "date": null,
          "items": [
            {
              "id": 9,
              "title": "USPS customer price",
              "checked": false,
              "pricePrefix": null,
              "price": "0",
              "datePrefix": null,
              "date": null,
              "disabled": false
            }
          ]
        }
      ]
    },
    "paymentMethods": {
      "title": "Payment",
      "description": null,
      "items": [
        {
          "id": 1,
          "title": "Credit card",
          "icon": "credit-card",
          "disabled": true,
          "checked": false,
          "hasInput": false,
          "inputPlaceholder": null
        },
        {
          "id": 2,
          "title": "Pay Pal",
          "icon": "paypal-payment",
          "disabled": true,
          "checked": false,
          "hasInput": false,
          "inputPlaceholder": null
        },
        {
          "id": 3,
          "title": "Purchase order",
          "icon": "order-payment",
          "disabled": false,
          "checked": false,
          "hasInput": true,
          "inputPlaceholder": "Insert your PO number"
        }
      ]
    },
    "totals": {
      "title": "Total",
      "description": null,
      "items": [
        {
          "title": "Summary",
          "value": "20"
        },
        {
          "title": "Shipping",
          "value": "0"
        },
        {
          "title": "Subtotal",
          "value": "0"
        },
        {
          "title": "Tax 8%",
          "value": "0"
        },
        {
          "title": "Totals",
          "value": "20"
        }
      ]
    },
    "submitLabel": "Place order",
    "validationMessage": "Error"
  },
  "errorMessage": null
}
