import React, { Component } from 'react';
import PropTypes from 'prop-types';
import ReactCSSTransitionGroup from 'react-addons-css-transition-group';
/* components */
import SVG from 'app.dump/SVG';
import Method from './Method';

class MethodsGroup extends Component {
  static propTypes = {
    validationMessage: PropTypes.string.isRequired,
    changePaymentMethod: PropTypes.func.isRequired,
    toggleDialog: PropTypes.func.isRequired,
    className: PropTypes.string.isRequired,
    checked: PropTypes.bool.isRequired,
    title: PropTypes.string.isRequired,
    icon: PropTypes.string.isRequired,
    id: PropTypes.number.isRequired,
    inputPlaceholder: PropTypes.string,
    disabled: PropTypes.bool,
    hasInput: PropTypes.bool,
    approvalRequired: PropTypes.bool,
    checkedObj: PropTypes.shape({
      id: PropTypes.number.isRequired,
      invoice: PropTypes.string,
      card: PropTypes.string
    }).isRequired,
    items: PropTypes.arrayOf(PropTypes.object.isRequired)
  };

  changePaymentCard = (id, card) => {
    this.props.changePaymentMethod(id, undefined, card);
  };

  render() {
    const {
      title,
      icon,
      disabled,
      id,
      inputPlaceholder,
      checkedObj,
      changePaymentMethod,
      approvalRequired,
      hasInput,
      toggleDialog,
      approvalRequiredText,
      items
    } = this.props;
    let { className } = this.props;

    const approvalNotice = approvalRequired
      ? (
        <div className="select-accordion__outer-note ml-2"><span className="font-weight-bold">{approvalRequiredText}</span></div>
      ) : null;

    const additionalInput = checkedObj.id === id && hasInput
      ? (
        <div className="input__wrapper">
          <input
                onChange={(e) => { changePaymentMethod(id, e.target.value); }}
                type="text"
                className="input__text"
                name="paymentMethodInvoice"
                placeholder={inputPlaceholder}
                value={checkedObj.invoice}
          />
        </div>
      ) : null;

    if (disabled) className += ' input__wrapper--disabled';
    if (checkedObj.id === id && items.length) className += ' isActive';

    const methods = items.map((item) => {
      return (
        <Method
          key={item.id}
          {...item}
          checked={checkedObj.card === item.id}
          changePaymentCard={(card) => { this.changePaymentCard(id, card); }}
        />
      );
    });

    return (
      <div>
        <div className={className}>
          <input disabled={disabled}
                onChange={(e) => { changePaymentMethod(id); }}
                onClick={approvalRequired ? (e) => { toggleDialog(); } : undefined }
                checked={id === checkedObj.id}
                id={`pmg-${id}`}
                name="paymentMethod"
                type="radio"
                className="input__radio" />
          <label htmlFor={`pmg-${id}`} className="input__label input__label--radio select-accordion__main-label">
            <SVG name={icon} className='icon-shipping' />
            <span>{title}</span>
          </label>
          {additionalInput}
          {approvalNotice}
        </div>

        <ReactCSSTransitionGroup
          transitionName="select-accordion__content--animation"
          component="div"
          transitionEnterTimeout={700}
          transitionLeaveTimeout={700}>
          {
            id === checkedObj.id && items.length
            ? <div className="select-accordion__content">{methods}</div>
            : null
          }
        </ReactCSSTransitionGroup>
      </div>
    );
  }
}

export default MethodsGroup;
