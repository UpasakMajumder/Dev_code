import React from 'react';
import PropTypes from 'prop-types';
import SVG from '../SVG';

const Stock = (props) => {
  const { type, text } = props;

  let stockClass;

  switch (type) {
  case 'available':
    stockClass = 'stock--available';
    break;
  case 'unavailable':
    stockClass = 'stock--unavailable';
    break;
  case 'out':
  default:
    stockClass = 'stock--out';
    break;
  }

  return (
    <div className={`stock ${stockClass}`}>
      <SVG name={stockClass} className={`icon-stock icon-${stockClass}`}/>
      {text}
    </div>
  );

};

Stock.propTypes = {
  type: PropTypes.string,
  text: PropTypes.string.isRequired
};

export default Stock;
