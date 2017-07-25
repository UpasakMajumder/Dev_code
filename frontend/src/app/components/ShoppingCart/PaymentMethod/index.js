import React, { Component } from 'react';
import PropTypes from 'prop-types';
import Method from './Method';
import Alert from '../../Alert';

class PaymentMethod extends Component {
  componentDidMount() {
    const { ui, changeShoppingData } = this.props;

    if (ui.isPayable) return;
    changeShoppingData('paymentMethod', 3, ' ');
  }

  render() {
    const { ui, checkedObj, changeShoppingData, validationMessage } = this.props;
    const { title, description, items, isPayable, unPayableText } = ui;

    const descriptionElement = description ? <p className="cart-fill__info">{description}</p> : null;

    const methods = items.map((item) => {
      let className = 'input__wrapper input__wrapper--icon-label';
      if (item.hasInput) className += ' cart-fill__block-input-wrapper';
      return (
        <Method changeShoppingData={changeShoppingData}
                checkedObj={checkedObj}
                {...item}
                className={className}
                validationMessage={validationMessage}
                key={`pm-${item.id}`} />
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
      <div>
        <h2>{title}</h2>
        {content}
      </div>
    );
  }
}

export default PaymentMethod;
