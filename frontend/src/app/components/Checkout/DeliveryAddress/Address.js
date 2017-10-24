import React from 'react';
import PropTypes from 'prop-types';
/* components */
import USAddress from 'app.dump/USAddress';

const Address = (props) => {
  const { id, street1, street2, city, state, zip, checkedId, changeShoppingData, disableInteractivity, customerName, email,
    country, phone } = props;

  return (
    <div>
      <input
        onChange={(e) => { changeShoppingData(e.target.name, id); }}
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
          street1={street1}
          street2={street2}
          city={city}
          state={state}
          zip={zip}
          country={country}
          phone={phone}
        />
      </label>
    </div>
  );
};

Address.propTypes = {
  street1: PropTypes.string.isRequired,
  street2: PropTypes.string,
  changeShoppingData: PropTypes.func.isRequired,
  city: PropTypes.string.isRequired,
  zip: PropTypes.string.isRequired,
  id: PropTypes.number.isRequired,
  checkedId: PropTypes.number,
  checked: PropTypes.bool,
  state: PropTypes.string,
  disableInteractivity: PropTypes.bool.isRequired,
  customerName: PropTypes.string,
  country: PropTypes.string,
  phone: PropTypes.string,
  email: PropTypes.string
};
export default Address;
