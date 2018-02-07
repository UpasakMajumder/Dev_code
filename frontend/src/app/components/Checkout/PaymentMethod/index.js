import React, { Component } from 'react';
import PropTypes from 'prop-types';
/* components */
import Alert from 'app.dump/Alert';
/* local components */
import MethodsGroup from './MethodsGroup';

class PaymentMethod extends Component {
  static propTypes = {
    validationMessage: PropTypes.string.isRequired,
    changeShoppingData: PropTypes.func.isRequired,
    checkedObj: PropTypes.object.isRequired,
    ui: PropTypes.shape({
      title: PropTypes.string.isRequired,
      items: PropTypes.arrayOf(PropTypes.object),
      unPayableText: PropTypes.string,
      description: PropTypes.string,
      isPayable: PropTypes.bool
    })
  };

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
      let className = 'select-accordion__item input__wrapper input__wrapper--icon-label';
      if (item.hasInput) className += ' cart-fill__block-input-wrapper';

      return (
        <MethodsGroup
          changeShoppingData={changeShoppingData}
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
      <div>
        <h2>{title}</h2>
        {content}
      </div>
    );
  }
}

export default PaymentMethod;
