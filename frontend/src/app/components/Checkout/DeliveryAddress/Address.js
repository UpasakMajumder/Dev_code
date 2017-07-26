import React from 'react';
import PropTypes from 'prop-types';

const Address = (props) => {
  const { id, street, city, state, zip, checkedId, changeShoppingData } = props;

  const streets = street.map((st, i) => <p key={i}>{st}</p>);

  return (
    <div>
      <input
        onChange={(e) => { changeShoppingData(e.target.name, id); }}
        id={`da-${id}`}
        name="deliveryAddress"
        checked={id === checkedId}
        type="radio"
        className="input__radio" />
      <label htmlFor={`da-${id}`} className="input__label input__label--radio">
        {streets}
        <p>{city}</p>
        <p>{state} {zip}</p>
      </label>
    </div>
  );
};

Address.propTypes = {
  street: PropTypes.arrayOf(PropTypes.string.isRequired).isRequired,
  changeShoppingData: PropTypes.func.isRequired,
  city: PropTypes.string.isRequired,
  zip: PropTypes.string.isRequired,
  id: PropTypes.number.isRequired,
  checkedId: PropTypes.number,
  checked: PropTypes.bool,
  state: PropTypes.string
};

export default Address;
