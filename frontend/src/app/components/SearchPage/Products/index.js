import React, { Component } from 'react';
import { connect } from 'react-redux';
import Spinner from '../../Spinner';
import { getUI } from '../../../AC/shoppingCart';

class SearchPageProducts extends Component {
  componentDidMount() {
    this.props.getUI();
  }

  render() {
    const { products } = this.props;

    const productList = products.map(product => <Product id={product.id} {...product} />);

    return productList.length ? productList : <Spinner />;
  }
}

export default connect((state) => {
  const { searchPage: products } = state;
  return { products };
}, {
  getUI
})(SearchPageProducts);
