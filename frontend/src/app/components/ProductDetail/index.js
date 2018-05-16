import React, { Component } from 'react';
import PropTypes from 'prop-types';
import Immutable from 'immutable';
import ImmutablePropTypes from 'react-immutable-proptypes';
import axios from 'axios';
/* globals */
import { PRODUCT_DETAIL } from 'app.globals';
/* constants */
import { FAILURE } from 'app.consts';
/* components */
import ProductThumbnail from './ProductThumbnail';
import Info from './Info';
import Table from './Table';
import Stock from './Stock';
import Proceed from './Proceed';
import ProductOptions from './ProductOptions';

class ProductDetail extends Component {
  static defaultProps = {
    ui: Immutable.fromJS(PRODUCT_DETAIL)
  };

  static propTypes = {
    ui: ImmutablePropTypes.mapContains({
      thumbnail: ImmutablePropTypes.map.isRequired,
      requiresApproval: ImmutablePropTypes.map.isRequired,
      attachments: ImmutablePropTypes.list,
      info: ImmutablePropTypes.map,
      estimates: ImmutablePropTypes.map,
      dynamicPrices: ImmutablePropTypes.map,
      availability: ImmutablePropTypes.map,
      packagingInfo: PropTypes.string,
      addToCart: ImmutablePropTypes.mapContains({
        quantityText: PropTypes.string,
        quantity: PropTypes.number,
        minQuantity: PropTypes.number,
        maxQuantity: PropTypes.number,
        documentId: PropTypes.string.isRequired,
        url: PropTypes.string.isRequired
      }),
      openTemplate: ImmutablePropTypes.map,
      description: ImmutablePropTypes.mapContains({
        title: PropTypes.string.isRequired,
        text: PropTypes.string.isRequired
      }),
      productOptions: ImmutablePropTypes.mapContains({
        priceUrl: PropTypes.string.isRequired,
        priceElementId: PropTypes.string.isRequired
      })
    }).isRequired
  };

  constructor(props) {
    super(props);

    const options = {};
    const categories = props.ui.getIn(['productOptions', 'categories']);

    if (categories) {
      categories.forEach((category) => {
        const selected = category.get('options').find((option) => {
          return option.get('selected') && !option.get('disabled');
        });

        if (selected) {
          options[category.get('name')] = selected.get('id');
        } else {
          options[category.get('name')] = null;
        }
      });
    }

    this.state = {
      options: Immutable.Map(options),
      optionsPrice: null,
      quantity: this.props.ui.getIn(['addToCart', 'quantity'], 1)
    };
  }

  proceedProduct = () => {
    console.log('🚀');
    // collect body
      // Quantity
      // Options
      // documentId
    // send
    // show notification
    // validation
      // quantity
      // options
  };

  handleChangeQuantity = quantity => this.setState({ quantity });

  handleChangeOptions = (name, value) => {
    const options = this.state.options.set(name, value);
    this.setState({ options }, this.getOptionsPrice);
  };

  getOptionsPrice = () => {
    axios
      .post(this.props.ui.getIn(['productOptions', 'priceUrl']), this.state.options.toJS())
      .then((res) => {
        const { errorMessage, payload, success } = res.data;
        if (success) {
          this.setState({ optionsPrice: `${payload.pricePrefix}${payload.priceValue}` });
        } else {
          window.store.dispatch({
            type: FAILURE,
            alert: errorMessage
          });
        }
      })
      .catch(() => {
        window.store.dispatch({ type: FAILURE });
      });
  };

  render() {
    const { ui } = this.props;

    const packagingInfoComponent = ui.get('packagingInfo')
      ? (
        <span className="add-to-cart__info mr-3">{ui.get('packagingInfo')}</span>
      ) : null;

    const descriptionComponent = ui.get('description')
      ? (
        <div className="block">
          <h2 className="block__heading pt-4 pb-2">{ui.getIn(['description', 'title'])}</h2>
          <p dangerouslySetInnerHTML={{ __html: ui.getIn(['description', 'text']) }} />
        </div>
      ) : null;

    return (
      <div>
        <div className="product-view">
          <ProductThumbnail
            thumbnail={ui.get('thumbnail')}
            attachments={ui.get('attachments')}
            requiresApproval={ui.get('requiresApproval')}
          />
          <div className="product-view__block">
            <Info
              info={ui.get('info')}
            />
            <ProductOptions
              productOptions={ui.get('productOptions')}
              handleChangeOptions={this.handleChangeOptions}
              stateOptions={this.state.options}
            />
            <div className="product-view__tables">
              <Table
                data={ui.get('estimates')}
                optionsPrice={this.state.optionsPrice}
                priceElementId={ui.getIn(['productOptions', 'priceElementId'])}
                estimates
              />
              <Table
                data={ui.get('dynamicPrices')}
                optionsPrice={this.state.optionsPrice}
                priceElementId={ui.getIn(['productOptions', 'priceElementId'])}
              />
            </div>
            <div className="product-view__footer">
              <Stock
                availability={ui.get('availability')}
              />
              {packagingInfoComponent}
              <Proceed
                addToCart={ui.get('addToCart')}
                openTemplate={ui.get('openTemplate')}
                handleChangeQuantity={this.handleChangeQuantity}
                quantity={this.state.quantity}
                proceedProduct={this.proceedProduct}
              />
            </div>
          </div>
        </div>
        {descriptionComponent}
      </div>
    );
  }
}

export default ProductDetail;
