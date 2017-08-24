import moment from 'moment';

export const convertToWords = (date) => {
  if (!date) return null;
  return moment(date).format('MMM D YYYY');
};

export const divideBySlash = (date) => {
  if (!date) return null;
  return moment(date).format('M/DD/YYYY');
};
