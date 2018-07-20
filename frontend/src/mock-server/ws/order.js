module.exports.reports = {
  rows1: {
    "success": true,
    "errorMessage": null,
    "payload": {
      // "rows": [
        /*
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
        */
      // ],
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
            '1', '2', '2017-08-04T09:12:08.108892Z','3', 'c', '5', '6', '7', '8', '2017-08-10T09:12:08.108892Z', { type: 'tracking', items: [{ id: '1' }, { id: '2', url: '#' }] }
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

module.exports.recent = {
  requiringApproval: {
    "success": true,
    "errorMessage": null,
    "payload": {
      headings: [
        "order number",
        "order date",
        "ordered",
        "order status",
        "delivery date",
        ""
      ],
      pageInfo: null,
      noOrdersMessage: "You have no orders",
      rows: [
        {
          "orderNumber": "0010-00162-17-00004",
          "orderDate": "2017-08-07T09:12:08.108892Z",
          "items": [
            {
              "name": "Product 3",
              "quantity": 1
            }
          ],
          "orderStatus": "Requiring Approval",
          "deliveryDate": "2017-08-07T09:12:08.108892Z",
          "shippingDate": "2017-08-07T09:12:08.108892Z",
          "viewBtn": {
            "text": "View",
            "url": "~/recent-orders/order-detail?orderID=0010-00162-17-00004"
          }
        },
        {
          "orderNumber": "0001-00091-17-00002",
          "orderDate": "2017-08-07T09:12:08.108892Z",
          "items": [
            {
              "name": "Chilli product",
              "quantity": 1
            },
            {
              "name": "POD static product",
              "quantity": 1
            },
            {
              "name": "Static product",
              "quantity": 1
            }
          ],
          "orderStatus": "Requiring Approval",
          "deliveryDate": "2017-08-07T09:12:08.108892Z",
          "shippingDate": "2017-08-07T09:12:08.108892Z",
          "viewBtn": {
            "text": "View",
            "url": "~/recent-orders/order-detail?orderID=0001-00091-17-00002"
          }
        },
        {
          "orderNumber": "0010-00184-17-00009",
          "orderDate": "2017-08-07T09:12:08.108892Z",
          "items": [
            {
              "name": "POD",
              "quantity": 4
            },
            {
              "name": "Inventory",
              "quantity": 6
            },
            {
              "name": "Product 3",
              "quantity": 5
            },
            {
              "name": "Katkas product",
              "quantity": 78
            }
          ],
          "orderStatus": "Requiring Approval",
          "deliveryDate": "2017-08-07T09:12:08.108892Z",
          "shippingDate": "2017-08-07T09:12:08.108892Z",
          "viewBtn": {
            "text": "View",
            "url": "~/recent-orders/order-detail?orderID=0010-00184-17-00009"
          }
        },
        {
          "orderNumber": "0001-00174-17-00001",
          "orderDate": "2017-06-21 14:16:24",
          "items": [
            {
              "name": "Inventory",
              "quantity": 1
            }
          ],
          "orderStatus": "Requiring Approval",
          "deliveryDate": "2017-08-07T09:12:08.108892Z",
          "shippingDate": "2017-08-07T09:12:08.108892Z",
          "viewBtn": {
            "text": "View",
            "url": "~/recent-orders/order-detail?orderID=0001-00174-17-00001"
          }
        },
        {
          "orderNumber": "0001-00174-17-00012",
          "orderDate": "2017-06-23 13:26:16",
          "items": [
            {
              "name": "Inventory",
              "quantity": 1
            }
          ],
          "orderStatus": "Requiring Approval",
          "deliveryDate": "2017-08-07T09:12:08.108892Z",
          "shippingDate": "2017-08-07T09:12:08.108892Z",
          "viewBtn": {
            "text": "View",
            "url": "~/recent-orders/order-detail?orderID=0001-00174-17-00012"
          }
        }
      ]
    }
  },
  ui: {
    "success": true,
    "errorMessage": null,
    "payload": {
      headings: [
        "order number",
        "order date",
        "ordered",
        "order status",
        "delivery date",
        ""
      ],
      pageInfo: {
        "pagesCount": 10,
        "rowsCount": 28,
        "rowsOnPage": 3
      },
      noOrdersMessage: "You have no orders",
      rows: [
        {
          "orderNumber": "0010-00162-17-00004",
          "orderDate": "2017-08-07T09:12:08.108892Z",
          "items": [
            {
              "name": "Product 3",
              "quantity": 1
            }
          ],
          "orderStatus": "Rejected",
          "deliveryDate": "2017-08-07T09:12:08.108892Z",
          "shippingDate": "2017-08-07T09:12:08.108892Z",
          "viewBtn": {
            "text": "View",
            "url": "~/recent-orders/order-detail?orderID=0010-00162-17-00004"
          }
        },
        {
          "orderNumber": "0001-00091-17-00002",
          "orderDate": "2017-08-07T09:12:08.108892Z",
          "items": [
            {
              "name": "Chilli product",
              "quantity": 1
            },
            {
              "name": "POD static product",
              "quantity": 1
            },
            {
              "name": "Static product",
              "quantity": 1
            }
          ],
          "orderStatus": null,
          "deliveryDate": "2017-08-07T09:12:08.108892Z",
          "shippingDate": "2017-08-07T09:12:08.108892Z",
          "viewBtn": {
            "text": "View",
            "url": "~/recent-orders/order-detail?orderID=0001-00091-17-00002"
          }
        },
        {
          "orderNumber": "0010-00184-17-00009",
          "orderDate": "2017-08-07T09:12:08.108892Z",
          "items": [
            {
              "name": "POD",
              "quantity": 4
            },
            {
              "name": "Inventory",
              "quantity": 6
            },
            {
              "name": "Product 3",
              "quantity": 5
            },
            {
              "name": "Katkas product",
              "quantity": 78
            }
          ],
          "orderStatus": null,
          "deliveryDate": "2017-08-07T09:12:08.108892Z",
          "shippingDate": "2017-08-07T09:12:08.108892Z",
          "viewBtn": {
            "text": "View",
            "url": "~/recent-orders/order-detail?orderID=0010-00184-17-00009"
          }
        },
        {
          "orderNumber": "0001-00174-17-00001",
          "orderDate": "2017-06-21 14:16:24",
          "items": [
            {
              "name": "Inventory",
              "quantity": 1
            }
          ],
          "orderStatus": null,
          "deliveryDate": "2017-08-07T09:12:08.108892Z",
          "shippingDate": "2017-08-07T09:12:08.108892Z",
          "viewBtn": {
            "text": "View",
            "url": "~/recent-orders/order-detail?orderID=0001-00174-17-00001"
          }
        },
        {
          "orderNumber": "0001-00174-17-00012",
          "orderDate": "2017-06-23 13:26:16",
          "items": [
            {
              "name": "Inventory",
              "quantity": 1
            }
          ],
          "orderStatus": "Rejected",
          "deliveryDate": "2017-08-07T09:12:08.108892Z",
          "shippingDate": "2017-08-07T09:12:08.108892Z",
          "viewBtn": {
            "text": "View",
            "url": "~/recent-orders/order-detail?orderID=0001-00174-17-00012"
          }
        }
      ]
    }
  },
  page1: {
    "success": true,
    "errorMessage": null,
    "payload": {
      "rows": [
        {
          "orderNumber": "0010-00162-17-фывто123",
          "orderDate": "2017-08-07T09:12:08.108892Z",
          "items": [
            {
              "name": "Product 3",
              "quantity": 1
            }
          ],
          "orderStatus": "Rejected",
          "deliveryDate": "2017-08-07T09:12:08.108892Z",
          "shippingDate": "2017-08-07T09:12:08.108892Z",
          "viewBtn": {
            "text": "View",
            "url": "~/recent-orders/order-detail?orderID=0010-00162-17-00004"
          }
        },
        {
          "orderNumber": "0001-00091-17-324фыв",
          "orderDate": "2017-08-07T09:12:08.108892Z",
          "items": [
            {
              "name": "Chilli product",
              "quantity": 1
            },
            {
              "name": "POD static product",
              "quantity": 1
            },
            {
              "name": "Static product",
              "quantity": 1
            }
          ],
          "orderStatus": null,
          "deliveryDate": "2017-08-07T09:12:08.108892Z",
          "shippingDate": "2017-08-07T09:12:08.108892Z",
          "viewBtn": {
            "text": "View",
            "url": "~/recent-orders/order-detail?orderID=0001-00091-17-00002"
          }
        },
        {
          "orderNumber": "0010-00184-17-1фывф",
          "orderDate": "2017-06-22 12:05:49",
          "items": [
            {
              "name": "POD",
              "quantity": 4
            },
            {
              "name": "Inventory",
              "quantity": 6
            },
            {
              "name": "Product 3",
              "quantity": 5
            },
            {
              "name": "Katkas product",
              "quantity": 78
            }
          ],
          "orderStatus": null,
          "deliveryDate": "2017-08-07T09:12:08.108892Z",
          "shippingDate": "2017-08-07T09:12:08.108892Z",
          "viewBtn": {
            "text": "View",
            "url": "~/recent-orders/order-detail?orderID=0010-00184-17-00009"
          }
        },
        {
          "orderNumber": "0001-00174-17-фывт123",
          "orderDate": "2017-08-07T09:12:08.108892Z",
          "items": [
            {
              "name": "Inventory",
              "quantity": 1
            }
          ],
          "orderStatus": null,
          "deliveryDate": "2017-08-07T09:12:08.108892Z",
          "shippingDate": "2017-08-07T09:12:08.108892Z",
          "viewBtn": {
            "text": "View",
            "url": "~/recent-orders/order-detail?orderID=0001-00174-17-00001"
          }
        },
        {
          "orderNumber": "0001-00174-17-121",
          "orderDate": "2017-08-07T09:12:08.108892Z",
          "items": [
            {
              "name": "Inventory",
              "quantity": 1
            }
          ],
          "orderStatus": "Rejected",
          "deliveryDate": "2017-08-07T09:12:08.108892Z",
          "shippingDate": "2017-08-07T09:12:08.108892Z",
          "viewBtn": {
            "text": "View",
            "url": "~/recent-orders/order-detail?orderID=0001-00174-17-00012"
          }
        }
      ]
    }
  },
  page2: {
    "success": true,
    "errorMessage": null,
    "payload": {
      "rows": [
        {
          "orderNumber": "0010-00162-17-00004",
          "orderDate": "2017-08-07T09:12:08.108892Z",
          "items": [
            {
              "name": "Product 3",
              "quantity": 1
            }
          ],
          "orderStatus": "Rejected",
          "deliveryDate": "2017-08-07T09:12:08.108892Z",
          "shippingDate": "2017-08-07T09:12:08.108892Z",
          "viewBtn": {
            "text": "View",
            "url": "~/recent-orders/order-detail?orderID=0010-00162-17-00004"
          }
        },
        {
          "orderNumber": "0001-00091-17-00002",
          "orderDate": "2017-08-07T09:12:08.108892Z",
          "items": [
            {
              "name": "Chilli product",
              "quantity": 1
            },
            {
              "name": "POD static product",
              "quantity": 1
            },
            {
              "name": "Static product",
              "quantity": 1
            }
          ],
          "orderStatus": null,
          "deliveryDate": "2017-08-07T09:12:08.108892Z",
          "shippingDate": "2017-08-07T09:12:08.108892Z",
          "viewBtn": {
            "text": "View",
            "url": "~/recent-orders/order-detail?orderID=0001-00091-17-00002"
          }
        },
        {
          "orderNumber": "0010-00184-17-00009",
          "orderDate": "2017-08-07T09:12:08.108892Z",
          "items": [
            {
              "name": "POD",
              "quantity": 4
            },
            {
              "name": "Inventory",
              "quantity": 6
            },
            {
              "name": "Product 3",
              "quantity": 5
            },
            {
              "name": "Katkas product",
              "quantity": 78
            }
          ],
          "orderStatus": null,
          "deliveryDate": "2017-08-07T09:12:08.108892Z",
          "shippingDate": "2017-08-07T09:12:08.108892Z",
          "viewBtn": {
            "text": "View",
            "url": "~/recent-orders/order-detail?orderID=0010-00184-17-00009"
          }
        },
        {
          "orderNumber": "0001-00174-17-00001",
          "orderDate": "2017-06-21 14:16:24",
          "items": [
            {
              "name": "Inventory",
              "quantity": 1
            }
          ],
          "orderStatus": null,
          "deliveryDate": "2017-08-07T09:12:08.108892Z",
          "shippingDate": "2017-08-07T09:12:08.108892Z",
          "viewBtn": {
            "text": "View",
            "url": "~/recent-orders/order-detail?orderID=0001-00174-17-00001"
          }
        },
        {
          "orderNumber": "0001-00174-17-00012",
          "orderDate": "2017-06-23 13:26:16",
          "items": [
            {
              "name": "Inventory",
              "quantity": 1
            }
          ],
          "orderStatus": "Rejected",
          "deliveryDate": "2017-08-07T09:12:08.108892Z",
          "shippingDate": "2017-08-07T09:12:08.108892Z",
          "viewBtn": {
            "text": "View",
            "url": "~/recent-orders/order-detail?orderID=0001-00174-17-00012"
          }
        }
      ]
    }
  }
};

module.exports.edit = {
  success: true,
  errorMessage: 'No! This is Wrong! Choose different approach',
  payload: {
    "pricingInfo": [
      {
        "title": "Summary",
        "value": "$ 113.20"
      },
      {
        "title": "Shipping",
        "value": "$ 1.00"
      },
      {
        "title": "Subtotal",
        "value": "$ 113.20"
      },
      {
        "title": "Tax",
        "value": "$ 2.00"
      },
      {
        "title": "Totals",
        "value": "$ 212.20"
      }
    ],
    "ordersPrice": [
      {
        lineNumber: "lineNumber-1", // like 693 row
        price: "$ 5"
      },
      {
        lineNumber: "lineNumber-2", // like 737 row
        price: "$ 50"
      }
    ],
    "orderHistory": {
      itemChanges: {
        title: "Item Changes",
        headers: ['Item Desc.', 'Chage Type', 'Original', 'New', 'Date', 'User'],
        items: [
          [
            {
              type: "text",
              text: "Item Name #1"
            },
            {
              type: "text",
              text: "Qty Change"
            },
            {
              type: "text",
              text: "20"
            },
            {
              type: "text",
              text: "15"
            },
            {
              type: "text",
              text: "05/29/2018 10:35:20 AM"
            },
            {
              type: "text",
              text: "asdasd@gmail.com"
            }
          ]
        ]
      },
      orderChanges: {
        title: "Order Changes",
        headers: ['Category', 'Original', 'New', 'Date', 'User'],
        items: [
          [
            {
              type: "text",
              text: "Shipping"
            },
            {
              type: "text",
              text: "Overnight"
            },
            {
              type: "text",
              text: "Order: #1450"
            },
            {
              type: "text",
              text: "05/29/2018 10:35:20 AM"
            },
            {
              type: "text",
              text: "asdasd@gmail.com"
            }
          ]
        ]
      }
    }
  }
}

module.exports.detail =  {
  ui: {
    "success": true,
    "errorMessage": null,
    "payload": {
      "dateTimeNAString": "N/A",
      "general": {
        customerName: 'customerName',
        customerId: 'customerId',
        orderId: 'orderId'
      },
      "commonInfo": {
        status: {
          title: 'Status',
          value: 'In progress',
          orderHistory: {
            label: 'View Order History',
            url: 'http://localhost:3000/api/order/detail/history'
          }
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
          "customerName": "Andrei Fidelman",
          "company": "Actum",
          "address1": "4001 Valley Industrial Blvd",
          "address2": "4002 Valley Industrial Blvd",
          "city": "Shakopee",
          "state": "MN",
          "zip": "55379",
          "county": "CZ",
          "phone": "+420 664 234 254",
          "email": "andrei.fidelman@actum.cz"
        },
        "trackingPrefix": "Track your packages",
        "tracking": null
      },
      "paymentInfo": {
        "title": "Payment",
        "paymentIcon": "credit-card",
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
            "SKUId": "SKUId-1",
            "removed": false,
            "removeLabel": "Rejected",
            "lineNumber": "lineNumber-1",
            "isReport": true,
            "image": "http://satyr.io/200-500x300-700",
            "template": "Information letter lamp post Mar 30 3018",
            "mailingList": "Mailing",
            "shippingDate": "2017-08-07T09:12:08.108892Z",
            "trackingPrefix": "Tracking ID",
            "tracking": [],
            "mailingListPrefix": "Mailing list",
            "shippingDatePrefix": "Shipping date",
            "templatePrefix": "Template",
            "price": "$ 112.2",
            "quantityPrefix": "Quantity:", // Addresses/Quantity,
            "quantityShippedPrefix": "Quantity shipped:",
            "quantityShipped": 22,
            "quantity": 11,
            "unitOfMeasure": "pc",
            "downloadPdfURL": "#",
            "options": [
              {
                "name": "Color",
                "value": "Red"
              },
              {
                "name": "Size",
                "value": "S"
              }
            ],
            "preview": {
              "exists": true,
              "text": "Preview",
              "url": "#"
            },
            "emailProof": {
              "exists": true,
              "text": "Email",
              "url": "2"
            }
          },
          {
            "id": 2,
            "SKUId": "SKUId-2",
            "removed": true,
            "removeLabel": "Rejected",
            "lineNumber": "lineNumber-2",
            "isReport": true,
            "image": "http://satyr.io/200-500x300-700",
            "template": "Information letter lamp post Mar 30 3017",
            "mailingList": "some mailingList",
            "shippingDate": "2017-08-07T09:12:08.108892Z",
            "trackingPrefix": "Tracking ID",
            "tracking": [
              {
                "id": "501213",
                "url": "#"
              },
              {
                "id": "5012114",
                "url": "#"
              },
              {
                "id": "5014214",
                "url": "#"
              },
              {
                "id": "5012214",
                "url": "#"
              },
              {
                "id": "5041214",
                "url": "#"
              },
              {
                "id": "5051214",
                "url": "#"
              },
              {
                "id": "5016214"
              },
              {
                "id": "501214-501214-501214",
                "url": "#"
              },
              {
                "id": "501214501214"
              },
              {
                "id": "50122214",
                "url": "#"
              }
            ],
            "mailingListPrefix": "Mailing list",
            "shippingDatePrefix": "Shipping date",
            "templatePrefix": "Template",
            "price": "$ 112.2",
            "quantityPrefix": "Quantity:", // Addresses/Quantity,
            "quantityShippedPrefix": "Quantity shipped:",
            "quantityShipped": 22,
            "quantity": 11,
            "unitOfMeasure": "pc",
            "downloadPdfURL": "#",
            "options": [
              {
                "name": "Color",
                "value": "Blue"
              }
            ],
            "preview": {
              "exists": false,
              "text": "Preview",
              "url": "#"
            },
            "emailProof": {
              "exists": true,
              "text": "Email",
              "url": "2"
            }
          },
          {
            "id": 3,
            "SKUId": "SKUId-3",
            "removed": false,
            "removeLabel": "Rejected",
            "lineNumber": "lineNumber-3",
            "isReport": true,
            "image": "http://satyr.io/200-500x300-700",
            "template": "Product with null mailingList property",
            "mailingList": null,
            "shippingDate": "2017-08-07T09:12:08.108892Z",
            "trackingPrefix": "Tracking ID",
            "tracking": [
              {
                "id": "501213",
                "url": "#"
              },
              {
                "id": "5012114",
                "url": "#"
              },
              {
                "id": "5014214",
                "url": "#"
              },
              {
                "id": "5012214",
                "url": "#"
              },
              {
                "id": "5041214",
                "url": "#"
              },
              {
                "id": "5051214",
                "url": "#"
              },
              {
                "id": "5016214"
              },
              {
                "id": "501214-501214-501214",
                "url": "#"
              },
              {
                "id": "501214501214"
              },
              {
                "id": "50122214",
                "url": "#"
              }
            ],
            "mailingListPrefix": "Mailing list",
            "shippingDatePrefix": "Shipping date",
            "templatePrefix": "Template",
            "price": "$ 112.2",
            "quantityPrefix": "Quantity:", // Addresses/Quantity,
            "quantityShippedPrefix": "Quantity shipped:",
            "quantityShipped": 22,
            "quantity": 11,
            "unitOfMeasure": "pc",
            "downloadPdfURL": "#",
            "options": [
              {
                "name": "Color",
                "value": "Blue"
              }
            ],
            "preview": {
              "exists": false,
              "text": "Preview",
              "url": "#"
            },
            "emailProof": {
              "exists": true,
              "text": "Email",
              "url": "2"
            }
          }
        ]
      },
      "actions": {
        "accept": {
          "button": "Accept Order",
          "proceedUrl": "http://localhost:3000/api/order/detail/accept"
        },
        "reject": {
          "button": "Reject Order",
          "proceedUrl": "http://localhost:3000/api/order/detail/reject",
          "dialog": {
            "title": "Are you sure?",
            "cancelButton": "Cancel",
            "proceedButton": "Reject Order"
          }
        },
        "comment": {
          "title": "Comment"
        }
      },
      "editOrders": {
        "button": "Edit",
        "proceedUrl": "http://localhost:3000/api/order/edit",
        "dialog": {
          title: "Title",
          description: "Description",
          validationMessage: "Maximum quantity is", // no space
          successMessage: "Cool! 🚀",
          buttons: {
            proceed: "Save Edits",
            cancel: "Cancel",
            remove: "Remove"
          }
        }
      }
    }
  },
  accept: {
    success: true,
    payload: {
      title: 'Yeahoo! 🚀',
      text: 'The order has been accepted',
      newStatus: 'Approved'
    },
    errorMessage: ''
  },
  reject: {
    success: true,
    payload: {
      title: 'Yeahoo! 😡',
      text: 'The order has been rejected',
      newStatus: 'Rejected'
    },
    errorMessage: ''
  },
  history: {
    success: true,
    errorMessage: '',
    payload: {
      title: "Order History",
      message: {
        title: "Message",
        text: "Lorem ipsum dolor sit, amet consectetur adipisicing elit. Nulla porro quos adipisci dolor aliquam odit accusamus hic, voluptatibus sapiente magnam, quidem explicabo maxime labore ea? Sequi esse culpa dolorem hic!"
      },
      itemChanges: {
        title: "Item Changes",
        headers: ['Item Desc.', 'Chage Type', 'Original', 'New', 'Date', 'User'],
        items: [
          [
            {
              type: "text",
              text: "Item Name #1"
            },
            {
              type: "text",
              text: "Qty Change"
            },
            {
              type: "text",
              text: "20"
            },
            {
              type: "text",
              text: "10"
            },
            {
              type: "date",
              text: "2017-08-07T09:12:08.108892Z"
            },
            {
              type: "text",
              text: "asdasd@gmail.com"
            }
          ],
          [
            {
              type: "text",
              text: "Item Name #1"
            },
            {
              type: "text",
              text: "Qty Change"
            },
            {
              type: "link",
              text: "View Proof",
              url: "#"
            },
            {
              type: "link",
              text: "View Proof",
              url: "#"
            },
            {
              type: "date",
              text: "2017-08-07T15:12:08.108892Z"
            },
            {
              type: "text",
              text: "asdasd@gmail.com"
            }
          ]
        ]
      },
      orderChanges: {
        title: "Order Changes",
        headers: ['Category', 'Original', 'New', 'Date', 'User'],
        items: [
          [
            {
              type: "text",
              text: "Shipping"
            },
            {
              type: "text",
              text: "Overnight"
            },
            {
              type: "text",
              text: "Order: #1450"
            },
            {
              type: "text",
              text: "05/29/2018 10:35:20 AM"
            },
            {
              type: "text",
              text: "asdasd@gmail.com"
            }
          ]
        ]
      }
    }
  }
};
