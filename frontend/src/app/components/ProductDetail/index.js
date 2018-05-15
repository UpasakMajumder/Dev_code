import React, { Component } from 'react';
import PropTypes from 'prop-types';
import Immutable from 'immutable';
import ImmutablePropTypes from 'react-immutable-proptypes';
/* globals */
import { PRODUCT_DETAIL } from 'app.globals';
/* components */
import ProductThumbnail from './ProductThumbnail';

class ProductDetail extends Component {
  static defaultProps = {
    ui: Immutable.fromJS(PRODUCT_DETAIL)
  };

  static propTypes = {
    ui: ImmutablePropTypes.mapContains({
      thumbnail: ImmutablePropTypes.map.isRequired,
      requiresApproval: ImmutablePropTypes.map.isRequired,
      attachments: ImmutablePropTypes.list,
      info: ImmutablePropTypes.mapContains({
        createdDate: PropTypes.string,
        code: PropTypes.string
      }),
      estimates: ImmutablePropTypes.mapContains({
        header: PropTypes.string.isRequired,
        body: ImmutablePropTypes.listOf(ImmutablePropTypes.mapContains({
          key: PropTypes.string.isRequired,
          value: PropTypes.string.isRequired
        }).isRequired).isRequired
      }),
      dynamicPrices: ImmutablePropTypes.mapContains({
        header: PropTypes.string.isRequired,
        body: ImmutablePropTypes.listOf(ImmutablePropTypes.mapContains({
          id: PropTypes.string.isRequired,
          key: PropTypes.string.isRequired,
          value: PropTypes.string.isRequired
        }).isRequired).isRequired
      }),
      availability: ImmutablePropTypes.mapContains({
        type: PropTypes.oneOf(['unavailable', 'outofstock', 'available']).isRequired,
        text: PropTypes.string.isRequired
      }),
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
        </div>
      </div>
    );
  }
}

export default ProductDetail;
