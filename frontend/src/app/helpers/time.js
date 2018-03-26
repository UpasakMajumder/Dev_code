import moment from 'moment';
import 'moment/min/locales.min.js';
import { LANGUAGES } from 'app.globals';

if (LANGUAGES) moment.locale(LANGUAGES.locale || 'en-gb');

export const dateFormat = 'MMM DD, YYYY';

export default (date, devaultValue) => {
  const timestamp = Date.parse(date);

  let result = devaultValue || date;
  result = isNaN(timestamp) ? result : moment(date).format(dateFormat);

  return result;
};
