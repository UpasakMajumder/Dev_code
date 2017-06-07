/* eslint-disable */
export default {
  "success": true,
  "payload": {
    "products": {
      "number": "You have 3 items in your cart",
      "items": [{
        "id": 1,
        "image": "",
        "template": "Information letter lamp post Mar 30 2017",
        "isMailingList": false,
        "mailingList": "FCLTR_Mar30_Lamp post",
        "delivery": "Delivery to USPS by Mar 2017 2017 for 328 addresses.",
        "pricePrefix": "$",
        "price": "25.49",
        "isEditable": false,
        "quantityPrefix": "Quantity",
        "quantity": 1
      }, {
        "id": 2,
        "image": "",
        "template": "Information letter lamp post Mar 30 2017",
        "isMailingList": false,
        "mailingList": "FCLTR_Mar30_Lamp post",
        "delivery": "Delivery to USPS by Mar 2017 2017 for 328 addresses.",
        "pricePrefix": "$",
        "price": "25.49",
        "isEditable": false,
        "quantityPrefix": "Quantity",
        "quantity": 1
      }, {
        "id": 3,
        "image": "",
        "template": "Information letter lamp post Mar 30 2017",
        "isMailingList": true,
        "mailingList": "FCLTR_Mar30_Lamp post",
        "delivery": "Delivery to USPS by Mar 2017 2017 for 328 addresses.",
        "pricePrefix": "$",
        "price": "25.49",
        "isEditable": false,
        "quantityPrefix": "Quantity",
        "quantity": 1
      }]
    },
    "deliveryAddresses": {
      "isDeliverable": true,
      "unDeliverableText": "All items will be mailed according to the selected mailing list.",
      "title": "Delivery address",
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
          "checked": true
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
      "title": "Delivery method",
      "description": "Select delivery carrier and option",
      "items": [
        {
          "id": 1,
          "title": "FedEx",
          "icon": "fedex-delivery",
          "opened": false,
          "disabled": false,
          "pricePrefix": "Cannot be delivered",
          "price": "",
          "datePrefix": null,
          "date": null,
          "items": [
            {
              "id": 5,
              "title": "Int'l Economy",
              "checked": false,
              "pricePrefix": "Cannot be delivered",
              "price": "",
              "datePrefix": null,
              "date": null,
              "disabled": true
            },
            {
              "id": 6,
              "title": "Int'l Ground",
              "checked": false,
              "pricePrefix": "Cannot be delivered",
              "price": "",
              "datePrefix": null,
              "date": null,
              "disabled": true
            }
          ]
        },
        {
          "id": 2,
          "title": "FedEx Customer",
          "icon": "fedex-delivery",
          "opened": true,
          "disabled": false,
          "pricePrefix": "Price based on your contract",
          "price": "",
          "datePrefix": null,
          "date": null,
          "items": [
            {
              "id": 2,
              "title": "FedEx customer price",
              "checked": true,
              "pricePrefix": "Price based on your contract",
              "price": "",
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
          "pricePrefix": "Price from",
          "price": "$ 9.99",
          "datePrefix": null,
          "date": null,
          "items": [
            {
              "id": 4,
              "title": "Ground",
              "checked": false,
              "pricePrefix": "Price",
              "price": "$ 9.99",
              "datePrefix": null,
              "date": null,
              "disabled": false
            },
            {
              "id": 7,
              "title": "NextDayStd",
              "checked": false,
              "pricePrefix": "Price",
              "price": "$ 42.52",
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
          "pricePrefix": "Price based on your contract",
          "price": "",
          "datePrefix": null,
          "date": null,
          "items": [
            {
              "id": 8,
              "title": "UPS customer price",
              "checked": false,
              "pricePrefix": "Price based on your contract",
              "price": "",
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
          "pricePrefix": "Cannot be delivered",
          "price": "",
          "datePrefix": null,
          "date": null,
          "items": [
            {
              "id": 1,
              "title": "1st class",
              "checked": false,
              "pricePrefix": "Cannot be delivered",
              "price": "",
              "datePrefix": null,
              "date": null,
              "disabled": true
            },
            {
              "id": 3,
              "title": "First Class",
              "checked": false,
              "pricePrefix": "Cannot be delivered",
              "price": "",
              "datePrefix": null,
              "date": null,
              "disabled": true
            }
          ]
        },
        {
          "id": 6,
          "title": "USPS Customer",
          "icon": "usps-delivery",
          "opened": false,
          "disabled": false,
          "pricePrefix": "Price based on your contract",
          "price": "",
          "datePrefix": null,
          "date": null,
          "items": [
            {
              "id": 9,
              "title": "USPS customer price",
              "checked": false,
              "pricePrefix": "Price based on your contract",
              "price": "",
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
      "isPayable": true,
      "unPayableText": "Your payment plan is set to monthly payment. To change this, contact the administrator.",
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
          "checked": true,
          "hasInput": true,
          "inputPlaceholder": "Insert your PO number"
        }
      ]
    },
    "totals": {
      "title": "Totals",
      "description": null,
      "items": [
        {
          "title": "Summary",
          "value": "$ 30.00"
        },
        {
          "title": "Shipping",
          "value": "$ 0.00"
        },
        {
          "title": "Subtotal",
          "value": "$ 30.00"
        },
        {
          "title": "Tax",
          "value": "$ 0.00"
        },
        {
          "title": "Totals",
          "value": "$ 30.00"
        }
      ]
    },
    "submitLabel": "Place order",
    "validationMessage": "Error"
  },
  "errorMessage": null
}
