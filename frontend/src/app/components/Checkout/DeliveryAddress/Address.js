import React from 'react';
import PropTypes from 'prop-types';
/* components */
import USAddress from 'app.dump/USAddress';

const Address = ({
  id,
  address1,
  address2,
  city,
  state,
  zip,
  checkedId,
  changeDeliveryAddress,
  disableInteractivity,
  customerName,
  email,
  country,
  phone,
  company
}) => {

  return (
    <div>
      <input
        onChange={() => { changeDeliveryAddress(id); }}
        id={`da-${id}`}
        disabled={disableInteractivity}
        name="deliveryAddress"
        checked={id === checkedId}
        type="radio"
        className="input__radio" />
      <label htmlFor={`da-${id}`} className="input__label input__label--radio">
        <USAddress
          customerName={customerName}
          email={email}
          address1={address1}
          address2={address2}
          city={city}
          state={state}
          zip={zip}
          country={country}
          phone={phone}
          company={company}
        />
      </label>
    </div>
  );
};

Address.propTypes = {
  address1: PropTypes.string.isRequired,
  address2: PropTypes.string,
  changeDeliveryAddress: PropTypes.func.isRequired,
  city: PropTypes.string.isRequired,
  zip: PropTypes.string.isRequired,
  id: PropTypes.number.isRequired,
  checkedId: PropTypes.number,
  checked: PropTypes.bool,
  state: PropTypes.string,
  disableInteractivity: PropTypes.bool.isRequired,
  customerName: PropTypes.string.isRequired,
  company: PropTypes.string,
  country: PropTypes.string,
  phone: PropTypes.string,
  email: PropTypes.string
};
export default Address;
