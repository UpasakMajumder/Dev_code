import axios from 'axios';
/* constants */
import { FETCH, SUCCESS, FAILURE, MANAGE_PRODUCTS } from 'app.consts';
/* helpers */
import { callAC } from 'app.helpers/ac';
/* globals */
import { MANAGE_PRODUCTS as MANAGE_PRODUCTS_RESPONSE } from 'app.globals';
/* web service */
import manageProductsResponse from 'app.ws/manageProducts';

// eslint-disable-next-line import/prefer-default-export
export const loadManageProducts = () => {
  return (dispatch) => {
    dispatch({ type: MANAGE_PRODUCTS + FETCH });

    const prod = () => {
      axios({
        method: 'get',
        url: MANAGE_PRODUCTS_RESPONSE.templatesUrl
      }).then((response) => {
        const { payload, success, errorMessage } = response.data;

        if (!success) {
          dispatch({
            type: MANAGE_PRODUCTS + FAILURE,
            alert: errorMessage
          });
        } else {
          dispatch({
            type: MANAGE_PRODUCTS + SUCCESS,
            payload
          });
        }
      }).catch((error) => {
        dispatch({ type: MANAGE_PRODUCTS + FAILURE });
      });
    };

    const dev = () => {
      setTimeout(() => {
        dispatch({
          type: MANAGE_PRODUCTS + SUCCESS,
          payload: manageProductsResponse.payload
        });
      }, 2000);
    };

    callAC(dev, prod);
  };
};
