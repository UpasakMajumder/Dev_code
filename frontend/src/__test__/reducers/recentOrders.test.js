import reducer from '../../app/reducers/recentOrders';
import configureStore from '../../app/store';
import { GET_RECENT_ORDERS_HEADINGS_SUCCESS, GET_RECENT_ORDERS_ROWS_SUCCESS } from '../../app/constants';
import { headings, pagination, rows } from '../../app/testServices/recentOrders';

describe('recentOrders reducer', () => {
  const defaultState = {
    headings: [],
    pagination: {},
    rows: {}
  };

  test('Init heading and pagination', () => {
    const action = {
      type: GET_RECENT_ORDERS_HEADINGS_SUCCESS,
      payload: {
        headings: headings.headings,
        pagination: pagination.pagination
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
      pagination: {
        "pagesCount": 10,
        "rowsCount": 190,
        "rowsOnPages": 20
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
          0: rows.rows
        }
      }
    };

    const actual = reducer(defaultState, action);

    const expected = {
      headings: [],
      pagination: {},
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
            0: rows.rows
          }
        }
      },
      {
        type: GET_RECENT_ORDERS_ROWS_SUCCESS,
        payload: {
          rows: {
            3: rows.rows
          }
        }
      }
    ];

    const store = configureStore();

    actions.forEach(action => store.dispatch(action));

    const actual = store.getState().recentOrders;

    const expected = {
      headings: [],
      pagination: {},
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
        }]
      }
    };

    expect(actual).toEqual(expected);
  });
});
