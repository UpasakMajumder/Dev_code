import React, { Component } from 'react';
import PropTypes from 'prop-types';
/* components */
import Dialog from 'app.dump/Dialog';
import Button from 'app.dump/Button';

class EditModal extends Component {
  static propTypes = {
    open: PropTypes.bool.isRequired,
    // func
    closeModal: PropTypes.func.isRequired,
    // ui
    title: PropTypes.string.isRequired,
    description: PropTypes.string,
    buttons: PropTypes.shape({
      proceed: PropTypes.string.isRequired,
      cancel: PropTypes.string.isRequired
    }).isRequired,
    orderedItems: PropTypes.arrayOf(PropTypes.shape({
      id: PropTypes.oneOfType([PropTypes.number, PropTypes.string]).isRequired,
      removeButton: PropTypes.string.isRequired,
      quantityPrefix: PropTypes.string.isRequired,
      template: PropTypes.string.isRequired,
      quantity: PropTypes.number.isRequired,
      image: PropTypes.string.isRequired,
      price: PropTypes.string.isRequired,
      templatePrefix: PropTypes.string.isRequired,
      unitOfMeasure: PropTypes.string.isRequired
    }).isRequired).isRequired,
    paidByCreditCard: PropTypes.bool.isRequired
  }

  state = { orderedItems: {} };

  componentWillReceiveProps(nextProps) {
    if (this.props.open === nextProps.open) return;
    const orderedItems = {};

    if (nextProps.open) {
      // create orderItems quanty { id: quantity }
      nextProps.orderedItems.forEach(orderedItem => orderedItems[orderedItem.id] = orderedItem.quantity);
    }

    this.setState({ orderedItems });
  }

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
      />
    </div>
  );

  getBody = () => {
    return (
      <div>
        {this.props.description ? <p>{this.props.description}</p> : null}
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
