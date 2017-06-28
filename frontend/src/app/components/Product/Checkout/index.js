import React, { Component } from 'react';
import PropTypes from 'prop-types';
import SVG from '../../SVG';

class CheckoutProduct extends Component {
  state = {
    quantity: 1,
    workingProcess: 0
  };

  static propsTypes = {
    changeProductQuantity: PropTypes.func.isRequired,
    pricePrefix: PropTypes.string.isRequired,
    removeProduct: PropTypes.func.isRequired,
    template: PropTypes.string.isRequired,
    price: PropTypes.string.isRequired,
    id: PropTypes.number.isRequired,
    isQuantityEditable: PropTypes.bool,
    quantityPrefix: PropTypes.string,
    stockQuantity: PropTypes.number,
    isMailingList: PropTypes.bool,
    mailingList: PropTypes.string,
    editorURL: PropTypes.string,
    isEditable: PropTypes.bool,
    delivery: PropTypes.string,
    quantity: PropTypes.number,
    image: PropTypes.string
  };

  componentWillReceiveProps(nextProps) {
    if (nextProps.quantity === this.state.quantity) return;

    this.setState({
      quantity: nextProps.quantity
    });
  }

  handleChange = (target) => {
    const quantity = target.value;
    if (!quantity) return;

    const { id, stockQuantity, changeProductQuantity } = this.props;
    const { workingProcess } = this.state;

    if (stockQuantity) {
      if (quantity < 1 || quantity > stockQuantity || isNaN(quantity)) return;
    }

    clearTimeout(workingProcess);

    const workingProcessId = setTimeout(() => {
      changeProductQuantity(id, quantity);
      target.blur();
    }, 1000);

    this.setState({
      workingProcess: workingProcessId,
      quantity
    });
  };

  defineEditButton = () => {
    const { isEditable, editorURL, isMailingList } = this.props;

    if (isMailingList) return null;

    if (isEditable) {
      if (editorURL) {
        return (
          <a href={editorURL} className="cart-product__btn">
            <SVG name="edit"/>
            Edit
          </a>
        );
      }

      return (
        <button type="button" className="cart-product__btn">
          <div>
            <SVG name="edit"/>
          </div>
          Edit
        </button>
      );
    }

    return null;
  };

  render() {
    const { delivery, id, image, isMailingList, mailingList, price, pricePrefix, quantityPrefix,
      template, removeProduct, isQuantityEditable } = this.props;
    const { quantity } = this.state;

    const quantityElement = isQuantityEditable
      ? <input onChange={(e) => { this.handleChange(e.target); }}
               type="number"
               min="1"
               value={quantity}/>
      : <span>{quantity}</span>;

    const productDifference = isMailingList
      ? <div className="cart-product__mlist">
        <p>
          <SVG name="mailing-list"/>
          <span>Mailing list: <strong>{mailingList}</strong></span>
        </p>
      </div>
      : <div className="cart-product__quantity">
        <span>{quantityPrefix}</span>
        {quantityElement}
      </div>;


    const productClassName = isMailingList ? 'cart-product' : 'cart-product--non-deliverable cart-product';

    const deliveryElement = delivery
      ? <div className="cart-product__delivery">
        <p>{delivery}</p>
      </div>
      : null;

    const imgElement = image
      ? <div className="cart-product__img">
        <img src={image} alt={template} />
      </div>
      : null;

    return (
      <div className={productClassName}>
        {imgElement}

        <div className="cart-product__content">
          <div className="cart-product__template">
            <p>
              <SVG name="products"/>
              <span>Template: <strong>{template}</strong></span>
            </p>
          </div>

          {productDifference}

          {deliveryElement}
        </div>

        <div className="cart-product__options">
          <div className="cart-product__price">
            <span>{pricePrefix} <span>{price}</span></span>
          </div>

          <div className="cart-product__action">
            {this.defineEditButton()}

            <button onClick={() => { removeProduct(id); }} type="button" className="cart-product__btn">
              <div>
                <SVG name="cross--dark"/>
              </div>
              Remove
            </button>
          </div>
        </div>

      </div>
    );
  }
}

export default CheckoutProduct;
