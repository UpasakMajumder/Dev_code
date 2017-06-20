import axios from 'axios';
import { SEARCH_PAGE_UI_FETCHING, SEARCH_PAGE_UI_SUCCESS, SEARCH_PAGE_UI_FAILURE } from '../constants';
import { SEARCH_PAGE } from '../globals';
import ui from '../testServices/searchPageUI';

export const getUI = () => {
  return (dispatch) => {
    dispatch({ type: SEARCH_PAGE_UI_FETCHING });

    // axios({
    //   method: 'get',
    //   url: SEARCH_PAGE.searchPageUrl
    // }).then((response) => {
    //   const { payload, success, errorMessage } = response.data;
    //
    //   if (!success) {
    //     dispatch({ type: SEARCH_PAGE_UI_FAILURE });
    //     alert(errorMessage); // eslint-disable-line no-alert
    //   } else {
    //     dispatch({
    //       type: SEARCH_PAGE_UI_SUCCESS,
    //       payload
    //     });
    //   }
    // })
    //   .catch(() => {
    //     dispatch({ type: SEARCH_PAGE_UI_FAILURE });
    //   });

    setTimeout(() => {
      dispatch({
        type: SEARCH_PAGE_UI_SUCCESS,
        payload: ui
      })
    }, 2000);
  };
};

export const changePage = (number) => {
  return number;
};
