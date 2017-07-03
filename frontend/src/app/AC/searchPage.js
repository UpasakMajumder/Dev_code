import axios from 'axios';
import { SEARCH_PAGE_UI_FETCHING, SEARCH_PAGE_UI_SUCCESS, SEARCH_PAGE_UI_FAILURE, CHANGE_PAGE_PAGINATOR, CHANGE_PAGE_PAGINATION_LIMIT } from 'app.consts';
import { SEARCH_PAGE } from 'app.globals';
import ui from 'app.ws/searchPageUI';

export const getUI = (query) => {
  return (dispatch) => {
    dispatch({ type: SEARCH_PAGE_UI_FETCHING });

    // axios({
    //   method: 'get',
    //   url: `${SEARCH_PAGE.searchPageUrl}?phrase=${query}`
    // }).then((response) => {
    //   const { payload, success, errorMessage } = response.data;
    //
    //   if (!success) {
    //     dispatch({ type: SEARCH_PAGE_UI_FAILURE });
    //     alert(errorMessage); // eslint-disable-line no-alert
    //   } else {
    //     dispatch({
    //       type: SEARCH_PAGE_UI_SUCCESS,
    //       payload: {
    //         ...payload,
    //         getAllResults: true
    //       }
    //     });
    //   }
    // })
    //   .catch(() => {
    //     dispatch({ type: SEARCH_PAGE_UI_FAILURE });
    //   });

    setTimeout(() => {
      dispatch({
        type: SEARCH_PAGE_UI_SUCCESS,
        payload: {
          ...ui,
          getAllResults: true
        }
      });
    }, 2000);
  };
};

export const changePage = (page, type) => {
  return (dispatch) => {
    dispatch({
      type: CHANGE_PAGE_PAGINATOR,
      payload: {
        page,
        type
      }
    });
  };
};

export const setPaginationLimit = (type, value) => {
  return (dispatch) => {
    dispatch({
      type: CHANGE_PAGE_PAGINATION_LIMIT,
      payload: {
        value,
        type
      }
    });
  };
};
