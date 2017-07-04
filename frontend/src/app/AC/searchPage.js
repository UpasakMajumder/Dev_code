import axios from 'axios';
/* constants */
import { FETCH, SUCCESS, FAILURE, INIT_UI, SEARCH_PAGE, CHANGE_PAGE_PAGINATOR, CHANGE_PAGINATION_LIMIT } from 'app.consts';

/* globals */
import { SEARCH_PAGE as SEARCH_PAGE_URL } from 'app.globals';
/* web service */
import ui from 'app.ws/searchPageUI';

export const getUI = (query) => {
  return (dispatch) => {
    dispatch({ type: SEARCH_PAGE + INIT_UI + FETCH });

    axios({
      method: 'get',
      url: `${SEARCH_PAGE_URL.searchPageUrl}?phrase=${query}`
    }).then((response) => {
      const { payload, success, errorMessage } = response.data;

      if (!success) {
        dispatch({ type: SEARCH_PAGE + INIT_UI + FAILURE });
        alert(errorMessage); // eslint-disable-line no-alert
      } else {
        dispatch({
          type: SEARCH_PAGE + INIT_UI + SUCCESS,
          payload: {
            ...payload,
            getAllResults: true
          }
        });
      }
    })
      .catch(() => {
        dispatch({ type: SEARCH_PAGE + INIT_UI + FAILURE });
      });

    // setTimeout(() => {
    //   dispatch({
    //     type: SEARCH_PAGE + INIT_UI + SUCCESS,
    //     payload: {
    //       ...ui,
    //       getAllResults: true
    //     }
    //   });
    // }, 2000);
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
      type: CHANGE_PAGINATION_LIMIT,
      payload: {
        value,
        type
      }
    });
  };
};
