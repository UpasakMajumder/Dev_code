module.exports.ui = {
  "success": true,
  "payload": {
    "emptyCart":{
      "text": "Your shopping cart is empty.",
      "dashboardButtonText": "Go to dashboard",
      "dashboardButtonUrl": "/dashboard",
      "productsButtonText" : "See products",
      "productsButtonUrl": "/products"
    },
    "products": {
      "summaryPrice": {
        "pricePrefix": "$",
        "price": "10000.0"
      },
      "number": "You have 1 item in your shopping cart",
      "productionTimeLabel": "Production time",
      "shipTimeLabel": "Shipping time",
      "buttonLabels": {
        "edit": "Edit",
        "remove": "Remove"
      },
      "items": [
        {
          "id": 33,
          "image": "",
          "template": "Template",
          "isMailingList": false,
          "mailingList": "Mailing list",
          "delivery": "",
          "pricePrefix": "$",
          "price": "112.2",
          "isEditable": true,
          "editorURL": "#",
          "quantityPrefix": "Quantity:",
          "quantity": 11,
          "stockQuantity": 15,
          "isQuantityEditable": true,
          "productionTime": "Now",
          "shipTime": "Tomorrow"
        },
        {
          "id": 34,
          "image": "http://satyr.io/100-200x400-500?2",
          "template": "Template",
          "isMailingList": false,
          "mailingList": "Mailing list",
          "delivery": "",
          "pricePrefix": "$",
          "price": "112.2",
          "isEditable": true,
          "editorURL": "#",
          "quantityPrefix": "Quantity:",
          "quantity": 11,
          "stockQuantity": 15,
          "isQuantityEditable": true,
          "productionTime": "",
          "shipTime": "Tomorrow"
        }
      ]
    },
    "deliveryAddresses": {
      "isDeliverable": true,
      "availableToAdd": true,
      "bounds": {
        "showMoreText": "Show more",
        "showLessText": "Show less",
        "limit": 3
      },
      "dialogUI": {
        "title": "New address",
        "saveAddressCheckbox": "Save in settings",
        "discardBtnLabel": "Discard changes",
        "submitBtnLabel": "Confirm & use address",
        "requiredErrorMessage": "The field is required",
        "fields": [
          {
            "id": "customerName",
            "label": "Customer Name",
            "values": [],
            "type": "text"
          },
          {
            "id": "address1",
            "label": "Address 1",
            "values": [],
            "type": "text"
          },
          {
            "id": "address2",
            "label": "Address 2",
            "values": [],
            "type": "text",
            "isOptional": true
          },
          {
            "id": "city",
            "label": "City",
            "values": [],
            "type": "text"
          },
          {
            "id": "zip",
            "label": "Postal Code",
            "values": [],
            "type": "text"
          },
          {
            "id": "state",
            "label": "State",
            "values": [],
            "type": "select"
          },
          {
            "id": "country",
            "label": "Country",
            "type": "select",
            "values": [
              {
                id: "1",
                name: "Czech Republic",
                values: []
              },
              {
                id: "2",
                name: "Japan",
                values: []
              },
              {
                id: "3",
                name: "USA",
                isDefault: true,
                values: [
                  {
                    id: "31",
                    name: "AK",
                  },
                  {
                    id: "32",
                    name: "AL"
                  }
                ]
              }
            ],
          },
          {
            "id": "phone",
            "label": "Phone",
            "values": [],
            "type": "text",
            "isOptional": true
          },
          {
            "id": "email",
            "label": "Email",
            "values": [],
            "type": "text"
          }
        ]
      },
      "unDeliverableText": "Text",
      "title": "Delivery address",
      "description": "Products will be delivered to selected address by",
      "newAddress": {
        "label": "New address",
        "url": "#?tab=t4"
      },
      "emptyMessage": "Fill at least one address",
      "items": [
        {
          "address1": "Test Address line 1",
          "address2": "Test Address line 2",
          "city": "New York",
          "state": "31",
          "zip": "38017",
          "id": 1,
          "checked": true,
          "country": "3"
        },
        {
          "address1": "Test Address line 1",
          "address2": "Test Address line 2",
          "city": "Tokyo",
          "state": "",
          "zip": "13228",
          "id": 3,
          "checked": false,
          "country": "2"
        },
        {
          "address1": "Test Address line 1",
          "address2": "Test Address line 2",
          "city": "Prague",
          "state": "",
          "zip": "14000",
          "id": 4,
          "checked": false,
          "country": "1"
        }
      ]
    },
    "paymentMethods": {
      "isPayable": true,
      "unPayableText": "Text 2",
      "title": "Payment",
      "description": null,
      "items": [
        {
          "id": 1,
          "title": "Credit card",
          "icon": "credit-card",
          "disabled": false,
          "checked": true,
          "hasInput": false,
          "inputPlaceholder": null,
          "items": [
            {
              "id": 0,
              "label": "John Gold 2355",
              "checked": false
            },
            {
              "id": 1,
              "label": "John Travolta 1263",
              "checked": false
            },
            {
              "id": 2,
              "label": "John Gold 2112",
              "checked": false
            },
            {
              "id": 3,
              "label": "New card",
              "checked": true
            }
          ]
        },
        {
          "id": 2,
          "title": "Pay Pal",
          "icon": "paypal-payment",
          "disabled": true,
          "checked": false,
          "hasInput": false,
          "inputPlaceholder": null,
          "items": []
        },
        {
          "id": 3,
          "title": "Purchase order",
          "icon": "order-payment",
          "disabled": false,
          "checked": false,
          "hasInput": true,
          "inputPlaceholder": "Insert your PO number",
          "items": []
        }
      ]
    },
    "submit": {
      "isDisabled": true,
      "btnLabel": "Place order",
      "disabledText": "Order cannot be submited until all templated products receive design file from Chilli"
    },
    "emailConfirmation": {
      "exists": true,
      "maxItems": 3,
      "tooltipText": {
        "add": "Add email address",
        "remove": "Remove email address"
      },
      "title": "Title",
      "description": "Description"
    },
    "validationMessage": "Error"
  },
  "errorMessage": null
};

