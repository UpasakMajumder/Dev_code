import React from 'react';
import PropTypes from 'prop-types';
import { Tooltip } from 'react-tippy';
/* components */
import SVG from 'app.dump/SVG';
/* local components */
import AddressCard from '../AddressCard';

const AddressBlock = (props) => {
  const { ui, openDialog, setDefault, unsetDefault } = props;

  if (!Object.keys(ui).length) return null;

  const { title, addButton, editButton, removeButton, addresses, allowAddresses, defaultAddress } = ui;
  const allowAddressesNumber = typeof allowAddresses === 'number' ? allowAddresses : 0;

  const addButtonElement = addButton.exists && addresses.length < allowAddressesNumber
    ?
    (
      <Tooltip
        title={addButton.text}
        position="right"
        animation="fade"
        arrow={true}
        theme="dark"
      >
        <button type="button" className="btn--off plus-btn" onClick={() => { openDialog(null, false); }}>
          <SVG name="plus" className="icon-modal" />
        </button>
      </Tooltip>
    )
    : null;

  const commonProps = {
    editButton,
    removeButton,
    openDialog,
    defaultAddress,
    setDefault,
    unsetDefault
  };

  const addressCards = addresses.length
    ? addresses.map((address) => {
      return <AddressCard key={address.id}
                          address={address}
                          {...commonProps}
      />;
    })
    : null;

  return (
    <div className="settings__item">
      <div className="action-heading">
        <h2>{title}</h2>
        {addButtonElement}
      </div>

      <div>
        { addressCards }
      </div>
    </div>
  );
};

AddressBlock.propTypes = {
  ui: PropTypes.shape({
    allowAddresses: PropTypes.number,
    addButton: PropTypes.shape({
      exists: PropTypes.bool.isRequired,
      tooltip: PropTypes.string
    }),
    addresses: PropTypes.arrayOf(PropTypes.object.isRequired),
    editButton: PropTypes.object,
    defaultAddress: PropTypes.object,
    removeButton: PropTypes.object,
    title: PropTypes.string
  }).isRequired,
  openDialog: PropTypes.func.isRequired,
  setDefault: PropTypes.func.isRequired,
  unsetDefault: PropTypes.func.isRequired
};

export default AddressBlock;
