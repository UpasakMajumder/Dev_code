import React from 'react';
import PropTypes from 'prop-types';
/* components */
import SVG from 'app.dump/SVG';
/* local components */
import AddressCard from '../AddressCard';

const AddressBlock = (props) => {
  const { ui, openDialog } = props;

  if (!Object.keys(ui).length) return null;

  const { title, addButton, editButton, removeButton, addresses } = ui;

  const addButtonElement = addButton.exists
  ? <buttn className="plus-btn">
      <SVG name="plus" className="icon-modal" />
    </buttn>
  : null;

  const commonProps = {
    editButton,
    removeButton,
    openDialog
  };

  const addressCards = addresses.length
    ? addresses.map((address) => {
      return <AddressCard key={address.id}
                          address={address}
                          {...commonProps} />;
    })
    : <AddressCard {...commonProps} />;

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
    editButton: PropTypes.object,
    removeButton: PropTypes.object,
    title: PropTypes.string
  }).isRequired,
  openDialog: PropTypes.func.isRequired
};

export default AddressBlock;
