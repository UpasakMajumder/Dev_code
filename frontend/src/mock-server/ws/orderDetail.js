module.exports =  {
  "success": true,
  "errorMessage": null,
  "payload": {
    "commonInfo": {
      "status": "In progress",
      "orderDate": "2017-08-07T09:12:08.108892Z",
      "shippingDate": "N/A",
      "totalCost": "$ 1,383.68"
    },
    "shippingInfo": {
      "title": "Shipping",
      "deliveryMethod": "fedex-delivery",
      "message": "All items will be mailed according to the selected mailing list",
      "address": "4001 Valley Industrial Blvd, Shakopee, MN 55379",
      "tracking": { // null
        "text": "Track your packages",
        "url": "#"
      }
    },
    "paymentInfo": {
      "title": "Payment",
      "paymentIcon": "order-payment",
      "paidBy": "Paid by credit card",
      "paymentDetail": "Mastercard •••• 4062",
      "date": "2017-08-07T09:12:08.108892Z"
    },
    "pricingInfo": {
      "title": "Pricing",
      "items": [
        {
          "title": "Summary",
          "value": "$ 112.20"
        },
        {
          "title": "Shipping",
          "value": "$ 0.00"
        },
        {
          "title": "Subtotal",
          "value": "$ 112.20"
        },
        {
          "title": "Tax",
          "value": "$ 0.00"
        },
        {
          "title": "Totals",
          "value": "$ 112.20"
        }
      ]
    },
    "orderedItems": {
      "title": "Ordered Items",
      "items": [
        {
          "id": 1,
          "isReport": true,
          "image": "http://satyr.io/200-500x300-700",
          "template": "Information letter lamp post Mar 30 3017",
          "mailingList": "Mailing",
          "shippingDate": "2017-08-07T09:12:08.108892Z",
          "trackingId": "",
          "price": "$ 112.2",
          "quantityPrefix": "Quantity:", // Addresses/Quantity,
          "quantityShippedPrefix": "Quantity shipped:",
          "quantityShipped": 22,
          "quantity": 11,
          "downloadPdfURL": "#"
        }
      ]
    }
  }
};
