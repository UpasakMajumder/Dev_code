import React, { Component } from 'react';
import PropTypes from 'prop-types';
/* components */
import Alert from 'app.dump/Alert';
/* local components */
import Method from './Method';

class PaymentMethod extends Component {
  constructor(props) {
    super(props);

    this.state = { shownInput: props.ui.items.find(item => item.checked).id };
  }

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

  toggleInput = (shownInput) => {
    this.setState({ shownInput });
  };

  render() {
    const { ui, checkedObj, changePaymentMethod, validationMessage } = this.props;
    const { title, description, items, isPayable, unPayableText } = ui;

    const descriptionElement = description ? <p className="cart-fill__info">{description}</p> : null;

    const methods = items.map((item) => {
      let className = 'input__wrapper input__wrapper--icon-label';
      if (item.hasInput) className += ' cart-fill__block-input-wrapper';

      return (
        <Method
          changePaymentMethod={changePaymentMethod}
          checkedObj={checkedObj}
          toggleInput={this.toggleInput}
          shownInput={this.state.shownInput}
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
