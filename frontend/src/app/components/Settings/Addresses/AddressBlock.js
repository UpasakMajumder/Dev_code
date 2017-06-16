import React, { Component } from 'react';
import AddressCard from './AddressCard';
import SVG from '../../SVG';

class AddressBlock extends Component {
  render() {
    const { ui, openDialog, dialog, closeDialog, modifyAddress } = this.props;
    if (!Object.keys(ui).length) return null;

    const { title, addButton, editButtonText, removeButtonText, addresses } = ui;

    const addButtonElement = addButton.exists
    ? <buttn className="plus-btn">
        <SVG name="plus" className="icon-modal" />
      </buttn>
    : null;

    const commonProps = {
      editButtonText,
      openDialog,
      dialog,
      closeDialog,
      modifyAddress
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
  }
}

export default AddressBlock;
