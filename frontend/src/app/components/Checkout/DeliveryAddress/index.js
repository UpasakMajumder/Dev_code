import React, { Component } from 'react';
import PropTypes from 'prop-types';
/* components */
import Alert from 'app.dump/Alert';
import Button from 'app.dump/Button';
import NewAddressDialog from '../NewAddressDialog';
/* local components */
import Address from './Address';

class DeliveryAddress extends Component {
  state = {
    isDialogOpen: false,
    address: {
      customerName: '',
      street: '',
      city: '',
      state: '',
      zip: '',
      country: '',
      phone: '',
      email: ''
    }
  };

  toggleDialog = () => {
    this.setState((prevState) => {
      return {
        isDialogOpen: !prevState.isDialogOpen,
        address: {
          customerName: '',
          street: '',
          city: '',
          state: '',
          zip: '',
          country: '',
          phone: '',
          email: ''
        }
      };
    });
  };

  submit = () => {
    const { address } = this.state;

    const data = {
      id: -1,
      customerName: address.customerName,
      street: [address.address1, address.address2],
      city: address.city,
      state: address.state,
      zip: address.zip,
      country: address.country,
      phone: address.phone,
      email: address.email
    };

    this.props.addNewAddress(data, this.props.addedDataId);
  };

  changeInput = (value, field) => {
    this.setState({
      address: {
        ...this.state.address,
        [field]: value
      }
    });
  };

  render() {
    const { ui, checkedId, changeShoppingData, disableInteractivity } = this.props;
    const { title, description, newAddress, items, emptyMessage, availableToAdd, dialogUI } = ui;

    const renderAddresses = (item) => {
      return (
        <div key={`da-${item.id}`} className="input__wrapper">
          <Address disableInteractivity={disableInteractivity}
                   changeShoppingData={changeShoppingData}
                   checkedId={checkedId}
                   {...item}
          />
        </div>
      );
    };

    const addresses = items.map(renderAddresses);

    const alert = items.length ? null : <Alert type="grey" text={emptyMessage} />;

    const newAddressBtn = availableToAdd
      ?
      (
        <Button text={newAddress.label} onClick={this.toggleDialog} type="action" btnClass="btn-action--secondary"/>
      )
      :
      (
        <a
          href={newAddress.url}
          data-dialog="#cart-add-adress"
          className="btn-action btn-action--secondary js-dialog">
          {newAddress.label}
        </a>
      );


    return (
      <div>
        {this.state.isDialogOpen && <NewAddressDialog
          address={this.state.address}
          submit={this.submit}
          closeDialog={this.toggleDialog}
          ui={dialogUI}
          changeInput={this.changeInput}
        />}

        <div>
          <h2>{title}</h2>
          <div className="cart-fill__block">
            <p className="cart-fill__info">{description}</p>
            {alert}
            <div className="cart-fill__block-inner cart-fill__block--flex">
              {addresses}
              <div className="btn-group btn-grout--left">
                {newAddressBtn}
              </div>
            </div>
          </div>
        </div>
      </div>
    );
  }
}

DeliveryAddress.propTypes = {
  changeShoppingData: PropTypes.func.isRequired,
  addNewAddress: PropTypes.func.isRequired,
  checkedId: PropTypes.number,
  addedDataId: PropTypes.number,
  disableInteractivity: PropTypes.bool.isRequired,
  ui: PropTypes.shape({
    items: PropTypes.arrayOf(PropTypes.object.isRequired),
    newAddress: PropTypes.shape({
      label: PropTypes.string.isRequired,
      url: PropTypes.string.isRequired
    }).isRequired,
    isDeliverable: PropTypes.bool.isRequired,
    dialogUI: PropTypes.object.isRequired,
    availableToAdd: PropTypes.bool.isRequired,
    description: PropTypes.string.isRequired,
    title: PropTypes.string.isRequired,
    unDeliverableText: PropTypes.string,
    emptyMessage: PropTypes.string
  }).isRequired
};

export default DeliveryAddress;
