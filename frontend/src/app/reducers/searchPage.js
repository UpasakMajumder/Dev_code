import { SEARCH_PAGE_UI_SUCCESS, CHANGE_PAGE_PAGINATOR, CHANGE_PAGE_PAGINATION_LIMIT } from 'app.consts';

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
  case SEARCH_PAGE_UI_SUCCESS:
    return {
      ...state,
      ...payload
    };

  case CHANGE_PAGE_PAGINATOR:
    return {
      ...state,
      [payload.type]: payload.page
    };

  case CHANGE_PAGE_PAGINATION_LIMIT:
    return {
      ...state,
      [payload.type]: payload.value
    };

  default:
    return state;
  }
};
