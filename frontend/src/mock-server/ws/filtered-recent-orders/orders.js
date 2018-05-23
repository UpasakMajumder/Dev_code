module.exports = {
  success: true,
  payload: {
    headers: [
      "Order Date",
      "Order ID",
      "Distributor",
      "Status",
      "Tracking number",
      "View"
    ],
    rows: [
      {
        dialog: {
            orderId: {
              label: 'Order Id',
              value: '38764'
            },
            distributor: {
              label: 'Distributor',
              value: 'John Gold'
            },
            pdf: {
              label: 'Open in PDF',
              value: '#'
            },
            table: {
              headers: [
                "Pos Number",
                "Product Name",
                "Quantiry",
                "Price"
              ],
              rows: [
                [
                  {
                    value: "50170202"
                  },
                  {
                    value: "product1"
                  },
                  {
                    value: "2"
                  },
                  {
                    value: "$20"
                  }
                ],
                [
                  {
                    value: "Business Unit",
                    span: 3
                  },
                  {
                    value: "50000123"
                  }
                ]
              ]
            }
        },
        items: [
          {
            value: "2017-02-01T09:12:08.108892Z"
          },
          {
            value: "3874"
          },
          {
            value: "John Gold"
          },
          {
            value: "Processed"
          },
          {
            value: "Track via FedEx",
            type: "longtext",
            url: "#"
          },
          {
            value: "View",
            type: "button"
          }
        ]
      }
    ]
  },
  errorMessage: null
}
