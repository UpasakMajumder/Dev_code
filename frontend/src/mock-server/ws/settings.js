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
          "exists": true,
        },
        "defaultAddress": {
          "id": 2,
          "labelDefault": "Default",
          "labelNonDefault": "Set as default",
          "tooltip": "Default address is pre-selected on checkout",
          "setUrl": 'http://localhost:3000/api/settings/address/default/set',
          "unsetUrl": 'http://localhost:3000/api/settings/address/default/unset'
        },
        "addresses": [
          {
            "id": 1,
            "address1": "4001 Valley Industrial Blvd",
            "address2": "",
            "city": "Shakopee",
            "state": "22",
            "zip": "55379",
            "country": "2",
            "customerName": "Andrei"
          },
          {
            "id": 2,
            "address1": "4001 Valley Industrial Blvd",
            "address2": "",
            "city": "Shakopee",
            "zip": "55379",
            "country": "1",
            "customerName": "Andrei"
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
            "id": "customerName",
            "label": "Customer Name",
            "values": [],
            "type": "text"
          },
          {
            "id": "company",
            "label": "Company",
            "values": [],
            "type": "text",
            "isOptional": true
          },
          {
            "id": "address1",
            "label": "Address line 1",
            "values": [],
            "type": "text"
          },
          {
            "id": "address2",
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
            "type": "select",
            "values": []
          },
          {
            "id": "zip",
            "label": "Zip code",
            "values": [],
            "type": "text"
          },
          {
            "id": "country",
            "label": "Country",
            "values": [
              {
                id: "1",
                name: "Czech Republic",
                values: []
              },
              {
                id: "2",
                name: "USA",
                isDefault: true,
                values: [
                  {
                    id: "21",
                    name: "AK",
                  },
                  {
                    id: "22",
                    name: "AL"
                  }
                ]
              }
            ],
            "type": "select"
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
      }
    }
  }
};

module.exports.address = address;
