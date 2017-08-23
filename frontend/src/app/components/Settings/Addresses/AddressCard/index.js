import React from 'react';
import PropTypes from 'prop-types';
/* components */
import SVG from 'app.dump/SVG';

const AddressCard = (props) => {
  const { editButton, removeButton, address, openDialog } = props;

  let editElement = null;
  if (editButton.exists) {
    const data = address || {
      street1: '',
      street2: '',
      city: '',
      state: '',
      zip: ''
    };

    editElement = (
      <button onClick={() => openDialog(data)} type="button" className="in-card-btn">
        <SVG name="edit"/>
        {editButton.text}
      </button>
    );
  }

  if (!address) {
    return (
      <div className="adress-card">{editElement}</div>
    );
  }

  const createAddressElement = (content) => {
    if (content) return <span>{content}</span>;
    return null;
  };

  const street1 = createAddressElement(address.street1);
  const street2 = createAddressElement(address.street2);
  const city = createAddressElement(address.city);
  const state = createAddressElement(address.state);
  const zip = createAddressElement(address.zip);

  const removeElement = removeButton.exists
    ? <button type="button" className="in-card-btn">
        <SVG name="cross--dark"/>
        {removeButton.text}
      </button>
    : null;

  const buttonBlock = editElement || removeElement
    ?
    (
      <div className="adress-card__btn-block">
        {editElement}
        {removeElement}
      </div>
    )
    : null;

  return (
    <div className="adress-card">
      <div>{street1}</div>
      <div>{street2}</div>
      <div>{city}</div>
      <div>{state} {zip}</div>
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
