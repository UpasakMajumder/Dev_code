import React from 'react';
import PropTypes from 'prop-types';
import SVG from 'app.dump/SVG';

const AddressCard = (props) => {
  const { editButtonText, removeButtonText, address, openDialog } = props;

  const createAddressElement = (content) => {
    if (content) return <span>{content}</span>;
    return null;
  };

  const street1 = createAddressElement(address.street1);
  const street2 = createAddressElement(address.street2);
  const city = createAddressElement(address.city);
  const state = createAddressElement(address.state);
  const zip = createAddressElement(address.zip);

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
};

AddressCard.propTypes = {
  address: PropTypes.shape({
    city: PropTypes.string,
    id: PropTypes.number,
    isEditButton: PropTypes.bool,
    isRemoveButton: PropTypes.bool,
    state: PropTypes.string,
    street1: PropTypes.string,
    street2: PropTypes.string,
    zip: PropTypes.string
  }).isRequired,
  editButtonText: PropTypes.string.isRequired,
  openDialog: PropTypes.func.isRequired,
  removeButtonText: PropTypes.string.isRequired
};

export default AddressCard;
