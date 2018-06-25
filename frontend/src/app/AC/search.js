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
  return (dispatch, getState) => {
    dispatch({ type: SEARCH_RESULTS + FETCH });

    axios({
      method: 'post',
      url: `${SEARCH.queryUrl}?phrase=${encodeURI(query)}`,
      data: {
        query
      }
    }).then((response) => {
      const { payload, success, errorMessage } = response.data;
      const visibilityStatus = getState().search.query.length ? SHOW : HIDE;

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

        dispatch({ type: SEARCH_RESULTS + visibilityStatus });
        dispatch({ type: HEADER_SHADOW + visibilityStatus });
      }
    })
      .catch((error) => {
        dispatch({ type: SEARCH_RESULTS + FAILURE });
      });
  };
};
