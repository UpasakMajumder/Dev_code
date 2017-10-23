import moment from 'moment';
import 'moment/min/locales.min.js';
import { LOCALE } from 'app.globals';

moment.locale(LOCALE);

export const convertToWords = (date) => {
  const timestamp = Date.parse(date);
  return isNaN(timestamp) ? date : moment(date).format('MMM D YYYY');
};

export const divideBySlash = (date) => {
  const timestamp = Date.parse(date);
  return isNaN(timestamp) ? date : moment(date).format('M/DD/YYYY');
};
