import React, { Component } from 'react';
import TextInput from './TextInput';
import SVG from '../SVG/index';
import PropTypes from 'prop-types';
import { CARD_PAYMENT } from '../../globals';

class PaymentForm extends Component {
  constructor() {
    super();
    this.getErrorMessage = this.getErrorMessage.bind(this);
  }

  getErrorMessage(field) {
    const { errorField, errorMessage } = this.props;
    return (errorField === field) ? errorMessage : '';
  }

  render() {
    const { number, name, cvv, expiry, changeFieldValue, changeFocus } = this.props;
    const cardNumbersSvgs = <div className="card-payment__icon-block">
      <SVG className='card-payment__icon' name='card-am'/>
      <SVG className='card-payment__icon' name='card-mc'/>
      <SVG className='card-payment__icon' name='card-vs'/>
    </div>;
    const cvvSvgs = <div className="card-payment__icon-block">
      <SVG className='card-payment__icon' name='card'/>
    </div>;

    return (
      <div>
        <div className="card-payment__field">
          <TextInput
            onChange={(e) => { changeFieldValue('number', e.target.value); }}
            changeFocusedField={() => { changeFocus('number'); }}
            label={CARD_PAYMENT.fields.number.label}
            value={number}
            labelAnimation={true}
            error={this.getErrorMessage('number')}
            innerElement={cardNumbersSvgs}/>

        </div>

        <div className="card-payment__field">
          <TextInput
            onChange={(e) => { changeFieldValue('name', e.target.value); }}
            changeFocusedField={() => { changeFocus('name'); }}
            label={CARD_PAYMENT.fields.name.label}
            labelAnimation={true}
            error={this.getErrorMessage('name')}
            value={name} />
        </div>

        <div className="card-payment__field card-payment__field--half">
          <TextInput
            onChange={(e) => { changeFieldValue('cvv', e.target.value); }}
            changeFocusedField={() => { changeFocus('cvv'); }}
            label={CARD_PAYMENT.fields.cvv.label}
            value={cvv}
            labelAnimation={true}
            error={this.getErrorMessage('cvv')}
            innerElement={cvvSvgs} />
        </div>

        <div className="card-payment__field card-payment__field--half">
          <TextInput
            onChange={(e) => { changeFieldValue('expiry', e.target.value); }}
            changeFocusedField={() => { changeFocus('expiry'); }}
            label={CARD_PAYMENT.fields.expiry.label}
            labelAnimation={true}
            error={this.getErrorMessage('expiry')}
            value={expiry} />
        </div>
      </div>
    );
  }
}

PaymentForm.propTypes = {
  number: PropTypes.string.isRequired,
  name: PropTypes.string.isRequired,
  cvv: PropTypes.string.isRequired,
  expiry: PropTypes.string.isRequired,
  changeFieldValue: PropTypes.func.isRequired,
  changeFocus: PropTypes.func.isRequired
};

export default PaymentForm;
