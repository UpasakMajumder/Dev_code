import React, { Component } from 'react';
import PropTypes from 'prop-types';
import axios from 'axios';
/* components */
import Dialog from 'app.dump/Dialog';
import Button from 'app.dump/Button';
import EditOrder from 'app.dump/Product/EditOrder';

class EditModal extends Component {
  static propTypes = {
    open: PropTypes.bool.isRequired,
    proceedUrl: PropTypes.string.isRequired,
    // func
    closeModal: PropTypes.func.isRequired,
    editOrders: PropTypes.func.isRequired,
    // ui
    title: PropTypes.string.isRequired,
    description: PropTypes.string,
    buttons: PropTypes.shape({
      proceed: PropTypes.string.isRequired,
      cancel: PropTypes.string.isRequired,
      remove: PropTypes.string.isRequired
    }).isRequired,
    validationMessage: PropTypes.string.isRequired,
    orderedItems: PropTypes.arrayOf(PropTypes.shape({
      id: PropTypes.oneOfType([PropTypes.number, PropTypes.string]).isRequired,
      quantity: PropTypes.number.isRequired
    }).isRequired).isRequired,
    paidByCreditCard: PropTypes.bool.isRequired
  }

  state = {
    orderedItems: null,
    maxQuantity: null,
    invalids: [],
    isLoading: false
  };

  componentWillReceiveProps(nextProps) {
    if (this.props.open === nextProps.open) return;
    const orderedItems = {};
    const maxQuantity = {};

    if (nextProps.open) {
      // create orderItems quanty { id: quantity }
      nextProps.orderedItems.forEach((orderedItem) => {
        orderedItems[orderedItem.id] = orderedItem.quantity;
        maxQuantity[orderedItem.id] = orderedItem.quantity;
      });
    }

    this.setState({ orderedItems, maxQuantity });
  }

  handleChangeQuantity = (id, quantity) => {
    const quantityNumber = parseFloat(quantity);
    if (quantityNumber < 0) return;
    this.setState({ orderedItems: { ...this.state.orderedItems, [id]: quantityNumber } });
  };

  handleChangeQuantityOnlyDecrease = (id, quantity) => {
    if (quantity <= this.state.maxQuantity[id]) {
      this.handleChangeQuantity(id, quantity);
      const invalids = this.state.invalids.filter(invalid => invalid.id !== id);
      this.setState({ invalids });
    } else {
      const recordedInvalid = !!this.state.invalids.find(invalid => invalid.id === id);
      if (recordedInvalid) return;
      this.setState({ invalids: [...this.state.invalids, { id, maxQuantity: this.state.maxQuantity[id] }] });
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

  // validate messages
  // validate input
  // change orders
  // change taxes

  submit = () => {
    this.setState({ isLoading: true });
    const body = {
      orderedItems: { ...this.state.orderedItems }
    };

    axios
      .post(this.props.proceedUrl, body)
      .then((response) => {
        const { success, payload, errorMessage } = response.data;
        if (success) {
          this.props.editOrders({
            pricingInfo: payload.pricingInfo,
            orderedItems: this.state.orderedItems
          });
          this.props.closeModal();
        } else {
          // TODO: errorMessage
        }

        this.setState({ isLoading: false });
      })
      .catch(() => {
        // TODO: toastr
        this.setState({ isLoading: false });
      });
  }

  getBody = () => {
    const orders = this.props.orderedItems.map((orderedItem) => {
      return (
        <EditOrder
          key={orderedItem.id}
          {...orderedItem}
          openTooltip={!!this.state.invalids.find(invalid => invalid.id === orderedItem.id)}
          titleTooltip={`${this.props.validationMessage} ${this.state.maxQuantity && this.state.maxQuantity[orderedItem.id]}`}
          onChange={this.props.paidByCreditCard ? e => this.handleChangeQuantityOnlyDecrease(orderedItem.id, e.target.value) : e => this.handleChangeQuantity(orderedItem.id, e.target.value)}
          value={this.state.orderedItems && this.state.orderedItems[orderedItem.id]}
          removeButton={this.props.buttons.remove}
        />
      );
    });

    return (
      <div>
        {this.props.description ? <p>{this.props.description}</p> : null}
        {orders}
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
