import React from 'react';
import PropTypes from 'prop-types';

const MailingTable = (props) => {
  const { items } = props;
  const tbody = items.map((item) => {
    return (
      <tr key={item.id}>
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
    id: PropTypes.string.isRequired,
    fullName: PropTypes.string.isRequired,
    firstAddressLine: PropTypes.string.isRequired,
    secondAddressLine: PropTypes.string,
    city: PropTypes.string.isRequired,
    state: PropTypes.string.isRequired,
    postalCode: PropTypes.string.isRequired,
    errorMessage: PropTypes.string
  }).isRequired).isRequired
};

export default MailingTable;
