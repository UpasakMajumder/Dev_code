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
    isDialogOpen: false
  };

  toggleDialog = () => {
    this.setState(prevState => ({ isDialogOpen: !prevState.isDialogOpen }));
  };

  submitNewAddress = (address) => {
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

    this.props.addNewAddress(data);
  };

  render() {
    const { ui, checkedId, changeShoppingData, disableInteractivity, newAddressObject } = this.props;
    const { title, description, newAddress, items, emptyMessage, availableToAdd, dialogUI, userNotification } = ui;

    const renderAddresses = (item) => {
      return (
        <div key={`da-${item.id}`} className="input__wrapper">
          <Address
            disableInteractivity={disableInteractivity}
            changeShoppingData={changeShoppingData}
            checkedId={checkedId}
            {...item}
          />
        </div>
      );
    };

    let addresses = items.map(renderAddresses);
    if (Object.keys(newAddressObject).length > 0) {
      addresses = [...items, newAddressObject].map(renderAddresses);
    }

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
          className="btn-action btn-action--secondary js-dialog"
        >
          {newAddress.label}
        </a>
      );

    const userNotificationComponent = userNotification ? <Alert type="info" text={userNotification}/> : null;


    return (
      <div>
        {this.state.isDialogOpen && <NewAddressDialog
          submit={this.submitNewAddress}
          closeDialog={this.toggleDialog}
          ui={dialogUI}
          userNotification={userNotification}
        />}

        <div>
          <h2>{title}</h2>
          <div className="cart-fill__block">
            <p className="cart-fill__info">{description}</p>
            {alert}
            {userNotificationComponent}
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
  disableInteractivity: PropTypes.bool.isRequired,
  ui: PropTypes.shape({
    userNotification: PropTypes.string,
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
  }).isRequired,
  newAddressObject: PropTypes.object
};

export default DeliveryAddress;
