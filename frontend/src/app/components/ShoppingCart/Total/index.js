import React, { Component } from 'react';
import PropTypes from 'prop-types';
import PriceTable from '../../PriceTable';

class Total extends Component {
  render() {
    const { ui } = this.props;
    const { title, description, items } = ui;

    const descriptionElement = description ? <p className="cart-fill__info">{description}</p> : null;

    return (
      <div>
        <h2>{title}</h2>
        <div className="cart-fill__block">
          {descriptionElement}
          <div className="cart-fill__block-inner">
            <div className="cart-fill__summary-table">
              <PriceTable items={items} />
            </div>
          </div>
        </div>
      </div>
    );
  }
}

export default Total;
