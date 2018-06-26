import React, { Component } from 'react';
import PropTypes from 'prop-types';
import ReactCSSTransitionGroup from 'react-addons-css-transition-group';
import { connect } from 'react-redux';
/* ac */
import { getUI } from 'app.ac/cartPreview';
/* components */
import CartPreviewProduct from 'app.dump/Product/CartPreview';
import Spinner from 'app.dump/Spinner';
/* globals */
import { BUTTONS_UI } from 'app.globals';

class CartPreview extends Component {
  static propTypes = {
    cartPreview: PropTypes.shape({
      items: PropTypes.arrayOf(PropTypes.object).isRequired,
      emptyCartMessage: PropTypes.string.isRequired,
      isVisible: PropTypes.bool.isRequired,
      isLoaded: PropTypes.bool.isRequired,
      summaryPrice: PropTypes.shape({
        pricePrefix: PropTypes.string,
        price: PropTypes.string
      }).isRequired
    }).isRequired,
    getUI: PropTypes.func.isRequired
  };

  componentDidMount() {
    this.props.getUI();
  }

  render() {
    const { cartPreview } = this.props;
    const { emptyCartMessage, items, isVisible, isLoaded, summaryPrice } = cartPreview;

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
              <span className="cart-preview__total-price">{summaryPrice.pricePrefix} {summaryPrice.price}</span>
              <div>
                <a className="btn-action btn-action--secondary" href={BUTTONS_UI.products.url}>{BUTTONS_UI.products.text}</a>
                <a className="btn-action cart-preview__proceed" href={BUTTONS_UI.checkout.url}>{BUTTONS_UI.checkout.text}</a>
              </div>
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
      <ReactCSSTransitionGroup
        transitionName="cart-preview"
        transitionEnterTimeout={400}
        transitionLeaveTimeout={400}
        component="div"
      >
        { isVisible ? preview : null}
      </ReactCSSTransitionGroup>
    );
  }

}

export default connect((state) => {
  const { cartPreview } = state;
  return { cartPreview };
}, {
  getUI
})(CartPreview);
