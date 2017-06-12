import React, { Component } from 'react';
import SVG from '../../SVG';

class Products extends Component {
  constructor() {
    super();

    this.state = {
      quantity: 1,
      workingProcess: 0
    };

    this.handleChange = this.handleChange.bind(this);
  }

  componentWillReceiveProps(nextProps) {
    if (nextProps.quantity === this.state.quantity) return;

    this.setState({
      quantity: nextProps.quantity
    });
  }

  handleChange(target) {
    const quantity = target.value;

    if (!quantity) return;

    const { id, stockQuantity } = this.props;

    if (stockQuantity) {
      if (quantity < 1 || quantity > stockQuantity || isNaN(quantity)) return;
    }

    clearTimeout(this.state.workingProcess);

    const workingProcess = setTimeout(() => {
      this.props.changeProductQuantity(id, quantity);
      target.blur();
    }, 1000);

    this.setState({
      workingProcess,
      quantity
    });
  }

  render() {
    const { delivery, id, image, isEditable, isMailingList, mailingList, price, pricePrefix, quantityPrefix, template, removeProduct } = this.props;
    const { quantity } = this.state;

    const productDifference = isMailingList
    ? <div className="cart-product__mlist">
        <p>
          <SVG name="mailing-list"/>
          <span>Mailing list: <strong>{mailingList}</strong></span>
        </p>
      </div>
    : <div className="cart-product__quantity">
        <span>{quantityPrefix}</span>
        <input onChange={(e) => { this.handleChange(e.target); }}
               type="number"
               min="1"
               value={quantity}/>
      </div>;

    const editButton = isEditable
    ? <button type="button" className="cart-product__btn">
        <SVG name="edit"/>
        Edit
      </button>
    : null;

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
            {editButton}

            <button onClick={() => { removeProduct(id); }} type="button" className="cart-product__btn">
              <SVG name="cross--dark"/>
              Remove
            </button>
          </div>
        </div>

      </div>
    );
  }
}

export default Products;
