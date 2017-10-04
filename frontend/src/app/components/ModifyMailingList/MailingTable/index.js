import React from 'react';
import PropTypes from 'prop-types';

const MailingTable = (props) => {
  const { items, fields } = props;
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
          <th>{fields.fullName.header}</th>
          <th>{fields.firstAddressLine.header}</th>
          <th>{fields.secondAddressLine.header}</th>
          <th>{fields.city.header}</th>
          <th>{fields.state.header}</th>
          <th>{fields.postalCode.header}</th>
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
  }).isRequired).isRequired,
  fields: PropTypes.shape({
    fullName: PropTypes.shape({
      header: PropTypes.string.isRequired
    }).isRequired,
    firstAddressLine: PropTypes.shape({
      header: PropTypes.string.isRequired
    }).isRequired,
    secondAddressLine: PropTypes.shape({
      header: PropTypes.string.isRequired
    }).isRequired,
    city: PropTypes.shape({
      header: PropTypes.string.isRequired
    }).isRequired,
    state: PropTypes.shape({
      header: PropTypes.string.isRequired
    }).isRequired,
    postalCode: PropTypes.shape({
      header: PropTypes.string.isRequired
    }).isRequired
  }).isRequired
};

export default MailingTable;
