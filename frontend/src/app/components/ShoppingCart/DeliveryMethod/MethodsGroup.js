import React, { Component } from 'react';
import PropTypes from 'prop-types';
import ReactCSSTransitionGroup from 'react-addons-css-transition-group';
import Method from './Method';
import SVG from '../../SVG';

class MethodsGroup extends Component {
  render() {
    const { id, title, icon, disabled, pricePrefix, price, datePrefix, date, items, checkedId,
      changeShoppingData, openId, changeOpenId } = this.props;

    const methods = items.map((item) => {
      return <Method changeShoppingData={changeShoppingData} checkedId={checkedId} key={`m-${item.id}`} {...item} />;
    });

    let className = 'input__wrapper select-accordion__item';

    if (openId === id) className += ' isActive';

    if (disabled) className += ' input__wrapper--disabled';

    const priceElement = (pricePrefix && price)
      ? <span>{pricePrefix} {price}</span>
      : (pricePrefix)
      ? <span>{pricePrefix}</span>
      : (price)
      ? <span>{price}</span>
      : null;

    const dateElement = (datePrefix && date)
      ? <span> | <span>{datePrefix} {date}</span></span>
      : (datePrefix)
      ? <span> | <span>{datePrefix}</span></span>
      : (date)
      ? <span> | <span>{date}</span></span>
      : null;

    const extraInfo = (priceElement || dateElement)
      ? <span>({priceElement}{dateElement})</span>
      : null;

    return (
      <div>
        <div className={className}>
          <input disabled={disabled}
                 onChange={() => changeOpenId(id)}
                 type="radio"
                 name="methodGroup"
                 className="input__radio"
                 id={`mg-${id}`} />
          <label htmlFor={`mg-${id}`} className="input__label input__label--radio select-accordion__main-label">
            <SVG name={icon} className="icon-shipping"/>
            {title}
            <span className="select-accordion__inner-label">
              {extraInfo}
            </span>
          </label>
        </div>

        <ReactCSSTransitionGroup
          transitionName="select-accordion__content--animation"
          component="div"
          transitionEnterTimeout={700}
          transitionLeaveTimeout={700}>
          {
            openId === id
            ? <div className="select-accordion__content">{methods}</div>
            : null
          }
        </ReactCSSTransitionGroup>
      </div>
    );
  }
}

export default MethodsGroup;
