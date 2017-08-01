import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';
/* ac */
import { getUI, togglePreview } from 'app.ac/cartPreview';
/* components */
import CartPreviewProduct from 'app.dump/Product/CartPreview';
import Spinner from 'app.dump/Spinner';

class CartPreview extends Component {
  static propTypes = {
    cartPreview: PropTypes.shape({
      items: PropTypes.arrayOf(PropTypes.object).isRequired,
      emptyCartMessage: PropTypes.string.isRequired,
      isVisible: PropTypes.bool.isRequired,
      cart: PropTypes.shape({
        label: PropTypes.string,
        url: PropTypes.string
      }).isRequired,
      isLoaded: PropTypes.bool.isRequired,
      totalPrice: PropTypes.shape({
        pricePrefix: PropTypes.string,
        price: PropTypes.string
      }).isRequired
    }).isRequired,
    getUI: PropTypes.func.isRequired,
    togglePreview: PropTypes.func.isRequired
  };

  componentDidMount() {
    this.props.getUI();
  }

  render() {
    const { cartPreview, togglePreview } = this.props;
    const { emptyCartMessage, cart, items, isVisible, isLoaded, totalPrice } = cartPreview;

    let content = <Spinner/>;

    if (isLoaded) {
      if (items.length) {
        const products = items.map(product => <CartPreviewProduct key={product.id} {...product} />);

        content = (
          <div>
            <div className="cart-preview__products">
              {products}
            </div>
            <div className="cart-preview__footer">
              <span className="cart-preview__total-price">{totalPrice.pricePrefix} {totalPrice.price}</span>
              <a className="btn-action cart-preview__proceed" href={cart.url}>{cart.label}</a>
            </div>
          </div>
        );
      } else {
        content = (
            <div className="cart-preview__empty">
              <p>{emptyCartMessage}</p>
            </div>
        );
      }
    }

    const preview = <div className="cart-preview__container">{content}</div>;

    return (
      <div onMouseEnter={() => togglePreview(true)}
           onMouseLeave={() => togglePreview(false)}
           className="cart-preview">
        { isVisible ? preview : null}
      </div>
    );
  }

}

export default connect((state) => {
  const { cartPreview } = state;
  return { cartPreview };
}, {
  getUI,
  togglePreview
})(CartPreview);
