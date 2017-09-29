import React from 'react';
import PropTypes from 'prop-types';
/* components */
import SVG from 'app.dump/SVG';
import USAddress from 'app.dump/USAddress';

const AddressCard = (props) => {
  const { editButton, removeButton, address, openDialog } = props;

  let editElement = null;
  if (editButton.exists) {
    editElement = (
      <button onClick={() => openDialog(address)} type="button" className="in-card-btn">
        <SVG name="edit"/>
        {editButton.text}
      </button>
    );
  }

  const removeElement = removeButton.exists
    ? <button type="button" className="in-card-btn">
        <SVG name="cross--dark"/>
        {removeButton.text}
      </button>
    : null;

  const buttonBlock = editElement || removeElement
    ?
    (
      <div className="address-card__btn-block">
        {editElement}
        {removeElement}
      </div>
    )
    : null;

  return (
    <div className="address-card">
      <USAddress
        street1={address.street1}
        street2={address.street2}
        city={address.city}
        state={address.state}
        zip={address.zip}
      />
      {buttonBlock}
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
  }),
  editButton: PropTypes.shape({
    text: PropTypes.string.isRequired,
    exists: PropTypes.bool.isRequired
  }).isRequired,
  removeButton: PropTypes.shape({
    text: PropTypes.string.isRequired,
    exists: PropTypes.bool.isRequired
  }).isRequired,
  openDialog: PropTypes.func.isRequired
};

export default AddressCard;
