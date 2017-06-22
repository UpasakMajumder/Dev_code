import React, { Component } from 'react';
import Product from '../../Products/Search';

class SearchProducts extends Component {
  render() {
    const { products } = this.props;
    const { url, items } = products;

    const filteredItems = items.filter((item, index) => index < 3);
    const productsList = filteredItems.map(item => <Product key={item.id} {...item} />);

    const content = items.length
    ? (
        <div>
          <div className="search__header">
            <h2>Products</h2>
            <a href={url}>Show all products</a>
          </div>

          <div className="search__results">
            {productsList}
          </div>
        </div>
      )
    : null;

    return content;
  }
}

export default SearchProducts;
