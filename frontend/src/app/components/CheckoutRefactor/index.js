import React, { Component } from 'react';
import PropTypes from 'prop-types';
import ImmutablePropTypes from 'react-immutable-proptypes';
import { connect } from 'react-redux';
/* ac */
import {
  initItems,
  removeProduct,
  changeProductQuantity,
  getTotals
} from 'app.ac/checkoutRefactor';
import {
  getUI as updateCartPreview
} from 'app.ac/cartPreview';
import * as appLoader from 'app.ac/appLoader';
/* components */
import Alert from 'app.dump/Alert';
import Products from './Products';
import EmptyCart from './EmptyCart';
import Totals from './Totals';

class Checkout extends Component {
  static propTypes = {
    /* config */
    config: PropTypes.shape({
      welcomeMessage: PropTypes.string,
      products: PropTypes.shape({
        items: PropTypes.array.isRequired
      }).isRequired,
      api: PropTypes.shape({
        removeProduct: PropTypes.string.isRequired,
        changeProductQuantity: PropTypes.string.isRequired,
        getTotals: PropTypes.string.isRequired
      }).isRequired,
      emptyCart: PropTypes.object.isRequired,
      totals: PropTypes.object.isRequired
    }).isRequired,
    /* store */
    store: ImmutablePropTypes.contains({
      products: ImmutablePropTypes.list.isRequired,
      quantityText: PropTypes.string.isRequired,
      totals: ImmutablePropTypes.list.isRequired
    }).isRequired,
    /* func */
    initItems: PropTypes.func.isRequired,
    updateCartPreview: PropTypes.func.isRequired,
    appLoaderStart: PropTypes.func.isRequired,
    appLoaderFinish: PropTypes.func.isRequired,
    getTotals: PropTypes.func.isRequired
  }

  /* lifecycles */
  componentDidMount() {
    const {
      products,
      totals,
      api
    } = this.props.config;

    const fn = async () => {
      await this.props.initItems({
        products: products.items,
        quantityText: products.quantityText,
        totals: totals.items
      });
      await this.props.appLoaderStart();
      await this.props.getTotals(api.getTotals);
      await this.props.appLoaderFinish();
    };

    fn();
  }

  /* ui */
  getWelcomeMessage = () => {
    const { welcomeMessage } = this.props.config;
    let WelcomeMessage = null;

    if (welcomeMessage) {
      WelcomeMessage = (
        <div className="shopping-cart__block">
          <Alert type="grey" text={welcomeMessage}/>
        </div>
      );
    }

    return WelcomeMessage;
  };

  /* handlers */
  removeProduct = async (id) => {
    const url = `${this.props.config.api.removeProduct}/${id}`;
    await this.props.appLoaderStart();
    await this.props.removeProduct(url, id);
    await this.props.updateCartPreview();
    // TODO: get delivery methods prices
    await this.props.getTotals(this.props.config.api.getTotals);
    await this.props.appLoaderFinish();
  };

  changeProductQuantity = async (id, quantity) => {
    const url = `${this.props.config.api.changeProductQuantity}/${id}`;
    await this.props.appLoaderStart();
    await this.props.changeProductQuantity(url, id, quantity);
    await this.props.updateCartPreview();
    // TODO: get delivery methods prices
    await this.props.getTotals(this.props.config.api.getTotals);
    await this.props.appLoaderFinish();
  };

  render() {
    const { config, store } = this.props;

    if (!store.get('products').size) {
      return (
        <EmptyCart
          ui={config.emptyCart}
        />
      );
    }

    return (
      <div>
        {this.getWelcomeMessage()}
        <div className="shopping-cart__block">
          <Products
            ui={config.products}
            items={store.get('products').toJS()}
            removeProduct={this.removeProduct}
            changeProductQuantity={this.changeProductQuantity}
            quantityText={store.get('quantityText')}
          />
        </div>
        <div className="shopping-cart__block">
          <Totals
            ui={config.totals}
            items={store.get('totals').toJS()}
          />
        </div>
      </div>
    );
  }
}

export default connect((state) => {
  const { checkoutRefactor } = state;
  return { store: checkoutRefactor };
}, {
  initItems,
  removeProduct,
  changeProductQuantity,
  updateCartPreview,
  appLoaderStart: appLoader.start,
  appLoaderFinish: appLoader.finish,
  getTotals
})(Checkout);
