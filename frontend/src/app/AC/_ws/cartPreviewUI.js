/* eslint-disable */
export default {
  "emptyCartMessage": "Your cart is empty",
  "cart": {
    "label": "Proceed to checkout",
    "url": "#"
  },
  "alertMessage": "Added to cart",
  "summaryPrice": {
    "pricePrefix": "$",
    "price": "1000.0"
  },
  "items": [
    {
      "id": 33,
      "image": "",
      "template": "Template",
      "isMailingList": false,
      "mailingList": "Mailing list",
      "pricePrefix": "$",
      "price": "112.2",
      "quantityPrefix": "Quantity:",
      "quantity": 11
    },
    {
      "id": 31,
      "image": "http://satyr.io/50-200x100-300?2",
      "template": "Template",
      "isMailingList": false,
      "mailingList": "Mailing list",
      "pricePrefix": "$",
      "price": "112.2",
      "quantityPrefix": "Quantity:",
      "quantity": 11
    },
    {
      "id": 13,
      "image": "http://satyr.io/50-200x100-300?3",
      "template": "Template",
      "isMailingList": true,
      "mailingList": "Mailing list",
      "pricePrefix": "$",
      "price": "112.2",
      "quantityPrefix": "Quantity:",
      "quantity": 100
    },
    {
      "id": 13123,
      "image": "http://satyr.io/50-200x100-300?3",
      "template": "Template",
      "isMailingList": true,
      "mailingList": "Mailing list",
      "pricePrefix": "$",
      "price": "112.2",
      "quantityPrefix": "Quantity:",
      "quantity": 100
    },
    {
      "id": 13123123,
      "image": "http://satyr.io/50-200x100-300?3",
      "template": "Template",
      "isMailingList": true,
      "mailingList": "Mailing list",
      "pricePrefix": "$",
      "price": "112.2",
      "quantityPrefix": "Quantity:",
      "quantity": 100
    },
    {
      "id": 131231231,
      "image": "http://satyr.io/50-200x100-300?3",
      "template": "Template",
      "isMailingList": true,
      "mailingList": "Mailing list",
      "pricePrefix": "$",
      "price": "112.2",
      "quantityPrefix": "Quantity:",
      "quantity": 100
    }
  ]
};

export const newState = {
  "cartPreview": {
    "summaryPrice": {
      "pricePrefix": "$",
      "price": "500.0"
    },
    "cart": {
      "label": "Proceed to checkout",
      "url": "/checkout"
    },
    "items": [
      {
        "id": 33,
        "image": "",
        "template": "Template",
        "isMailingList": false,
        "mailingList": "Mailing list",
        "pricePrefix": "$",
        "price": "112.2",
        "quantityPrefix": "Quantity:",
        "quantity": 11
      },
      {
        "id": 31,
        "image": "http://satyr.io/50-200x100-300?2",
        "template": "Template",
        "isMailingList": false,
        "mailingList": "Mailing list",
        "pricePrefix": "$",
        "price": "112.2",
        "quantityPrefix": "Quantity:",
        "quantity": 11
      }
    ]
  },
  "confirmation": {
    "alertMessage": "Your selected product(s) have been added to the shopping cart. Please select an option below",
    "btns": {
      "cancel": {
        "text": "Continue Shopping",
        "url": "/product"
      },
      "checkout": {
        "text": "Checkout",
        "url": "/checkout"
      }
    }
  }
};
