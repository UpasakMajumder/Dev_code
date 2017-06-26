import React, { Component } from 'react';
import SVG from '../../../SVG/index';

class AddressCard extends Component {
  static createAddressElement(content) {
    if (content) return <span>{content}</span>;
    return null;
  }

  render() {
    const { editButtonText, removeButtonText, address, openDialog } = this.props;

    const street1 = AddressCard.createAddressElement(address.street1);
    const street2 = AddressCard.createAddressElement(address.street2);
    const city = AddressCard.createAddressElement(address.city);
    const state = AddressCard.createAddressElement(address.state);
    const zip = AddressCard.createAddressElement(address.zip);

    const editButton = address.isEditButton
      ? <button onClick={() => openDialog(address)} type="button" className="in-card-btn">
        <SVG name="edit"/>
        {editButtonText}
      </button>
      : null;


    const removeButton = address.isRemoveButton
      ? <button type="button" className="in-card-btn">
          <SVG name="cross--dark"/>
          {removeButtonText}
        </button>
      : null;

    return (
      <div className="adress-card">
        {street1}
        {street2}
        {city}
        <span>{state} {zip}</span>

        <div className="adress-card__btn-block">
          {editButton}
          {removeButton}
        </div>
      </div>
    );
  }
}

export default AddressCard;
