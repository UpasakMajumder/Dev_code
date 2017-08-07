import axios from 'axios';
/* constants */
import { SHOW, HIDE, FETCH, SUCCESS, FAILURE, SEARCH_RESULTS, HEADER_SHADOW, CHANGE_SEARCH_QUERY } from 'app.consts';
/* globals */
import { SEARCH } from 'app.globals';
/* web service */
import ui from 'app.ws/searchUI';

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

    const prod = () => {
      axios({
        method: 'post',
        url: `${SEARCH.queryUrl}?phrase=${encodeURI(query)}`,
        data: {
          query
        }
      }).then((response) => {
        const { payload, success, errorMessage } = response.data;

        if (!success) {
          dispatch({ type: SEARCH_RESULTS + FAILURE });
          alert(errorMessage); // eslint-disable-line no-alert
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
          alert(error); // eslint-disable-line no-alert
        });
    };

    const dev = () => {
      setTimeout(() => {
        dispatch({
          type: SEARCH_RESULTS + SUCCESS,
          payload: {
            ...ui,
            pressedEnter: pressedEnter || false
          }
        });

        dispatch({ type: SEARCH_RESULTS + SHOW });
        dispatch({ type: HEADER_SHADOW + SHOW });
      }, 200);
    };

    // dev();
    prod();
  };
};
