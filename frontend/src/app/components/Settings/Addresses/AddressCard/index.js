import React from 'react';
import PropTypes from 'prop-types';
import { Tooltip } from 'react-tippy';
/* components */
import SVG from 'app.dump/SVG';

const AddressCard = (props) => {
  const { editButton, removeButton, address, openDialog, defaultAddress, setDefault, unsetDefault } = props;

  let editElement = null;
  if (editButton.exists) {
    editElement = (
      <button onClick={() => openDialog(address)} type="button" className="in-card-btn">
        <SVG name="edit"/>
        {editButton.text}
      </button>
    );
  }

  const createAddressElement = (content) => {
    if (content) return <span>{content}</span>;
    return null;
  };

  const setDefaultElement = () => {
    const isDefault = address.id === defaultAddress.id;

    const onClick = () => {
      if (isDefault) {
        unsetDefault(address.id, defaultAddress.unsetUrl);
      } else {
        setDefault(address.id, defaultAddress.setUrl);
      }
    };

    return (
      <Tooltip
        title={defaultAddress.tooltip}
        position="bottom"
        animation="fade"
        arrow={true}
        theme="dark"
      >
        <button onClick={onClick} type="button" className={`in-card-btn ${isDefault ? 'in-card-btn--primary' : ''}`}>
          <SVG name="star--default" style={{ fill: isDefault ? 'white' : '#404040' }}/>
          {isDefault ? defaultAddress.labelDefault : defaultAddress.labelNonDefault}
        </button>
      </Tooltip>
    );
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
      <div className="address-card__btn-block">
        {editElement}
        {removeElement}
        {setDefaultElement()}
      </div>
    )
    : null;

  return (
    <div className="address-card">
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
  defaultAddress: PropTypes.shape({
    id: PropTypes.number,
    labelDefault: PropTypes.string.isRequired,
    labelNonDefault: PropTypes.string.isRequired,
    tooltip: PropTypes.string.isRequired
  }).isRequired,
  editButton: PropTypes.shape({
    text: PropTypes.string.isRequired,
    exists: PropTypes.bool.isRequired
  }).isRequired,
  removeButton: PropTypes.shape({
    text: PropTypes.string.isRequired,
    exists: PropTypes.bool.isRequired
  }).isRequired,
  openDialog: PropTypes.func.isRequired,
  setDefault: PropTypes.func.isRequired,
  unsetDefault: PropTypes.func.isRequired
};

export default AddressCard;
