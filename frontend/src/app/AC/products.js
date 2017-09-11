import axios from 'axios';
/* constants */
import { FETCH, SUCCESS, FAILURE, START, FINISH,
  PRODUCTS_LOAD, PRODUCTS_FAVORITE_LOAD, PRODUCT_MARK_AS_FAVOURITE, PRODUCT_UNMARK_AS_FAVOURITE,
} from 'app.consts';
/* helpers */
import { callAC } from 'app.helpers/ac';

/* globals */
//TODO URLs from config
//import { CHECKOUT as CHECKOUT_URL, NOTIFICATION } from 'app.globals';



//TODO enhance config
/*config.localization.products = {
  addToFavorites: "asdasdasdasdasdas"
};

config.products = {
  loadProducts: "/api/products",
  markProductFavourite: "/api/favorites/set"
  unmarkProductFavourite: "/api/favorites/unset"
};*/



/* web service */
import loadProductsResponse from 'app.ws/products';
import loadFavoriteProductsResponse from 'app.ws/productsFavorite';



//TODO
export const markProductFavourite = (productId) => {
  //TODO
  console.log('markProductFavourite', productId);
};


//TODO
export const unmarkProductFavourite = (productId) => {
  //TODO
  console.log('unmarkProductFavourite', productId);
};


export const loadProducts = () => {
  return (dispatch) => {
    dispatch({ type: PRODUCTS_LOAD + FETCH });

    const prod = () => {
      //TODO
      /*
      axios.post(url, { id })
        .then((response) => {
          const { payload, success, errorMessage } = response.data;

          if (!success) {
            dispatch({ type: RECALCULATE_CHECKOUT_PRICE + FAILURE });
            alert(errorMessage); // eslint-disable-line no-alert
            return;
          }

          getTotalPrice(dispatch);

          dispatch({
            type: RECALCULATE_CHECKOUT_PRICE + SUCCESS,
            payload: {
              ui: payload
            }
          });
        })
        .catch((error) => {
          alert(error); // eslint-disable-line no-alert
          dispatch({ type: RECALCULATE_CHECKOUT_PRICE + FAILURE });
        });
      */
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


export const loadFavoriteProducts = () => {
  return (dispatch) => {
    dispatch({ type: PRODUCTS_FAVORITE_LOAD + FETCH });

    const prod = () => {
      //TODO
      /*
       axios.post(url, { id })
       .then((response) => {
       const { payload, success, errorMessage } = response.data;

       if (!success) {
       dispatch({ type: RECALCULATE_CHECKOUT_PRICE + FAILURE });
       alert(errorMessage); // eslint-disable-line no-alert
       return;
       }

       getTotalPrice(dispatch);

       dispatch({
       type: RECALCULATE_CHECKOUT_PRICE + SUCCESS,
       payload: {
       ui: payload
       }
       });
       })
       .catch((error) => {
       alert(error); // eslint-disable-line no-alert
       dispatch({ type: RECALCULATE_CHECKOUT_PRICE + FAILURE });
       });
       */
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
