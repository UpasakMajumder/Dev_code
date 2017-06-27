import reducer from '../../app/reducers/recentOrders';
import configureStore from '../../app/store';
import { GET_RECENT_ORDERS_HEADINGS_SUCCESS, GET_RECENT_ORDERS_ROWS_SUCCESS } from '../../app/constants';
import { headings, pageInfo, rows1 } from '../../app/testServices/recentOrders';

describe('recentOrders reducer', () => {
  const defaultState = {
    headings: [],
    pageInfo: {},
    rows: {}
  };

  test('Init heading and pageInfo', () => {
    const action = {
      type: GET_RECENT_ORDERS_HEADINGS_SUCCESS,
      payload: {
        headings: headings.headings,
        pageInfo: pageInfo.pageInfo
      }
    };

    const actual = reducer(defaultState, action);

    const expected = {
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
        "rowsOnPages": 3
      },
      rows: {}
    };

    expect(actual).toEqual(expected);
  });

  test('Add rows to 0 page', () => {
    const action = {
      type: GET_RECENT_ORDERS_ROWS_SUCCESS,
      payload: {
        rows: {
          0: rows1.payload.rows
        }
      }
    };

    const actual = reducer(defaultState, action);

    const expected = {
      headings: [],
      pageInfo: {},
      rows: {
        0: [{
          "orderNumber": "0010-00162-17-фывто123",
          "orderDate": "2017-06-23 13:36:36",
          "items": [
            {
              "name": "Product 3",
              "quantity": 1
            }
          ],
          "orderStatus": "Rejected",
          "deliveryDate": "0001-01-01 00:00:00",
          "viewBtn": {
            "text": "View",
            "url": "~/recent-orders/order-detail?orderID=0010-00162-17-00004"
          }
        },
        {
          "orderNumber": "0001-00091-17-324фыв",
          "orderDate": "2017-06-21 13:17:36",
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
          "deliveryDate": "0001-01-01 00:00:00",
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
          "deliveryDate": "0001-01-01 00:00:00",
          "viewBtn": {
            "text": "View",
            "url": "~/recent-orders/order-detail?orderID=0010-00184-17-00009"
          }
        },
        {
          "orderNumber": "0001-00174-17-фывт123",
          "orderDate": "2017-06-21 14:16:24",
          "items": [
            {
              "name": "Inventory",
              "quantity": 1
            }
          ],
          "orderStatus": null,
          "deliveryDate": "0001-01-01 00:00:00",
          "viewBtn": {
            "text": "View",
            "url": "~/recent-orders/order-detail?orderID=0001-00174-17-00001"
          }
        },
        {
          "orderNumber": "0001-00174-17-121",
          "orderDate": "2017-06-23 13:26:16",
          "items": [
            {
              "name": "Inventory",
              "quantity": 1
            }
          ],
          "orderStatus": "Rejected",
          "deliveryDate": "0001-01-01 00:00:00",
          "viewBtn": {
            "text": "View",
            "url": "~/recent-orders/order-detail?orderID=0001-00174-17-00012"
          }
        }]
      }
    };

    expect(actual).toEqual(expected);
  });

  test('Add rows to 0 and 3 pages', () => {
    const actions = [
      {
        type: GET_RECENT_ORDERS_ROWS_SUCCESS,
        payload: {
          rows: {
            0: rows1.payload.rows
          }
        }
      },
      {
        type: GET_RECENT_ORDERS_ROWS_SUCCESS,
        payload: {
          rows: {
            3: rows1.payload.rows
          }
        }
      }
    ];

    const store = configureStore();

    actions.forEach(action => store.dispatch(action));

    const actual = store.getState().recentOrders;

    const expected = {
      headings: [],
      pageInfo: {},
      noOrdersMessage: "",
      rows: {
        0: [{
          "orderNumber": "0010-00162-17-фывто123",
          "orderDate": "2017-06-23 13:36:36",
          "items": [
            {
              "name": "Product 3",
              "quantity": 1
            }
          ],
          "orderStatus": "Rejected",
          "deliveryDate": "0001-01-01 00:00:00",
          "viewBtn": {
            "text": "View",
            "url": "~/recent-orders/order-detail?orderID=0010-00162-17-00004"
          }
        },
        {
          "orderNumber": "0001-00091-17-324фыв",
          "orderDate": "2017-06-21 13:17:36",
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
          "deliveryDate": "0001-01-01 00:00:00",
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
          "deliveryDate": "0001-01-01 00:00:00",
          "viewBtn": {
            "text": "View",
            "url": "~/recent-orders/order-detail?orderID=0010-00184-17-00009"
          }
        },
        {
          "orderNumber": "0001-00174-17-фывт123",
          "orderDate": "2017-06-21 14:16:24",
          "items": [
            {
              "name": "Inventory",
              "quantity": 1
            }
          ],
          "orderStatus": null,
          "deliveryDate": "0001-01-01 00:00:00",
          "viewBtn": {
            "text": "View",
            "url": "~/recent-orders/order-detail?orderID=0001-00174-17-00001"
          }
        },
        {
          "orderNumber": "0001-00174-17-121",
          "orderDate": "2017-06-23 13:26:16",
          "items": [
            {
              "name": "Inventory",
              "quantity": 1
            }
          ],
          "orderStatus": "Rejected",
          "deliveryDate": "0001-01-01 00:00:00",
          "viewBtn": {
            "text": "View",
            "url": "~/recent-orders/order-detail?orderID=0001-00174-17-00012"
          }
        }],
        3: [{
          "orderNumber": "0010-00162-17-фывто123",
          "orderDate": "2017-06-23 13:36:36",
          "items": [
            {
              "name": "Product 3",
              "quantity": 1
            }
          ],
          "orderStatus": "Rejected",
          "deliveryDate": "0001-01-01 00:00:00",
          "viewBtn": {
            "text": "View",
            "url": "~/recent-orders/order-detail?orderID=0010-00162-17-00004"
          }
        },
        {
          "orderNumber": "0001-00091-17-324фыв",
          "orderDate": "2017-06-21 13:17:36",
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
          "deliveryDate": "0001-01-01 00:00:00",
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
          "deliveryDate": "0001-01-01 00:00:00",
          "viewBtn": {
            "text": "View",
            "url": "~/recent-orders/order-detail?orderID=0010-00184-17-00009"
          }
        },
        {
          "orderNumber": "0001-00174-17-фывт123",
          "orderDate": "2017-06-21 14:16:24",
          "items": [
            {
              "name": "Inventory",
              "quantity": 1
            }
          ],
          "orderStatus": null,
          "deliveryDate": "0001-01-01 00:00:00",
          "viewBtn": {
            "text": "View",
            "url": "~/recent-orders/order-detail?orderID=0001-00174-17-00001"
          }
        },
        {
          "orderNumber": "0001-00174-17-121",
          "orderDate": "2017-06-23 13:26:16",
          "items": [
            {
              "name": "Inventory",
              "quantity": 1
            }
          ],
          "orderStatus": "Rejected",
          "deliveryDate": "0001-01-01 00:00:00",
          "viewBtn": {
            "text": "View",
            "url": "~/recent-orders/order-detail?orderID=0001-00174-17-00012"
          }
        }]
      }
    };

    expect(actual).toEqual(expected);
  });
});
