import { FETCH, SUCCESS, FAILURE, START, FINISH,
  PRODUCTS_LOAD, PRODUCT_MARK_AS_FAVOURITE, PRODUCT_UNMARK_AS_FAVOURITE,
} from 'app.consts';


const defaultState = {
  categories: [],
  products: [],
  isLoading: false,
};

export default (state = defaultState, action) => {
  const { type, payload } = action;

  switch (type) {
    case PRODUCTS_LOAD + FETCH:
      return {
        ...state,
        isLoading: true
      };

    case PRODUCTS_LOAD + SUCCESS:
      return {
        ...state,
        products: payload.payload.products,
        categories: payload.payload.categories,
        isLoading: false
      };

    case PRODUCTS_LOAD + FAILURE:
      return {
        ...state,
        isLoading: false
      };

    //TODO
    //case PRODUCT_MARK_AS_FAVOURITE + FETCH:
    //case PRODUCT_UNMARK_AS_FAVOURITE + FETCH:


    default:
      return state;
  }
};
