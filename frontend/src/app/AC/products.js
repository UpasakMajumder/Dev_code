import axios from 'axios';
/* constants */
import { FETCH, SUCCESS, FAILURE, START, FINISH,
  PRODUCTS_LOAD, PRODUCTS_FAVORITE_LOAD, PRODUCT_MARK_AS_FAVOURITE, PRODUCT_UNMARK_AS_FAVOURITE
} from 'app.consts';
/* helpers */
import { callAC } from 'app.helpers/ac';
/* globals */
import { PRODUCTS as PRODUCTS_GLOBAL } from 'app.globals';

export const markProductFavourite = (productId) => {
  return (dispatch) => {
    dispatch({
      type: PRODUCT_MARK_AS_FAVOURITE,
      id: productId
    });

    axios.put(`${PRODUCTS_GLOBAL.markProductFavouriteUrl}/${productId}`)
      .catch(console.error); // eslint-disable-line no-console
  };
};

export const unmarkProductFavourite = (productId) => {
  return (dispatch) => {
    dispatch({
      type: PRODUCT_UNMARK_AS_FAVOURITE,
      id: productId
    });

    axios.put(`${PRODUCTS_GLOBAL.unmarkProductFavouriteUrl}/${productId}`)
      .catch(console.error); // eslint-disable-line no-console
  };
};

export const loadProducts = () => {
  return (dispatch) => {
    dispatch({ type: PRODUCTS_LOAD + FETCH });

    axios.get(`${PRODUCTS_GLOBAL.loadProductsUrl}?url=${PRODUCTS_GLOBAL.nodeAliasPath}`)
      .then((response) => {
        const { payload, success, errorMessage } = response.data;

        if (!success) {
          dispatch({
            type: PRODUCTS_LOAD + FAILURE,
            alert: errorMessage
          });
          return;
        }

        dispatch({
          type: PRODUCTS_LOAD + SUCCESS,
          payload: {
            ui: payload
          }
        });
      })
      .catch((error) => {
        dispatch({ type: PRODUCTS_LOAD + FAILURE });
      });
  };
};


export const loadFavoritesProducts = () => {
  return (dispatch) => {
    dispatch({ type: PRODUCTS_FAVORITE_LOAD + FETCH });

    axios.get(PRODUCTS_GLOBAL.loadFavoritesProductsUrl)
      .then((response) => {
        const { payload, success, errorMessage } = response.data;

        if (!success) {
          dispatch({
            type: PRODUCTS_FAVORITE_LOAD + FAILURE,
            alert: errorMessage
          });
          return;
        }

        dispatch({
          type: PRODUCTS_FAVORITE_LOAD + SUCCESS,
          payload: {
            ui: payload
          }
        });
      })
      .catch((error) => {
        dispatch({ type: PRODUCTS_FAVORITE_LOAD + FAILURE });
      });
  };
};
