import React, { Component } from 'react';
import PropTypes from 'prop-types';
import Immutable from 'immutable';
import ImmutablePropTypes from 'react-immutable-proptypes';
import uuid from 'uuid';
/* component */
import SVG from 'app.dump/SVG';

const Table = ({
  data,
  optionsPrice,
  priceElementId,
  estimates = false
}) => {
  if (!data) return null;

  const body = data.get('body');
  if (!body || !body.count()) return null;

  const list = body.map((item) => {
    const value = priceElementId === item.get('id')
      ? optionsPrice || item.get('value')
      : item.get('value');

    return (
      <tr key={uuid()}>
        <td>{item.get('key')}</td>
        <td>{value}</td>
      </tr>
    );
  });

  return (
      <table className="table">
        <tbody>
          <tr>
            <th colSpan="2">
              <SVG
                className="icon-deliver"
                name={estimates ? 'deliver-car' : 'money'}
              />
              {data.get('header')}
            </th>
          </tr>
          {list}
        </tbody>
      </table>
  );
};

Table.propTypes = {
  data: ImmutablePropTypes.mapContains({
    header: PropTypes.string.isRequired,
    body: ImmutablePropTypes.listOf(ImmutablePropTypes.mapContains({
      id: PropTypes.string,
      key: PropTypes.string.isRequired,
      value: PropTypes.string.isRequired
    }))
  }),
  estimates: PropTypes.bool,
  optionsPrice: PropTypes.string,
  priceElementId: PropTypes.string
};

export default Table;
