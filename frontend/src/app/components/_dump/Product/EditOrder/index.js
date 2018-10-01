import React, { Component } from 'react';
import PropTypes from 'prop-types';
/* components */
import changePaymentMethod from 'app.ac/checkout';
import Alert from 'app.dump/Alert';
import Dialog from 'app.dump/Dialog';
import { Tooltip } from 'react-tippy';
import TextInput from 'app.dump/Form/TextInput';
import PaymentMethod from '../../../Checkout/PaymentMethod';

const EditOrder = (props) => {
  const removeButton = props.removeButton
    ? (
      <button
        type="button"
        className="cart-product__btn mt-2"
        onClick={props.removeOrder}
      >
        {props.removeButton}
      </button>
    ) : null;

  const validationMessage = 'hello';
  const checkedData = 'aaa';
  const paymentMethods = { approvalRequiredButton: 'Confirm', approvalRequiredText: 'Approval Required', isPayable: true, items: [{ id: 1, icon: 'credit-card', approvalRequired: false, title: 'Credit card', items: [{ id: '', label: 'XYZ' }] }, { id: 3, icon: 'order-payment', approvalRequired: false, title: 'Purchase Order', items: [{ id: '', label: 'XYZ' }] }] };
  return (
    <div className="edit-order">
      <img
        src={props.image}
        className="edit-order__image"
        alt={props.template}
      />
      <div className="edit-order__info">
        <p>{props.templatePrefix}: <strong>{props.template}</strong></p>
        <p className="edit-order__price">{props.price}</p>
        <div className="edit-order__quantity">
          {props.quantityPrefix}
          <Tooltip
            title={props.titleTooltip}
            position="bottom"
            animation="fade"
            open={props.openTooltip}
            arrow
            theme="danger"
          >
            <TextInput
              type="number"
              onChange={props.onChange}
              value={props.value}
            />
          </Tooltip>
          {props.unitOfMeasure}
        </div>
        {removeButton}
      </div><div><PaymentMethod
        validationMessage={validationMessage}
        changePaymentMethod={changePaymentMethod}
        checkedObj={checkedData}
        ui={paymentMethods} />
     </div>
    </div>
  );
};

EditOrder.propTypes = {
  quantityPrefix: PropTypes.string.isRequired,
  template: PropTypes.string.isRequired,
  quantity: PropTypes.number.isRequired,
  image: PropTypes.string.isRequired,
  price: PropTypes.string.isRequired,
  templatePrefix: PropTypes.string.isRequired,
  unitOfMeasure: PropTypes.string.isRequired,
  openTooltip: PropTypes.bool.isRequired,
  titleTooltip: PropTypes.string.isRequired,
  onChange: PropTypes.func.isRequired,
  value: PropTypes.oneOfType([PropTypes.string, PropTypes.number]),
  removeButton: PropTypes.string,
  removeOrder: PropTypes.func
};

export default EditOrder;
