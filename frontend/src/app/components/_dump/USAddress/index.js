import React from 'react';
import PropTypes from 'prop-types';

const USAddress = (props) => {
  const createAddressElement = (content) => {
    if (content) return <p>{content}</p>;
    return null;
  };

  return (
    <div>
      {createAddressElement(props.customerName)}
      {createAddressElement(props.email)}
      <p>{props.street1}</p>
      {createAddressElement(props.street2)}
      <p>{props.city}, {props.state} {props.zip}</p>
      {createAddressElement(props.country)}
      {createAddressElement(props.phone)}
    </div>
  );
};

USAddress.propTypes = {
  customerName: PropTypes.string,
  email: PropTypes.string,
  street1: PropTypes.string.isRequired,
  street2: PropTypes.string,
  city: PropTypes.string.isRequired,
  state: PropTypes.string,
  zip: PropTypes.string.isRequired,
  country: PropTypes.string,
  phone: PropTypes.string
};

export default USAddress;
