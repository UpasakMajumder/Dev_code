import React, { Component } from 'react';
import TextInput from '../form/TextInput';
import SVG from '../SVG';
import PropTypes from 'prop-types';

class PaymentForm extends Component {
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
              label={'Credit card number'}
              value={number}
              labelAnimation={true}
              innerElement={cardNumbersSvgs}/>

          </div>

          <div className="card-payment__field">
            <TextInput
              onChange={(e) => { changeFieldValue('name', e.target.value); }}
              onFocus={() => { changeFocus('name'); }}
              label={'Full name'}
              labelAnimation={true}
              value={name} />
          </div>

          <div className="card-payment__field card-payment__field--half">
            <TextInput
              onChange={(e) => { changeFieldValue('cvc', e.target.value); }}
              onFocus={() => { changeFocus('cvc'); }}
              label={'CVC'}
              value={cvc}
              labelAnimation={true}
              innerElement={cvcSvgs} />
          </div>

          <div className="card-payment__field card-payment__field--half">
            <TextInput
              onChange={(e) => { changeFieldValue('expiry', e.target.value); }}
              onFocus={() => { changeFocus('expiry'); }}
              label={'Expiration date'}
              labelAnimation={true}
              value={expiry} />
          </div>
        </div>
      )
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
