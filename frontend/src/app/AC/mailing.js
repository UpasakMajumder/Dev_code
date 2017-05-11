import axios from 'axios';
import { browserHistory } from 'react-router';
import { push } from 'react-router-redux';
import * as constants from '../constants';

export default function sendMailingList(credentials) {
  return (dispatch) => {
    dispatch({
      type: constants.FETCH_SERVERS,
      isLoading: true
    });

    // ToDo: Change to POST and URL
    axios.get('http://localhost:3000/mailingSuccess', credentials)
      .then(() => {
        dispatch({
          type: constants.FETCH_SERVERS_SUCCESS
        });

        dispatch(push('map-columns.html'));
      })
      .catch(() => {
        dispatch({
          type: constants.FETCH_SERVERS_FAILURE,
          isLoading: false
        });
      });
  };
}
