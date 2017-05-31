import React, { Component } from 'react';
import PropTypes from 'prop-types';
import Method from './Method';

class PaymentMethod extends Component {
  render() {
    const { ui, checkedObj, changeShoppingData, validationField, validationMessage } = this.props;
    const { title, description, items } = ui;

    const descriptionElement = description ? <p className="cart-fill__info">{description}</p> : null;

    const methods = items.map((item) => {
      let className = 'input__wrapper input__wrapper--icon-label';
      if (item.hasInput) className += ' cart-fill__block-input-wrapper';
      return (
        <Method changeShoppingData={changeShoppingData}
                checkedObj={checkedObj}
                {...item}
                className={className}
                validationField={validationField}
                validationMessage={validationMessage}
                key={`pm-${item.id}`} />
      );
    });

    return (
      <div>
        <h2>{title}</h2>
        <div className="cart-fill__block">
          {descriptionElement}
          <div className="cart-fill__block-inner">
            {methods}
          </div>
        </div>
      </div>
    );
  }
}

export default PaymentMethod;
