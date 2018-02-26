import React, { Component } from 'react';
import PropTypes from 'prop-types';
/* components */
import Alert from 'app.dump/Alert';
import Button from 'app.dump/Button';
import NewAddressDialog from '../NewAddressDialog';
/* local components */
import Address from './Address';

class DeliveryAddress extends Component {
  constructor(props) {
    super(props);

    this.state = {
      isDialogOpen: false,
      addressesNumber: props.ui.bounds.limit
    };
  }

  toggleDialog = () => {
    this.setState(prevState => ({ isDialogOpen: !prevState.isDialogOpen }));
  };

  toggleAddressesNumber = () => {
    this.setState((prevState) => {
      if (prevState.addressesNumber === Math.POSITIVE_INFINITY) {
        return {
          addressesNumber: this.props.ui.bounds.limit
        };
      }
      return {
        addressesNumber: Math.POSITIVE_INFINITY
      };
    });
  };

  render() {
    const { addressesNumber } = this.state;
    const { ui, checkedId, changeDeliveryAddress, disableInteractivity, newAddressObject } = this.props;
    const { title, description, newAddress, items, emptyMessage, availableToAdd, dialogUI, userNotification, bounds } = ui;

    const renderAddresses = (item, i) => {
      if (i + 1 > addressesNumber) return false;

      const country = dialogUI.fields
        .find(field => field.id === 'country')
        .values
        .find(country => country.id === item.country);

      const state = country.values.find(state => state.id === item.state);

      const address = {
        ...item,
        country: country.name,
        state: state && state.name
      };

      return (
        <div key={`da-${item.id}`} className="input__wrapper">
          <Address
            disableInteractivity={disableInteractivity}
            changeDeliveryAddress={changeDeliveryAddress}
            checkedId={checkedId}
            {...address}
          />
        </div>
      );
    };

    let addresses = items;
    if (Object.keys(newAddressObject).length > 0) {
      addresses = [newAddressObject, ...items];
    }

    const addressesComponent = addresses.map(renderAddresses);

    const alert = addresses.length ? null : <Alert type="grey" text={emptyMessage} />;

    const showMoreButton = addresses.length > bounds.limit
      ?
      (
        <Button
          text={addressesNumber === Math.POSITIVE_INFINITY ? bounds.showLessText : bounds.showMoreText}
          onClick={this.toggleAddressesNumber}
          type="action"
          btnClass="btn-action--secondary"
        />
      )
      : null;

    const newAddressBtn = availableToAdd
      ? (
        <Button text={newAddress.label} onClick={this.toggleDialog} type="action" btnClass="btn-action--secondary"/>
      ) : (
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
      <div id="delivery-address">
        {this.state.isDialogOpen && <NewAddressDialog
          submit={this.props.addNewAddress}
          closeDialog={this.toggleDialog}
          ui={dialogUI}
          userNotification={userNotification}
          saveAddress={this.props.saveAddress}
        />}

        <div>
          <h2>{title}</h2>
          <div className="cart-fill__block">
            <p className="cart-fill__info">{description}</p>
            {alert}
            {userNotificationComponent}
            <div className="cart-fill__block-inner cart-fill__block--flex">
              {addressesComponent}
              <div className="cart-fill__btns">
                {newAddressBtn}
                {showMoreButton}
              </div>
            </div>
          </div>
        </div>
      </div>
    );
  }
}

DeliveryAddress.propTypes = {
  changeDeliveryAddress: PropTypes.func.isRequired,
  addNewAddress: PropTypes.func.isRequired,
  checkedId: PropTypes.number,
  disableInteractivity: PropTypes.bool.isRequired,
  saveAddress: PropTypes.func.isRequired,
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
    emptyMessage: PropTypes.string,
    bounds: PropTypes.shape({
      showMoreText: PropTypes.string.isRequired,
      showLessText: PropTypes.string.isRequired,
      limit: PropTypes.number.isRequired
    }).isRequired
  }).isRequired,
  newAddressObject: PropTypes.object
};

export default DeliveryAddress;
