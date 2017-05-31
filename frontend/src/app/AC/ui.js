/* eslint-disable */
export default {
  "deliveryAddresses": {
    "title": "Delivery address",
    "description": "Products will be delivered to selected address by",
    "addAddressLabel": "New address",

    "items": [{
      "id": 1,
      "street": ["4001 Valley Industrial Blvd"],
      "city": "Shakopee",
      "state": "MN",
      "zip": "55379",
      "checked": true
    },
      {
        "id": 2,
        "street": ["4001 Valley Industrial Blvd"],
        "city": "Shakopee",
        "state": "MN",
        "zip": "55379",
        "checked": false
      }
    ]
  },
  "deliveryMethod": {
    "title": "Delivery method",
    "description": "",
    "items": [{
      "id": 1,
      "title": "FedEx",
      "icon": "fedex-delivery",
      "opened": true,
      "disabled": false,
      "pricePrefix": "Price",
      "price": "$ 13.00",
      "datePrefix": "Get it",
      "date": "tomorrow",
      "items": [{
        "id": 1,
        "title": "FedEx Overnight",
        "checked": true,
        "pricePrefix": "Price",
        "price": "$ 13.00",
        "datePrefix": "Get it",
        "date": "May 16-20",
        "disabled": false
      },
      {
        "id": 2,
        "title": "FedEx Classik",
        "checked": false,
        "pricePrefix": "Price",
        "price": "$ 13.00",
        "datePrefix": "Get it",
        "date": "May 16-20",
        "disabled": false
      },
      {
        "id": 3,
        "title": "FedEx Flash",
        "checked": false,
        "pricePrefix": "Price",
        "price": "$ 1359.00",
        "datePrefix": "Get it",
        "date": "in 5 seconds",
        "disabled": false
      }]
    },
    {
      "id": 2,
      "title": "UPS",
      "icon": "ups-delivery",
      "opened": false,
      "disabled": false,
      "pricePrefix": "Price",
      "price": "$ 13.00",
      "datePrefix": "Get it",
      "date": "tomorrow",
      "items": [{
        "id": 4,
        "title": "UPS Overnight",
        "checked": false,
        "pricePrefix": "Price",
        "price": "$ 13.00",
        "datePrefix": "Get it",
        "date": "May 16-20",
        "disabled": false
      },
      {
        "id": 5,
        "title": "UPS Classik",
        "checked": false,
        "pricePrefix": "Price",
        "price": "$ 13.00",
        "datePrefix": "Get it",
        "date": "May 16-20",
        "disabled": false
      },
      {
        "id": 6,
        "title": "UPS Flash",
        "checked": false,
        "pricePrefix": "Price",
        "price": "$ 1359.00",
        "datePrefix": "Get it",
        "date": "in 5 seconds",
        "disabled": false
      }]
    },
    {
      "id": 3,
      "title": "USPS",
      "icon": "usps-delivery",
      "opened": false,
      "disabled": false,
      "pricePrefix": "Price",
      "price": "$ 13.00",
      "datePrefix": "Get it",
      "date": "tomorrow",
      "items": [{
        "id": 7,
        "title": "USPS Overnight",
        "checked": false,
        "pricePrefix": "Price",
        "price": "$ 13.00",
        "datePrefix": "Get it",
        "date": "May 16-20",
        "disabled": false
      },
      {
        "id": 8,
        "title": "USPS Classik",
        "checked": false,
        "pricePrefix": "Price",
        "price": "$ 13.00",
        "datePrefix": "Get it",
        "date": "May 16-20",
        "disabled": false
      },
      {
        "id": 9,
        "title": "USPS Flash",
        "checked": false,
        "pricePrefix": "Price",
        "price": "$ 1359.00",
        "datePrefix": "Get it",
        "date": "in 5 seconds",
        "disabled": false
      }]
    }]
  },
  "paymentMethod": {
    "title": "Payment method",
    "description": "",

    "items": [{
      "id": 1,
      "title": "Credit card",
      "icon": "credit-card",
      "disabled": true,
      "checked": false,
    },{
      "id": 2,
      "title": "PayPal",
      "icon": "paypal-payment",
      "disabled": true,
      "checked": false,
    },{
      "id": 3,
      "title": "Purchase order",
      "icon": "order-payment",
      "disabled": false,
      "checked": true,
      "hasInput": true,
      "placeholderInput": "Insert your PO number"
    }]

  },
  "totals": {
    "title": "Total",
    "description": "",
    "items": [{
      "title": "Summary",
      "value": "$ 1,025.49"
    },
      {
        "title": "Shipping",
        "value": "$ 42.64"
      },
      {
        "title": "Subtotal",
        "value": "$ 67.49"
      },
      {
        "title": "Tax",
        "value": "$ 16.20"
      },
      {
        "title": "Total",
        "value": "$ 1,383.68"
      }]
  },
  "submitLabel": "Submit order",
  "validationMessage": "Error"
}
