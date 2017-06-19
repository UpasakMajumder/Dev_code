import React, { Component } from 'react';
import SVG from '../../SVG';

class AddressBlock extends Component {
  constructor() {
    super();

    this.changeDataAddress = this.changeDataAddress.bind(this);
  }

  static createAddressElement(content) {
    if (content) return <span>{content}</span>;
    return null;
  }

  changeDataAddress(id) {
    const keys = Object.keys(this.refs);
    const values = Object.values(this.refs);
    const data = { id };

    keys.forEach((key, i) => data[key] = values[i].value);

    this.props.modifyAddress(data);
  }

  render() {
    const { editButtonText, removeButtonText, address, openDialog, dialog, closeDialog } = this.props;

    const street1 = AddressBlock.createAddressElement(address.street1);
    const street2 = AddressBlock.createAddressElement(address.street2);
    const city = AddressBlock.createAddressElement(address.city);
    const state = AddressBlock.createAddressElement(address.state);
    const zip = AddressBlock.createAddressElement(address.zip);

    const footer = <div className="btn-group btn-group--right">
      <button onClick={closeDialog} type="button" className="btn-action btn-action--secondary">{dialog.buttons.discard}</button>
      <button onClick={() => { this.changeDataAddress(address.id || -1); }} type="button" className="btn-action">{dialog.buttons.save}</button>
    </div>;


    const bodyContent = dialog.fields.map((field, index) => {
      const { label, values, type, id } = field;

      const input = type === 'text'
      ? <input type="text"
               ref={id}
               className="input__text"
               placeholder={label}
               defaultValue={address[id]} />

      : <div className="input__select">
          <select ref={id} defaultValue={address[id] || label}>
            <option disabled={true}>{label}</option>
            { values.map(value => <option key={value} value={value}>{value}</option>) }
          </select>
        </div>;

      return (
        <td key={index}>
          <div className="input__wrapper">
            <span className="input__label">{label}</span>
            {input}
          </div>
        </td>
      );
    });

    const body = <table className="cart__dialog-table">
      <tbody>
        <tr>
          {bodyContent}
        </tr>
      </tbody>
    </table>;

    const dialogData = {
      isCloseButton: true,
      footer
    };

    if (Object.keys(address).length === 1) {
      dialogData.headerTitle = dialog.types.add;
      dialogData.body = body;
    } else {
      dialogData.headerTitle = dialog.types.edit;
      dialogData.body = body;
    }

    const editButton = address.isEditButton
      ? <button onClick={() => openDialog(dialogData)} type="button" className="in-card-btn">
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

export default AddressBlock;
