import React from 'react';
import PropTypes from 'prop-types';

const MailingTable = (props) => {
  const { items } = props;
  const tbody = items.map((item, index) => {
    if (typeof item.line === 'object') {
      return (
        <tr key={index}>
          <td>{item.line.value}</td>
          <td>{item.fullName.value}</td>
          <td>{item.firstAddressLine.value}</td>
          <td>{item.secondAddressLine.value}</td>
          <td>{item.city.value}</td>
          <td>{item.state.value}</td>
          <td>{item.postalCode.value}</td>
        </tr>
      );
    }
    return (
      <tr key={index}>
        <td>{item.line}</td>
        <td>{item.fullName}</td>
        <td>{item.firstAddressLine}</td>
        <td>{item.secondAddressLine}</td>
        <td>{item.city}</td>
        <td>{item.state}</td>
        <td>{item.postalCode}</td>
      </tr>
    );
  });

  return (
    <table className="table processed-list__table--shadow">
      <tbody>
        <tr>
          <th>Line</th>
          <th>Name</th>
          <th>Address Line 1</th>
          <th>Address Line 2</th>
          <th>City</th>
          <th>State</th>
          <th>Zip Code</th>
        </tr>

        {tbody}
      </tbody>
    </table>
  );
};

MailingTable.propTypes = {
  items: PropTypes.arrayOf(PropTypes.shape({
    line: PropTypes.oneOfType([
      PropTypes.string,
      PropTypes.shape({
        value: PropTypes.string.isRequired,
        isInvalid: PropTypes.bool,
        message: PropTypes.string
      })
    ]).isRequired,
    fullName: PropTypes.oneOfType([
      PropTypes.string,
      PropTypes.shape({
        value: PropTypes.string.isRequired,
        isInvalid: PropTypes.bool,
        message: PropTypes.string
      })
    ]).isRequired,
    firstAddressLine: PropTypes.oneOfType([
      PropTypes.string,
      PropTypes.shape({
        value: PropTypes.string.isRequired,
        isInvalid: PropTypes.bool,
        message: PropTypes.string
      })
    ]).isRequired,
    secondAddressLine: PropTypes.oneOfType([
      PropTypes.string,
      PropTypes.shape({
        value: PropTypes.string,
        isInvalid: PropTypes.bool,
        message: PropTypes.string
      })
    ]).isRequired,
    city: PropTypes.oneOfType([
      PropTypes.string,
      PropTypes.shape({
        value: PropTypes.string.isRequired,
        isInvalid: PropTypes.bool,
        message: PropTypes.string
      })
    ]).isRequired,
    state: PropTypes.oneOfType([
      PropTypes.string,
      PropTypes.shape({
        value: PropTypes.string.isRequired,
        isInvalid: PropTypes.bool,
        message: PropTypes.string
      })
    ]).isRequired,
    postalCode: PropTypes.oneOfType([
      PropTypes.string,
      PropTypes.shape({
        value: PropTypes.string.isRequired,
        isInvalid: PropTypes.bool,
        message: PropTypes.string
      })
    ]).isRequired
  }).isRequired).isRequired
};

export default MailingTable;
