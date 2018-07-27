import moment from 'moment';
import 'moment/min/locales.min.js';
import { LANGUAGES } from 'app.globals';

if (LANGUAGES) moment.locale(LANGUAGES.locale || 'en');

export const dateFormat = 'MMM DD, YYYY';
export const dateTimeFormat = 'L LTS';

export default (date, devaultValue, time = false) => {
  const timestamp = Date.parse(date);

  let result = devaultValue || date;
  result = isNaN(timestamp) ? result : moment(date).format(time ? dateTimeFormat : dateFormat);

  return result;
};
