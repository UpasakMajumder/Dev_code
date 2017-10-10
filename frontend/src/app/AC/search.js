import axios from 'axios';
/* constants */
import { SHOW, HIDE, FETCH, SUCCESS, FAILURE, SEARCH_RESULTS, HEADER_SHADOW, CHANGE_SEARCH_QUERY } from 'app.consts';
/* helpers */
import { callAC } from 'app.helpers/ac';
/* globals */
import { SEARCH } from 'app.globals';

export const closeDropdown = () => {
  return (dispatch) => {
    dispatch({ type: SEARCH_RESULTS + HIDE });
    dispatch({ type: HEADER_SHADOW + HIDE });
  };
};

export const changeSearchQuery = (query) => {
  return (dispatch) => {
    dispatch({
      type: CHANGE_SEARCH_QUERY,
      payload: {
        query
      }
    });
  };
};

export const sendQuery = (query, pressedEnter) => {
  return (dispatch) => {
    dispatch({ type: SEARCH_RESULTS + FETCH });

    axios({
      method: 'post',
      url: `${SEARCH.queryUrl}?phrase=${encodeURI(query)}`,
      data: {
        query
      }
    }).then((response) => {
      const { payload, success, errorMessage } = response.data;

      if (!success) {
        dispatch({
          type: SEARCH_RESULTS + FAILURE,
          alert: errorMessage
        });
      } else {
        dispatch({
          type: SEARCH_RESULTS + SUCCESS,
          payload: {
            ...payload,
            pressedEnter: pressedEnter || false
          }
        });
        dispatch({ type: SEARCH_RESULTS + SHOW });
        dispatch({ type: HEADER_SHADOW + SHOW });
      }
    })
      .catch((error) => {
        dispatch({ type: SEARCH_RESULTS + FAILURE });
      });
  };
};
