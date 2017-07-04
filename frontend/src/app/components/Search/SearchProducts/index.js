import React from 'react';
import PropTypes from 'prop-types';
/* components */
import Product from 'app.dump/Product/Search';

const SearchProducts = ({ products }) => {
  const { url, items } = products;

  const filteredItems = items.filter((item, index) => index < 3);
  const productsList = filteredItems.map(item => <Product key={item.id} {...item} />);

  return items.length
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
};

SearchProducts.propTypes = {
  products: PropTypes.shape({
    url: PropTypes.string,
    items: PropTypes.arrayOf(PropTypes.shape({
      image: PropTypes.string,
      category: PropTypes.string,
      title: PropTypes.string.isRequired,
      url: PropTypes.string.isRequired,
      stock: PropTypes.shape({
        type: PropTypes.string.isRequired,
        text: PropTypes.string.isRequired
      })
    }))
  })
};

export default SearchProducts;
