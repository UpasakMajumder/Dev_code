import React, { Component } from 'react';
import PropTypes from 'prop-types';
/* component */
import SVG from 'app.dump/SVG';
import TextInput from 'app.dump/Form/TextInput';
/* globals */
import { CARD_PAYMENT } from 'app.globals';

class PaymentForm extends Component {
  getErrorMessage = (field) => {
    const { errorField, errorMessage } = this.props;
    return (errorField === field) ? errorMessage : '';
  };

  render() {
    const { number, name, cvc, expiry, changeFieldValue, changeFocus } = this.props;
    const cardNumbersSvgs = <div className="card-payment__icon-block">
      <SVG className='card-payment__icon' name='card-am'/>
      <SVG className='card-payment__icon' name='card-mc'/>
      <SVG className='card-payment__icon' name='card-vs'/>
    </div>;
    const cvcSvgs = <div className="card-payment__icon-block">
      <SVG className='card-payment__icon' name='card'/>
    </div>;

    return (
      <div>
        <div className="card-payment__field">
          <TextInput
            onChange={(e) => { changeFieldValue('number', e.target.value); }}
            onFocus={() => { changeFocus('number'); }}
            label={CARD_PAYMENT.fields.number.label}
            value={number}
            error={this.getErrorMessage('number')}
          />
        </div>

        <div className="card-payment__field">
          <TextInput
            onChange={(e) => { changeFieldValue('name', e.target.value); }}
            onFocus={() => { changeFocus('name'); }}
            label={CARD_PAYMENT.fields.name.label}
            error={this.getErrorMessage('name')}
            value={name}
          />
        </div>

        <div className="card-payment__field card-payment__field--half">
          <TextInput
            onChange={(e) => { changeFieldValue('cvc', e.target.value); }}
            onFocus={() => { changeFocus('cvc'); }}
            label={CARD_PAYMENT.fields.cvc.label}
            value={cvc}
            error={this.getErrorMessage('cvc')}
          />
        </div>

        <div className="card-payment__field card-payment__field--half">
          <TextInput
            onChange={(e) => { changeFieldValue('expiry', e.target.value); }}
            onFocus={() => { changeFocus('expiry'); }}
            label={CARD_PAYMENT.fields.expiry.label}
            error={this.getErrorMessage('expiry')}
            value={expiry}
          />
        </div>
      </div>
    );
  }
}

PaymentForm.propTypes = {
  number: PropTypes.string.isRequired,
  name: PropTypes.string.isRequired,
  cvc: PropTypes.string.isRequired,
  expiry: PropTypes.string.isRequired,
  changeFieldValue: PropTypes.func.isRequired,
  changeFocus: PropTypes.func.isRequired
};

export default PaymentForm;
