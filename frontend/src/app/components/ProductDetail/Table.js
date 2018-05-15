import React, { Component } from 'react';
import PropTypes from 'prop-types';
import Immutable from 'immutable';
import ImmutablePropTypes from 'react-immutable-proptypes';
import uuid from 'uuid';
/* component */
import SVG from 'app.dump/SVG';

const Table = ({
  data,
  estimates = false
}) => {
  const body = data.get('body');

  if (!data || !body.count()) return null;

  const list = body.map((item) => {
    return (
      <tr key={uuid()}>
        <td>{item.get('key')}</td>
        <td>{item.get('value')}</td>
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
    }).isRequired)
  }),
  estimates: PropTypes.bool
};

export default Table;
