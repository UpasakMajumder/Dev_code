import moment from 'moment';

export const convertToWords = date => moment(date).format('MMM D YYYY');
export const divideBySlash = date => moment(date).format('M/DD/YYYY');
