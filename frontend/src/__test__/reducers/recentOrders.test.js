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
          0: rows1.rows
        }
      }
    };

    const actual = reducer(defaultState, action);

    const expected = {
      headings: [],
      pageInfo: {},
      rows: {
        0: [{
            "orderNumber": 1,
            "orderDate": "10/01/2017",
            "items": [
              "1", "2", "3"
            ],
            "orderStatus": "In rogressing",
            "deliveryDate": "10/05/2017",
            "viewBtn": {
              "text": "View",
              "url": "#"
            }
          },
          {
            "deliveryDate": "10/05/2017",
            "items": [
              "1",
              "2",
              "3"
            ],
            "orderDate": "10/01/2017",
            "orderNumber": 2,
            "orderStatus": "In rogressing",
            "viewBtn": {
              "text": "View",
              "url": "#"
            }
          },
          {
            "deliveryDate": "10/05/2017",
            "items": [
              "1",
              "2",
              "3"
            ],
            "orderDate": "10/01/2017",
            "orderNumber": 3,
            "orderStatus": "In rogressing",
            "viewBtn": {
              "text": "View",
              "url": "#"
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
            0: rows1.rows
          }
        }
      },
      {
        type: GET_RECENT_ORDERS_ROWS_SUCCESS,
        payload: {
          rows: {
            3: rows1.rows
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
      rows: {
        0: [{
            "orderNumber": 1,
            "orderDate": "10/01/2017",
            "items": [
              "1", "2", "3"
            ],
            "orderStatus": "In rogressing",
            "deliveryDate": "10/05/2017",
            "viewBtn": {
              "text": "View",
              "url": "#"
            }
          },
          {
            "deliveryDate": "10/05/2017",
            "items": [
              "1",
              "2",
              "3"
            ],
            "orderDate": "10/01/2017",
            "orderNumber": 2,
            "orderStatus": "In rogressing",
            "viewBtn": {
              "text": "View",
              "url": "#"
            }
          },
          {
            "deliveryDate": "10/05/2017",
            "items": [
              "1",
              "2",
              "3"
            ],
            "orderDate": "10/01/2017",
            "orderNumber": 3,
            "orderStatus": "In rogressing",
            "viewBtn": {
              "text": "View",
              "url": "#"
            }
          }],
        3: [{
            "orderNumber": 1,
            "orderDate": "10/01/2017",
            "items": [
              "1", "2", "3"
            ],
            "orderStatus": "In rogressing",
            "deliveryDate": "10/05/2017",
            "viewBtn": {
              "text": "View",
              "url": "#"
            }
          },
          {
            "deliveryDate": "10/05/2017",
            "items": [
              "1",
              "2",
              "3"
            ],
            "orderDate": "10/01/2017",
            "orderNumber": 2,
            "orderStatus": "In rogressing",
            "viewBtn": {
              "text": "View",
              "url": "#"
            }
          },
          {
            "deliveryDate": "10/05/2017",
            "items": [
              "1",
              "2",
              "3"
            ],
            "orderDate": "10/01/2017",
            "orderNumber": 3,
            "orderStatus": "In rogressing",
            "viewBtn": {
              "text": "View",
              "url": "#"
            }
          }]
      }
    };

    expect(actual).toEqual(expected);
  });
});
