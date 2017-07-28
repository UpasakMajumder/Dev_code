import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';
/* ac */
import { getUI, togglePreview } from 'app.ac/cartPreview';
/* components */
import CartPreviewProduct from 'app.dump/Product/CartPreview';

class CartPreview extends Component {
  static propTypes = {
    items: PropTypes.arrayOf(PropTypes.object).isRequired,
    togglePreview: PropTypes.func.isRequired,
    getUI: PropTypes.func.isRequired,
    emptyCartMessage: PropTypes.string.isRequired,
    isVisible: PropTypes.bool.isRequired,
    cart: PropTypes.shape({
      label: PropTypes.string,
      url: PropTypes.string
    }).isRequired
  };

  componentDidMount() {
    this.props.getUI();
  }

  render() {
    const { emptyCartMessage, cart, items, isVisible, togglePreview } = this.props;

    let content = (
      <div className="cart-preview__container">
        <div className="cart-preview__empty">
          <p>{emptyCartMessage}</p>
        </div>
      </div>
    );

    if (items.length) {
      const products = items.map(product => <CartPreviewProduct key={product.id} {...product} />);

      content = (
        <div className="cart-preview__container">
          <div className="cart-preview__products">
            {products}
          </div>
          <div className="cart-preview__footer">
            <a className="btn-action cart-preview__proceed" href={cart.url}>{cart.label}</a>
          </div>
        </div>
      );
    }

    return (
      <div onMouseEnter={() => togglePreview(true)}
           onMouseLeave={() => togglePreview(false)}
           className="cart-preview">
        { isVisible ? content : null}
      </div>
    );
  }

}

export default connect((state) => {
  const { cartPreview } = state;
  return { ...cartPreview };
}, {
  getUI,
  togglePreview
})(CartPreview);
