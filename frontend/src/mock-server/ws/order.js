module.exports.reports = {
  rows1: {
    "success": true,
    "errorMessage": null,
    "payload": {
      "rows": [
        {
          url: '#',
          items: [
            '1111111111111111111111111', '2222222222222222222222', '2017-08-07T09:12:08.108892Z','333333333333333333333', '44444444444444444444', '555555555555555555555555555555', '66666666666666666666', '766666666666666666666', '8666666666666666666', '2017-08-10T09:12:08.108892Z', '106666666666666666666666'
          ]
        },
        {
          url: '#',
          items: [
            '1', 'a', '2017-08-10T09:12:08.108892Z','3', '4', '5', '2', '7', '8', '2017-08-10T09:12:08.108892Z', '10'
          ]
        },
        {
          url: '#',
          items: [
            '1', '2', '2017-08-04T09:12:08.108892Z','3', 'c', '5', '6', '7', '8', '2017-08-10T09:12:08.108892Z', '10'
          ]
        },
        {
          url: '#',
          items: [
            '1', '2', '2017-08-03T09:12:08.108892Z','3', '4', '5', '6', '7', '123', '2017-08-10T09:12:08.108892Z', '10'
          ]
        },
        {
          url: '#',
          items: [
            '1', '2', '2017-08-02T09:12:08.108892Z','d', '4', '5', '6', '7', '8', '2017-08-10T09:12:08.108892Z', '10'
          ]
        }
      ],
      "pagination": {
        "pagesCount": 2,
        "rowsCount": 5,
        "rowsOnPage": 3
      }
    }
  },
  rows2: {
    "success": true,
    "errorMessage": null,
    "payload": {
      "rows": [
        {
          url: '#',
          items: [
            '1666666666666666666666666666666', '2666666666666666666666666666666', '2017-08-07T09:12:08.108892Z','3666666666666666666666666666666', '4666666666666666666666666666666', '5666666666666666666666666666666', '6666666666666666666666666666666', '7666666666666666666666666666666', '8666666666666666666666666666666', '2017-08-10T09:12:08.108892Z', '10666666666666666666666666666666'
          ]
        },
        {
          url: '#',
          items: [
            '1', '2', '2017-08-02T09:12:08.108892Z','d', '4', '5', '6', '7', '8', '2017-08-10T09:12:08.108892Z', '10'
          ]
        },
        {
          url: '#',
          items: [
            '1', '2', '2017-08-04T09:12:08.108892Z','3', 'c', '5', '6', '7', '8', '2017-08-10T09:12:08.108892Z', '10'
          ]
        },
        {
          url: '#',
          items: [
            '1', 'a', '2017-08-10T09:12:08.108892Z','3', '4', '5', '2', '7', '8', '2017-08-10T09:12:08.108892Z', '10'
          ]
        },
        {
          url: '#',
          items: [
            '1', '2', '2017-08-03T09:12:08.108892Z','3', '4', '5', '6', '7', '123', '2017-08-10T09:12:08.108892Z', '10'
          ]
        }
      ],
      "pagination": {
        "pagesCount": 2,
        "rowsCount": 5,
        "rowsOnPage": 3
      }
    }
  }
};

module.exports.detail =  {
  "success": true,
  "errorMessage": null,
  "payload": {
    "dateTimeNAString": "N/A",
    "commonInfo": {
      status: {
        title: 'Status',
        value: 'In progress'
      },
      orderDate: {
        title: 'Order date',
        value: '2017-02-01T09:12:08.108892Z'
      },
      shippingDate: {
        title: 'Shipping date',
        value: null
      },
      totalCost: {
        title: 'Total cost',
        value: '$ 1,383.68'
      }
    },
    "shippingInfo": {
      "title": "Shipping",
      "deliveryMethod": "fedex-delivery",
      "message": "All items will be mailed according to the selected mailing list",
      "address": {
        "address1": "4001 Valley Industrial Blvd",
        "city": "Shakopee",
        "state": "MN",
        "zip": "55379"
      },
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
      "date": "2017-08-07T09:12:08.108892Z",
      "datePrefix": "Payment date"
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
          "mailingListPrefix": "Mailing list",
          "shippingDatePrefix": "Tracking ID",
          "trackingIdPrefix": "Shipping date",
          "templatePrefix": "Shipping date",
          "price": "$ 112.2",
          "quantityPrefix": "Quantity:", // Addresses/Quantity,
          "quantityShippedPrefix": "Quantity shipped:",
          "quantityShipped": 22,
          "quantity": 11,
          "downloadPdfURL": "#"
        },
        {
          "id": 1,
          "isReport": true,
          "image": "http://satyr.io/200-500x300-700",
          "template": "Information letter lamp post Mar 30 3017",
          "mailingList": "Mailing",
          "shippingDate": "2017-08-07T09:12:08.108892Z",
          "trackingId": "",
          "mailingListPrefix": "Mailing list",
          "shippingDatePrefix": "Tracking ID",
          "trackingIdPrefix": "Shipping date",
          "templatePrefix": "Shipping date",
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
