/* eslint-disable */
import React, { Component } from 'react';
import PropTypes from 'prop-types';
import axios from 'axios';
/* components */
import Dialog from 'app.dump/Dialog';
import Button from 'app.dump/Button';
import EditOrder from 'app.dump/Product/EditOrder';
import PaymentMethod from 'app.smart/Checkout/PaymentMethod';
/* consts */
import { FAILURE, SUCCESS } from 'app.consts';

class EditModal extends Component {
  static propTypes = {
    open: PropTypes.bool.isRequired,
    proceedUrl: PropTypes.string.isRequired,
    // func
    closeModal: PropTypes.func.isRequired,
    clearHistory: PropTypes.func.isRequired,
    // ui
    title: PropTypes.string.isRequired,
    description: PropTypes.string,
    buttons: PropTypes.shape({
      proceed: PropTypes.string.isRequired,
      cancel: PropTypes.string.isRequired,
      remove: PropTypes.string.isRequired
    }).isRequired,
    validationMessage: PropTypes.string.isRequired,
    successMessage: PropTypes.string.isRequired,
    orderedItems: PropTypes.arrayOf(PropTypes.shape({
      id: PropTypes.oneOfType([PropTypes.number, PropTypes.string]).isRequired,
      quantity: PropTypes.number.isRequired,
      lineNumber: PropTypes.string.isRequired,
      SKUId: PropTypes.string.isRequired
    }).isRequired).isRequired,
    general: PropTypes.shape({
      orderId: PropTypes.oneOfType([PropTypes.number, PropTypes.string]).isRequired
    }).isRequired,
    paidByCreditCard: PropTypes.bool.isRequired,
    maxOrderQuantity: PropTypes.object.isRequired,
    paymentMethod: PropTypes.shape({
      id: PropTypes.oneOfType([PropTypes.number, PropTypes.string]),
      invoice: PropTypes.string
    }).isRequired,
    changePaymentMethod: PropTypes.func.isRequired,
    paymentMethodUI: PropTypes.object.isRequired
  }

  state = {
    orderedItems: {},
    invalids: [],
    isLoading: false
  };

  componentWillReceiveProps(nextProps) {
    if (this.props.open === nextProps.open) return;
    const orderedItems = {};

    if (nextProps.open) {
      // create orderItems quanty { id: quantity }
      nextProps.orderedItems.forEach((orderedItem) => {
        orderedItems[orderedItem.id] = {
          quantity: orderedItem.quantity,
          remove: false
        };
      });
    }

    this.setState({ orderedItems });
  }

  handleChangeQuantity = (id, quantity) => {
    const quantityNumber = parseFloat(quantity);
    if (quantityNumber < 0) return;
    this.setState({
      orderedItems: {
        ...this.state.orderedItems,
        [id]: {
          ...this.state.orderedItems[id],
          quantity: quantityNumber
        }
      }
    });
  };

  handleRemoveOrder = (id) => {
    this.setState({
      orderedItems: {
        ...this.state.orderedItems,
        [id]: {
          remove: true,
          quantity: 0
        }
      }
    });
  };

  handleChangeQuantityOnlyDecrease = (id, quantity) => {
    if (quantity <= this.props.maxOrderQuantity[id]) {
      this.handleChangeQuantity(id, quantity);
      const invalids = this.state.invalids.filter(invalid => invalid.id !== id);
      this.setState({ invalids });
    } else {
      const recordedInvalid = !!this.state.invalids.find(invalid => invalid.id === id);
      if (recordedInvalid) return;
      this.setState({ invalids: [...this.state.invalids, { id, maxOrderQuantity: this.props.maxOrderQuantity[id] }] });
      setTimeout(() => {
        const invalids = this.state.invalids.filter(invalid => invalid.id !== id);
        this.setState({ invalids });
      }, 3000);
    }
  };

  getFooter = () => (
    <div className="btn-group btn-group--right">
      <Button
        text={this.props.buttons.cancel}
        type="action"
        btnClass="btn-action--secondary"
        onClick={this.props.closeModal}
      />

      <Button
        text={this.props.buttons.proceed}
        type="action"
        onClick={this.submit}
        isLoading={this.state.isLoading}
      />
    </div>
  );

  getOrder = id => this.props.orderedItems.find(item => item.id.toString() === id.toString());

  submit = () => {
    const orderedItems = Object.keys(this.state.orderedItems)
      .filter(id => this.state.orderedItems[id].quantity !== this.getOrder(id).quantity)
      .map((id) => {
        const { lineNumber } = this.getOrder(id);
        return {
          quantity: this.state.orderedItems[id].quantity,
          lineNumber,
          removed: this.state.orderedItems[id].remove
        };
      });

    if (!orderedItems.length) {
      this.props.closeModal();
      return;
    }

    const body = {
      items: orderedItems,
      orderId: this.props.general.orderId
    };

    this.setState({ isLoading: true });

    axios
      .post(this.props.proceedUrl, body)
      .then((response) => {
        const { success, payload, errorMessage } = response.data;
        if (success) {
          // payload might be null if user lacks permissions to see prices
          this.props.editOrders({
            pricingInfo: payload ? payload.pricingInfo : [],
            orderedItems,
            ordersPrice: payload ? payload.ordersPrice : []
          });
          this.props.closeModal();
          this.props.clearHistory();
          window.store.dispatch({
            type: SUCCESS,
            alert: this.props.successMessage
          });
        } else {
          window.store.dispatch({
            type: FAILURE,
            alert: errorMessage
          });
        }

        this.setState({ isLoading: false });
      })
      .catch((error) => {
        window.store.dispatch({ type: FAILURE, error });
        this.setState({ isLoading: false });
      });
  }

  getBody = () => {
    const orders = this.props.orderedItems.filter(item => item.quantity).map((orderedItem) => {
      const isRemoved = !!(
        this.state.orderedItems &&
        this.state.orderedItems[orderedItem.id] &&
        this.state.orderedItems[orderedItem.id].remove
      );
      if (isRemoved) return null;

      if (orderedItem.mailingList) return null;

      return (
        <EditOrder
          key={orderedItem.id}
          {...orderedItem}
          openTooltip={!!this.state.invalids.find(invalid => invalid.id === orderedItem.id)}
          titleTooltip={`${this.props.validationMessage} ${this.props.maxOrderQuantity[orderedItem.id]}`}
          onChange={this.props.paidByCreditCard ? e => this.handleChangeQuantityOnlyDecrease(orderedItem.id, e.target.value) : e => this.handleChangeQuantity(orderedItem.id, e.target.value)}
          removeOrder={() => this.handleRemoveOrder(orderedItem.id)}
          value={this.state.orderedItems && this.state.orderedItems[orderedItem.id] && this.state.orderedItems[orderedItem.id].quantity}
          removeButton={this.props.buttons.remove}
        />
      );
    });

    return (
      <div>
        {this.props.description ? <p>{this.props.description}</p> : null}
        {orders}
        <div className="mt-4">
          <PaymentMethod
            validationMessage="String"
            changePaymentMethod={this.props.changePaymentMethod}
            checkedObj={this.props.paymentMethod}
            ui={this.props.paymentMethodUI}
          />
        </div>
      </div>
    );
  };

  render() {
    return (
      <Dialog
        closeDialog={this.props.closeModal}
        hasCloseBtn={true}
        title={this.props.title}
        body={this.getBody()}
        footer={this.getFooter()}
        open={this.props.open}
      />
    );
  }
}

export default EditModal;
