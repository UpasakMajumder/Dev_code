import React, { Component } from 'react';
import PropTypes from 'prop-types';
import Immutable from 'immutable';
import ImmutablePropTypes from 'react-immutable-proptypes';
/* component */
import SVG from 'app.dump/SVG';
import Spinner from 'app.dump/Spinner';

const Stock = ({ availability }) => {
  if (!availability) return null;
  const type = availability.get('type');
  let content = <div className="stock__spinner"><Spinner /></div>;

  if (type) {
    content = [
      <SVG
        name={`stock--${type}`}
        className="icon-stock"
        key={0}
      />,
      <span key={1}>{availability.get('text')}</span>
    ];
  }

  return (
    <div className={`stock mr-3 stock--${type}`}>
      {content}
    </div>
  );
};

Stock.propTypes = {
  availability: ImmutablePropTypes.mapContains({
    type: PropTypes.oneOf(['unavailable', 'outofstock', 'available', '']),
    text: PropTypes.string
  })
};

export default Stock;
