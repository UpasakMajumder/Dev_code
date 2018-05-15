import React, { Component } from 'react';
import PropTypes from 'prop-types';
import Immutable from 'immutable';
import ImmutablePropTypes from 'react-immutable-proptypes';
/* component */
import SVG from 'app.dump/SVG';

const Table = ({ availability }) => {
  if (!availability) return null;
  const type = availability.get('type');

  return (
    <div className={`stock stock--${type}`}>
      <SVG
        name={`stock--${type}`}
        className="icon-stock"
      />
      {availability.get('text')}
    </div>
  );
};

Table.propTypes = {
  availability: ImmutablePropTypes.mapContains({
    type: PropTypes.oneOf(['unavailable', 'outofstock', 'available']).isRequired,
    text: PropTypes.string.isRequired
  })
};

export default Table;
