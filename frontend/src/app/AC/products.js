import axios from 'axios';
/* constants */
import { FETCH, SUCCESS, FAILURE, START, FINISH,
  PRODUCTS_LOAD, PRODUCTS_FAVORITE_LOAD, PRODUCT_MARK_AS_FAVOURITE, PRODUCT_UNMARK_AS_FAVOURITE
} from 'app.consts';
/* helpers */
import { callAC } from 'app.helpers/ac';
/* globals */
import { PRODUCTS as PRODUCTS_GLOBAL } from 'app.globals';
/* web service */
import loadProductsResponse from 'app.ws/products';
import loadFavoriteProductsResponse from 'app.ws/productsFavorite';

export const markProductFavourite = (productId) => {
  return (dispatch) => {
    dispatch({
      type: PRODUCT_MARK_AS_FAVOURITE,
      id: productId
    });

    axios.put(`${PRODUCTS_GLOBAL.markProductFavouriteUrl}${productId}`)
      .catch(console.error); // eslint-disable-line no-console
  };
};

export const unmarkProductFavourite = (productId) => {
  return (dispatch) => {
    dispatch({
      type: PRODUCT_UNMARK_AS_FAVOURITE,
      id: productId
    });

    axios.put(`${PRODUCTS_GLOBAL.unmarkProductFavouriteUrl}${productId}`)
      .catch(console.error); // eslint-disable-line no-console
  };
};

export const loadProducts = () => {
  return (dispatch) => {
    dispatch({ type: PRODUCTS_LOAD + FETCH });

    const prod = () => {
      axios.get(`${PRODUCTS_GLOBAL.loadProductsUrl}?url=${window.location.pathname}`)
        .then((response) => {
          const { payload, success, errorMessage } = response.data;

          if (!success) {
            dispatch({ type: PRODUCTS_LOAD + FAILURE });
            alert(errorMessage); // eslint-disable-line no-alert
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
          alert(error); // eslint-disable-line no-alert
          dispatch({ type: PRODUCTS_LOAD + FAILURE });
        });
    };

    const dev = () => {
      setTimeout(() => {
        dispatch({
          type: PRODUCTS_LOAD + SUCCESS,
          payload: {
            ui: loadProductsResponse.payload
          }
        });
      }, 2000);
    };

    callAC(dev, prod);
  };
};


export const loadFavoritesProducts = () => {
  return (dispatch) => {
    dispatch({ type: PRODUCTS_FAVORITE_LOAD + FETCH });

    const prod = () => {
      axios.get(PRODUCTS_GLOBAL.loadFavoritesProductsUrl)
        .then((response) => {
          const { payload, success, errorMessage } = response.data;

          if (!success) {
            dispatch({ type: PRODUCTS_FAVORITE_LOAD + FAILURE });
            alert(errorMessage); // eslint-disable-line no-alert
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
          alert(error); // eslint-disable-line no-alert
          dispatch({ type: PRODUCTS_FAVORITE_LOAD + FAILURE });
        });
    };

    const dev = () => {
      setTimeout(() => {
        dispatch({
          type: PRODUCTS_FAVORITE_LOAD + SUCCESS,
          payload: {
            ui: loadFavoriteProductsResponse.payload
          }
        });
      }, 2000);
    };

    callAC(dev, prod);
  };
};
