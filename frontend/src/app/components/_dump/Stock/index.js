// @flow
import React from 'react';
import PropTypes from 'prop-types';
/* components */
import SVG from 'app.dump/SVG';

const Stock = (props: { type: ?string, text: string }) => {
  const { type, text } = props;

  let stockClass: string;

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
