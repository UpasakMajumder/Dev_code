const address = {
  ui: {
    "success": true,
    "errorMessage": null,
    "payload": {
      "billing": {},
      "shipping": {
        "title": "Shipping addresses",
        "allowAddresses": 3,
        "addButton": {
          "text": "Add new shipping address",
          "exists": true,
        },
        "editButton": {
          "text": "Edit",
          "exists": true,
        },
        "removeButton": {
          "text": "Remove",
          "exists": false,
        },
        "addresses": [
          {
            "id": 1,
            "street1": "4001 Valley Industrial Blvd",
            "street2": "",
            "city": "Shakopee",
            "state": "MN",
            "zip": "55379"
          },
          {
            "id": 2,
            "street1": "4001 Valley Industrial Blvd",
            "street2": "",
            "city": "Shakopee",
            "state": "MN",
            "zip": "55379"
          }
        ]
      },
      "dialog": {
        "types": {
          "add": "Add address:",
          "edit": "Edit address:"
        },
        "buttons": {
          "discard": "Discard changes",
          "save": "Save address"
        },
        "fields": [
          {
            "id": "street1",
            "label": "Address line 1",
            "values": [],
            "type": "text"
          },
          {
            "id": "street2",
            "label": "Address line 2",
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
            "id": "state",
            "label": "State",
            "values": ["AK",
              "AL",
              "AR",
              "AZ",
              "CA",
              "CO",
              "CT",
              "DC",
              "DE",
              "FL",
              "GA",
              "HI",
              "IA",
              "ID",
              "IL",
              "IN",
              "KS",
              "KY",
              "LA",
              "MA",
              "MD",
              "ME",
              "MI",
              "MN",
              "MO",
              "MS",
              "MT",
              "NC",
              "ND",
              "NE",
              "NH",
              "NJ",
              "NM",
              "NV",
              "NY",
              "OH",
              "OK",
              "OR",
              "PA",
              "RI",
              "SC",
              "SD",
              "TN",
              "TX",
              "UT",
              "VA",
              "VT",
              "WA",
              "WI",
              "WV",
              "WY"],
            "type": "select"
          },
          {
            "id": "zip",
            "label": "Zip code",
            "values": [],
            "type": "text"
          }
        ]
      }
    }
  }
};

module.exports.address = address;