module.exports.totalPrice = {
  "success": true,
  "payload": {
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
          "price": "$ 11.40",
          "datePrefix": null,
          "date": null,
          "items": [
            {
              "id": 4,
              "title": "Ground",
              "checked": false,
              "pricePrefix": "Price",
              "price": "$ 11.40",
              "datePrefix": null,
              "date": null,
              "disabled": false
            },
            {
              "id": 7,
              "title": "NextDayStd",
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
          "pricePrefix": "$ 14.00",
          "price": "",
          "datePrefix": "Tomorrow",
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
    "totals": {
      "title": "Totals",
      "description": null,
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
    }
  },
  "errorMessage": null
};

// export const newAddress = {
//   "street": [
//     "Test Address line 1",
//     "Test Address line 2"
//   ],
//   "city": "COLLIERVILLE",
//   "state": "TN",
//   "zip": "38017",
//   "id": 12312414,
//   "customerName": "Andrew",
//   "country": "USA",
//   "phone": "+1251512125",
//   "email": "fake@user.com"
// };
//
// export const staticUI2 = {
//   "success": true,
//   "payload": {
//     "products": {
//       "summaryPrice": {
//         "pricePrefix": "$",
//         "price": "10.0"
//       },
//       "number": "You have 1 item in your shopping cart",
//       "items": [
//         {
//           "id": 34,
//           "image": "http://satyr.io/100-200x400-500?2",
//           "template": "Template",
//           "isMailingList": false,
//           "mailingList": "Mailing list",
//           "delivery": "",
//           "pricePrefix": "$",
//           "price": "112.2",
//           "isEditable": true,
//           "editorURL": "#",
//           "quantityPrefix": "Quantity:",
//           "quantity": 11,
//           "stockQuantity": 15,
//           "isQuantityEditable": true
//         }
//       ]
//     },
//     "deliveryAddresses": {
//       "isDeliverable": true,
//       "unDeliverableText": "Text",
//       "title": "Delivery address",
//       "description": "Products will be delivered to selected address by",
//       "newAddress": {
//         "label": "New address",
//         "url": "#?tab=t4"
//       },
//       "emptyMessage": "Fill at least one address",
//       "items": [
//         {
//           "street": [
//             "Test Address line 1"
//           ],
//           "city": "COLLIERVILLE",
//           "state": "TN",
//           "zip": "38017",
//           "id": 1,
//           "checked": true
//         },
//         {
//           "street": [
//             "House number 123"
//           ],
//           "city": "Red Watter Village",
//           "state": "",
//           "zip": "56161",
//           "id": 2,
//           "checked": false
//         },
//         {
//           "street": [
//             "Sun street 1"
//           ],
//           "city": "Tokyo",
//           "state": "",
//           "zip": "13228",
//           "id": 3,
//           "checked": false
//         },
//         {
//           "street": [
//             "Actum Hyper Company"
//           ],
//           "city": "Prague",
//           "state": "",
//           "zip": "14000",
//           "id": 4,
//           "checked": false
//         }
//       ]
//     },
//     "paymentMethods": {
//       "isPayable": true,
//       "unPayableText": "Text 2",
//       "title": "Payment",
//       "description": null,
//       "items": [
//         {
//           "id": 1,
//           "title": "Credit card",
//           "icon": "credit-card",
//           "disabled": true,
//           "checked": false,
//           "hasInput": false,
//           "inputPlaceholder": null
//         },
//         {
//           "id": 2,
//           "title": "Pay Pal",
//           "icon": "paypal-payment",
//           "disabled": true,
//           "checked": false,
//           "hasInput": false,
//           "inputPlaceholder": null
//         },
//         {
//           "id": 3,
//           "title": "Purchase order",
//           "icon": "order-payment",
//           "disabled": false,
//           "checked": true,
//           "hasInput": true,
//           "inputPlaceholder": "Insert your PO number"
//         }
//       ]
//     },
//     "submit": {
//       "isDisabled": true,
//       "btnLabel": "Place order",
//       "disabledText": "Order cannot be submited until all templated products receive design file from Chilli"
//     },
//     "validationMessage": "Error"
//   },
//   "errorMessage": null
// };
//
// export const completeUI = {
//   "success": true,
//   "payload": {
//     "deliveryMethods": {
//       "title": "Delivery method",
//       "description": "Select delivery carrier and option",
//       "items": [
//         {
//           "id": 1,
//           "title": "FedEx",
//           "icon": "fedex-delivery",
//           "opened": false,
//           "disabled": false,
//           "pricePrefix": "Cannot be delivered",
//           "price": "",
//           "datePrefix": null,
//           "date": null,
//           "items": [
//             {
//               "id": 5,
//               "title": "Int'l Economy",
//               "checked": false,
//               "pricePrefix": "Cannot be delivered",
//               "price": "",
//               "datePrefix": null,
//               "date": null,
//               "disabled": true
//             },
//             {
//               "id": 6,
//               "title": "Int'l Ground",
//               "checked": false,
//               "pricePrefix": "Cannot be delivered",
//               "price": "",
//               "datePrefix": null,
//               "date": null,
//               "disabled": true
//             }
//           ]
//         },
//         {
//           "id": 2,
//           "title": "FedEx Customer",
//           "icon": "fedex-delivery",
//           "opened": true,
//           "disabled": false,
//           "pricePrefix": "Price based on your contract",
//           "price": "",
//           "datePrefix": null,
//           "date": null,
//           "items": [
//             {
//               "id": 2,
//               "title": "FedEx customer price",
//               "checked": true,
//               "pricePrefix": "Price based on your contract",
//               "price": "",
//               "datePrefix": null,
//               "date": null,
//               "disabled": false
//             }
//           ]
//         },
//         {
//           "id": 3,
//           "title": "UPS",
//           "icon": "ups-delivery",
//           "opened": false,
//           "disabled": false,
//           "pricePrefix": "Price from",
//           "price": "$ 11.40",
//           "datePrefix": null,
//           "date": null,
//           "items": [
//             {
//               "id": 4,
//               "title": "Ground",
//               "checked": false,
//               "pricePrefix": "Price",
//               "price": "$ 11.40",
//               "datePrefix": null,
//               "date": null,
//               "disabled": false
//             },
//             {
//               "id": 7,
//               "title": "NextDayStd",
//               "checked": false,
//               "pricePrefix": "Cannot be delivered",
//               "price": "",
//               "datePrefix": null,
//               "date": null,
//               "disabled": true
//             }
//           ]
//         },
//         {
//           "id": 4,
//           "title": "UPS Customer",
//           "icon": "ups-delivery",
//           "opened": false,
//           "disabled": false,
//           "pricePrefix": "Price based on your contract",
//           "price": "",
//           "datePrefix": null,
//           "date": null,
//           "items": [
//             {
//               "id": 8,
//               "title": "UPS customer price",
//               "checked": false,
//               "pricePrefix": "Price based on your contract",
//               "price": "",
//               "datePrefix": null,
//               "date": null,
//               "disabled": false
//             }
//           ]
//         },
//         {
//           "id": 5,
//           "title": "USPS",
//           "icon": "usps-delivery",
//           "opened": false,
//           "disabled": false,
//           "pricePrefix": "Cannot be delivered",
//           "price": "",
//           "datePrefix": null,
//           "date": null,
//           "items": [
//             {
//               "id": 1,
//               "title": "1st class",
//               "checked": false,
//               "pricePrefix": "Cannot be delivered",
//               "price": "",
//               "datePrefix": null,
//               "date": null,
//               "disabled": true
//             },
//             {
//               "id": 3,
//               "title": "First Class",
//               "checked": false,
//               "pricePrefix": "Cannot be delivered",
//               "price": "",
//               "datePrefix": null,
//               "date": null,
//               "disabled": true
//             }
//           ]
//         },
//         {
//           "id": 6,
//           "title": "USPS Customer",
//           "icon": "usps-delivery",
//           "opened": false,
//           "disabled": false,
//           "pricePrefix": "Price based on your contract",
//           "price": "",
//           "datePrefix": null,
//           "date": null,
//           "items": [
//             {
//               "id": 9,
//               "title": "USPS customer price",
//               "checked": false,
//               "pricePrefix": "Price based on your contract",
//               "price": "",
//               "datePrefix": null,
//               "date": null,
//               "disabled": false
//             }
//           ]
//         }
//       ]
//     },
//     "totals": {
//       "title": "Totals",
//       "description": null,
//       "items": [
//         {
//           "title": "Summary",
//           "value": "$ 112.20"
//         },
//         {
//           "title": "Shipping",
//           "value": "$ 0.00"
//         },
//         {
//           "title": "Subtotal",
//           "value": "$ 112.20"
//         },
//         {
//           "title": "Tax",
//           "value": "$ 0.00"
//         },
//         {
//           "title": "Totals",
//           "value": "$ 112.20"
//         }
//       ]
//     },
//     "products": {
//       "number": "You have 1 item in your shopping cart",
//       "items": [
//         {
//           "id": 33,
//           "image": "",
//           "template": "Template",
//           "isMailingList": false,
//           "mailingList": "Mailing list",
//           "delivery": "",
//           "pricePrefix": "$",
//           "price": "112.2",
//           "isEditable": true,
//           "editorURL": "#",
//           "quantityPrefix": "Quantity:",
//           "quantity": 11,
//           "stockQuantity": 15,
//           "isQuantityEditable": true
//         },
//         {
//           "id": 34,
//           "image": "http://satyr.io/100-200x400-500?2",
//           "template": "Template",
//           "isMailingList": false,
//           "mailingList": "Mailing list",
//           "delivery": "",
//           "pricePrefix": "$",
//           "price": "112.2",
//           "isEditable": true,
//           "editorURL": "#",
//           "quantityPrefix": "Quantity:",
//           "quantity": 11,
//           "stockQuantity": 15,
//           "isQuantityEditable": true
//         }
//       ]
//     },
//     "deliveryAddresses": {
//       "isDeliverable": true,
//       "unDeliverableText": "Text",
//       "title": "Delivery address",
//       "description": "Products will be delivered to selected address by",
//       "newAddress": {
//         "label": "New address",
//         "url": "#?tab=t4"
//       },
//       "emptyMessage": "Fill at least one address",
//       "items": [
//         {
//           "street": [
//             "Test Address line 1"
//           ],
//           "city": "COLLIERVILLE",
//           "state": "TN",
//           "zip": "38017",
//           "id": 1,
//           "checked": true
//         },
//         {
//           "street": [
//             "House number 123"
//           ],
//           "city": "Red Watter Village",
//           "state": "",
//           "zip": "56161",
//           "id": 2,
//           "checked": false
//         },
//         {
//           "street": [
//             "Sun street 1"
//           ],
//           "city": "Tokyo",
//           "state": "",
//           "zip": "13228",
//           "id": 3,
//           "checked": false
//         },
//         {
//           "street": [
//             "Actum Hyper Company"
//           ],
//           "city": "Prague",
//           "state": "",
//           "zip": "14000",
//           "id": 4,
//           "checked": false
//         }
//       ]
//     },
//     "paymentMethods": {
//       "isPayable": true,
//       "unPayableText": "Text 2",
//       "title": "Payment",
//       "description": null,
//       "items": [
//         {
//           "id": 1,
//           "title": "Credit card",
//           "icon": "credit-card",
//           "disabled": true,
//           "checked": false,
//           "hasInput": false,
//           "inputPlaceholder": null
//         },
//         {
//           "id": 2,
//           "title": "Pay Pal",
//           "icon": "paypal-payment",
//           "disabled": true,
//           "checked": false,
//           "hasInput": false,
//           "inputPlaceholder": null
//         },
//         {
//           "id": 3,
//           "title": "Purchase order",
//           "icon": "order-payment",
//           "disabled": false,
//           "checked": true,
//           "hasInput": true,
//           "inputPlaceholder": "Insert your PO number"
//         }
//       ]
//     },
//     "submit": {
//       "isDisabled": true,
//       "btnLabel": "Place order",
//       "disabledText": "Order cannot be submited until all templated products receive design file from Chilli"
//     },
//     "validationMessage": "Error"
//   },
//   "errorMessage": null
// };
