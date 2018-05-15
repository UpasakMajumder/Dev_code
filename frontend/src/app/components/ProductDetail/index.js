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
        url: PropTypes.string.isRequired,
        text: PropTypes.string.isRequired,
        unit: PropTypes.string.isRequired,
        quantity: PropTypes.number,
        minQuantity: PropTypes.number,
        maxQuantity: PropTypes.number,
        quantityText: PropTypes.string
      }),
      openTemplate: ImmutablePropTypes.mapContains({
        url: PropTypes.string.isRequired,
        text: PropTypes.string.isRequired
      }),
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

  render() {
    const { ui } = this.props;
    console.log('ui', ui.toJS());

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
            </div>
          </div>
        </div>
      </div>
    );
  }
}

export default ProductDetail;
