import React from 'react';
import PropTypes from 'prop-types';

const Address = (props) => {
  const { id, street, city, state, zip, checkedId, changeShoppingData, disableInteractivity, customerName, email,
    country, phone } = props;

  const streets = street.map((st, i) => <p key={i}>{st}</p>);
  const customerNameEl = customerName && <p>{customerName}</p>;
  const emailEl = email && <p>{email}</p>;
  const cityEl = city && <p>{city}</p>;
  const countryEl = country && <p>{country}</p>;
  const phoneEl = phone && <p>{phone}</p>;

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
        {customerNameEl}
        {emailEl}
        {streets}
        {cityEl}
        <p>{state} {zip}</p>
        {countryEl}
        {phoneEl}
      </label>
    </div>
  );
};

Address.propTypes = {
  street: PropTypes.arrayOf(PropTypes.string).isRequired,
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
