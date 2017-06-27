import React from 'react';
import PropTypes from 'prop-types';
import AddressCard from '../AddressCard';
import SVG from '../../../SVG/index';

const AddressBlock = (props) => {
  const { ui, openDialog } = props;
  if (!Object.keys(ui).length) return null;

  const { title, addButton, editButtonText, removeButtonText, addresses } = ui;

  const addButtonElement = addButton.exists
  ? <buttn className="plus-btn">
      <SVG name="plus" className="icon-modal" />
    </buttn>
  : null;

  const commonProps = {
    editButtonText,
    openDialog
  };

  const addressCards = addresses.length
    ? addresses.map((address) => {
      return <AddressCard key={address.id}
                          removeButtonText={removeButtonText}
                          address={address}
                          {...commonProps} />;
    })
    : <AddressCard address={{ isEditButton: true }} {...commonProps} />;

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
    addButton: PropTypes.shape({
      exists: PropTypes.bool.isRequired,
      tooltip: PropTypes.string
    }),
    addresses: PropTypes.arrayOf(PropTypes.object.isRequired),
    editButtonText: PropTypes.string,
    title: PropTypes.string
  }).isRequired,
  openDialog: PropTypes.func.isRequired
};

export default AddressBlock;
