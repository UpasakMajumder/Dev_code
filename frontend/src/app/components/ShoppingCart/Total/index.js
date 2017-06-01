import React, { Component } from 'react';
import PropTypes from 'prop-types';
import Price from './Price';

class Total extends Component {
  render() {
    const { ui } = this.props;
    const { title, description, items } = ui;

    const prices = items.map((item, index) => {
      let className = 'summary-table__row';
      if (index === items.length - 1) className += ' summary-table__amount--emphasized';
      return <Price className={className} key={item.title} {...item} />;
    });

    const descriptionElement = description ? <p className="cart-fill__info">{description}</p> : null;

    return (
      <div>
        <h2>{title}</h2>
        <div className="cart-fill__block">
          {descriptionElement}
          <div className="cart-fill__block-inner">
            <div className="summary-table">
              {prices}
            </div>
          </div>
        </div>
      </div>
    );
  }
}

export default Total;
