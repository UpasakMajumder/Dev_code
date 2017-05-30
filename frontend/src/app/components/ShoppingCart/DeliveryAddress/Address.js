import React, { Component } from 'react';
import PropTypes from 'prop-types';

class Address extends Component {
  render() {
    const { id, street, city, state, zip, checkedId, changeShoppingData } = this.props;

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
  }
}

export default Address;
