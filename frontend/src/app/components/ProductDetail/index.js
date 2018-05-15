import React, { Component } from 'react';
import PropTypes from 'prop-types';
import Immutable from 'immutable';
import ImmutablePropTypes from 'react-immutable-proptypes';
/* globals */
import { PRODUCT_DETAIL } from 'app.globals';
/* components */
import ProductThumbnail from './ProductThumbnail';
import Info from './Info';
import Table from './Table';
import Stock from './Stock';
import Proceed from './Proceed';

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
        maxQuantity: PropTypes.number
      }),
      openTemplate: ImmutablePropTypes.map,
      description: ImmutablePropTypes.mapContains({
        titlte: PropTypes.string.isRequired,
        text: PropTypes.string.isRequired
      }),
    }).isRequired
    // TODO: Product options
  };

  state = {
    dynamicPrices: { ...this.props.ui.getIn(['dynamicPrices', 'body']) }, // Immutabe.Map
    quantity: this.props.ui.getIn(['addToCart', 'quantity'], 1),
  }

  handleChangeQuantity = quantity => this.setState({ quantity });

  render() {
    const { ui } = this.props;
    console.log('ui', ui.toJS());

    const packagingInfoComponent = ui.get('packagingInfo')
      ? (
        <span className="add-to-cart__info mr-3">{ui.get('packagingInfo')}</span>
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
            <div className="product-view__tables">
              <Table
                data={ui.get('estimates')}
                estimates
              />
              <Table
                data={ui.get('dynamicPrices')}
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
              />
            </div>
          </div>
        </div>
      </div>
    );
  }
}

export default ProductDetail;
