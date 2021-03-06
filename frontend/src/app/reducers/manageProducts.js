import { FETCH, SUCCESS, FAILURE, START, FINISH, MANAGE_PRODUCTS } from 'app.consts';

const defaultState = {
  tableHeaders: [],
  templates: [],
  title: '',
  openInDesignBtn: '',
  isLoading: false
};

export default (state = defaultState, action) => {
  const { type, payload } = action;

  switch (type) {
  case MANAGE_PRODUCTS + FETCH:
    return {
      ...state,
      isLoading: true
    };

  case MANAGE_PRODUCTS + SUCCESS:
    return {
      ...state,
      tableHeaders: payload.header,
      templates: payload.data,
      title: payload.title,
      openInDesignBtn: payload.openInDesignBtn,
      isLoading: false
    };
  case MANAGE_PRODUCTS + FAILURE:
    return {
      ...state,
      isLoading: false
    };
  default:
    return state;
  }
};
