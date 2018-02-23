import React, { Component } from 'react';
import PropTypes from 'prop-types';
/* components */
import Alert from 'app.dump/Alert';
/* local components */
import MethodsGroup from './MethodsGroup';

class PaymentMethod extends Component {
  static propTypes = {
    validationMessage: PropTypes.string.isRequired,
    changePaymentMethod: PropTypes.func.isRequired,
    checkedObj: PropTypes.object.isRequired,
    ui: PropTypes.shape({
      title: PropTypes.string.isRequired,
      items: PropTypes.arrayOf(PropTypes.object),
      unPayableText: PropTypes.string,
      description: PropTypes.string,
      isPayable: PropTypes.bool
    })
  };

  render() {
    const { ui, checkedObj, changePaymentMethod, validationMessage } = this.props;
    const { title, description, items, isPayable, unPayableText } = ui;

    const descriptionElement = description ? <p className="cart-fill__info">{description}</p> : null;

    const methods = items.map((item) => {
      let className = 'select-accordion__item input__wrapper input__wrapper--icon-label';
      if (item.hasInput) className += ' cart-fill__block-input-wrapper';

      return (
        <MethodsGroup
          changePaymentMethod={changePaymentMethod}
          checkedObj={checkedObj}
          {...item}
          className={className}
          validationMessage={validationMessage}
          key={`pm-${item.id}`}
        />
      );
    });

    const content = isPayable
    ? <div className="cart-fill__block">
        {descriptionElement}
        <div className="cart-fill__block-inner">
          {methods}
        </div>
      </div>
    : <Alert type="grey" text={unPayableText} />;

    return (
      <div id="payment-method">
        <h2>{title}</h2>
        {content}
      </div>
    );
  }
}

export default PaymentMethod;
