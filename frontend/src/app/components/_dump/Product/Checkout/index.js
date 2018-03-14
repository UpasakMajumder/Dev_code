import React, { Component } from 'react';
import PropTypes from 'prop-types';
/* components */
import SVG from 'app.dump/SVG';
import DefaultImg from 'app.dump/DefaultImg';

class CheckoutProduct extends Component {
  state = {
    quantity: this.props.quantity,
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
    disableInteractivity: PropTypes.bool.isRequired,
    quantityPrefix: PropTypes.string,
    stockQuantity: PropTypes.number,
    isMailingList: PropTypes.bool,
    mailingList: PropTypes.string,
    editorURL: PropTypes.string,
    isEditable: PropTypes.bool,
    delivery: PropTypes.string,
    quantity: PropTypes.number,
    image: PropTypes.string,
    templatePrefix: PropTypes.string.isRequired,
    mailingListPrefix: PropTypes.string.isRequired,
    buttonLabels: PropTypes.shape({
      edit: PropTypes.string.isRequired,
      remove: PropTypes.string.isRequired
    }).isRequired,
    productionTime: PropTypes.string,
    shipTime: PropTypes.string,
    options: PropTypes.arrayOf(PropTypes.object).isRequired
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
    const { isEditable, editorURL, isMailingList, disableInteractivity, buttonLabels } = this.props;

    if (isMailingList) return null;

    if (isEditable) {
      if (editorURL) {
        return (
          <a href={editorURL}
             className="cart-product__btn">
            <SVG name="edit"/>
            {buttonLabels.edit}
          </a>
        );
      }

      return (
        <button type="button"
                disabled={disableInteractivity}
                className="cart-product__btn">
          <div>
            <SVG name="edit"/>
          </div>
          {buttonLabels.edit}
        </button>
      );
    }

    return null;
  };

  getShippingElement = () => {
    const {
      productionTimeLabel,
      shipTimeLabel,
      productionTime,
      shipTime
    } = this.props;

    if (!productionTime && !shipTime) return null;

    const productionTimeElement = productionTime
      ? (
        <span className="mr-3"><strong>{productionTimeLabel}:</strong> {productionTime}</span>
      ) : null;

    const shipTimeElement = shipTime
      ? (
        <span><strong>{shipTimeLabel}:</strong> {shipTime}</span>
      ) : null;

    return (
      <div className="cart-product__shipping">
        {productionTimeElement}
        {shipTimeElement}
      </div>
    );
  };

  getPreviewButton = () => {
    const { preview } = this.props;
    if (!preview) return null;
    if (!preview.exists) return null;
    return (
      <a
        href={preview.url}
        className="cart-product__btn"
        target="_blank"
      >
        <SVG name="eye"/>
        {preview.text}
      </a>
    );
  };

  render() {
    const {
      delivery,
      id,
      image,
      isMailingList,
      mailingList,
      price,
      pricePrefix,
      quantityPrefix,
      template,
      removeProduct,
      isQuantityEditable,
      disableInteractivity,
      templatePrefix,
      mailingListPrefix,
      buttonLabels,
      options
    } = this.props;
    const { quantity } = this.state;

    const optionsElement = options.length
      ? options.map((option, i) => <div key={i}>{option.name}: <strong>{option.value}</strong></div>)
      : null;

    const quantityElement = isQuantityEditable
      ? <input onChange={(e) => { this.handleChange(e.target); }}
               type="number"
               min="1"
               disabled={disableInteractivity}
               value={quantity}/>
      : <span>{quantity}</span>;

    const productDifference = isMailingList
      ? (
        <div className="cart-product__mlist">
          <div>
            <SVG name="mailing-list"/>
            <span>{mailingListPrefix}: <strong>{mailingList}</strong></span>
          </div>
          {optionsElement}
        </div>
      ) : (
        <div className="cart-product__quantity">
          <div>{quantityPrefix} {quantityElement}</div>
          {optionsElement}
        </div>
      );

    const productClassName = isMailingList ? 'cart-product' : 'cart-product--non-deliverable cart-product';

    const deliveryElement = delivery
      ? (
        <div className="cart-product__delivery">
          <p>{delivery}</p>
        </div>
      ) : null;

    const removeButton = (
      <button
        onClick={() => { removeProduct(id); }}
        type="button"
        disabled={disableInteractivity}
        className="cart-product__btn"
      >
        <div>
          <SVG name="cross--dark"/>
        </div>
        {buttonLabels.remove}
      </button>
    );

    return (
      <div className={productClassName}>
        <div className="cart-product__img">
          <DefaultImg img={image} alt={template}/>
        </div>


        <div className="cart-product__content">
          <div className="cart-product__template">
            <p>
              <SVG name="products"/>
              <span>{templatePrefix}: <strong>{template}</strong></span>
            </p>
          </div>

          {productDifference}

          {deliveryElement}

          {this.getShippingElement()}
        </div>

        <div className="cart-product__options">
          <div>
            <div className="cart-product__price">
              <span>{pricePrefix} <span>{price}</span></span>
            </div>

            <div className="cart-product__action">
              {this.defineEditButton()}
              {this.getPreviewButton()}
              {removeButton}
            </div>
          </div>
        </div>

      </div>
    );
  }
}

export default CheckoutProduct;
