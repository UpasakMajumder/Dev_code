import { SUCCESS, INIT_UI, SEARCH_PAGE, CHANGE_PAGE_PAGINATOR, CHANGE_PAGINATION_LIMIT } from 'app.consts';

const defaultState = {
  products: [],
  pages: [],
  pagesPage: 0,
  productsPage: 0,
  productsPaginationLimit: 8,
  getAllResults: false
};

export default (state = defaultState, action) => {
  const { type, payload } = action;

  switch (type) {
  case SEARCH_PAGE + INIT_UI + SUCCESS:
    return {
      ...state,
      ...payload
    };

  case CHANGE_PAGE_PAGINATOR:
    return {
      ...state,
      [payload.type]: payload.page
    };

  case CHANGE_PAGINATION_LIMIT:
    return {
      ...state,
      [payload.type]: payload.value
    };

  default:
    return state;
  }
};
