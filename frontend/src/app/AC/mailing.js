import axios from 'axios';
import * as constants from '../constants';
import { browserHistory } from 'react-router'
import { push } from 'react-router-redux';

export default function sendMailingList(credentials) {
  return (dispatch) => {
    dispatch({
      type: constants.FETCH_SERVERS,
      isLoading: true
    });

    setTimeout(() => {
      // ToDo: Change to POST and URL
      axios.get('http://localhost:3000/mailingSuccess', credentials)
        .then((response) => {
          dispatch({
            type: constants.FETCH_SERVERS_SUCCESS
          });

          dispatch(push('map-columns.html'));
          // browserHistory.push('map-columns.html');
        })
        .catch(() => {
          dispatch({
            type: constants.FETCH_SERVERS_FAILURE,
            isLoading: false
          });
        });
    }, 1000);
  };
}
