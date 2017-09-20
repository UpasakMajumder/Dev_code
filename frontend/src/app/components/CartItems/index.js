import React, { Component } from 'react';
import { connect } from 'react-redux';
import PropTypes from 'prop-types';

class CartItems extends Component {
  static propTypes = {
    itemsNumber: PropTypes.number.isRequired
  };

  render() {
    const { itemsNumber } = this.props;
    if (!itemsNumber) return null;
    return (
      <div className="nav-badge">
        <span className="nav-badge__text">{itemsNumber}</span>
      </div>
    );
  }
}

export default connect((state) => {
  const { cartPreview } = state;
  const itemsNumber = cartPreview.items.length;

  return { itemsNumber };
}, {})(CartItems);
