import React from 'react';
import PropTypes from 'prop-types';
/* local components */
import SearchProducts from '../SearchProducts';
import SearchPages from '../SearchPages';

const SearchDropdown = ({ areResultsShown, message, products, pages }) => {
  const searchProducts = products
    ? <SearchProducts products={products}/>
    : null;

  const searchPages = pages
    ? <SearchPages pages={pages}/>
    : null;

  const dropDownContent = message
  ? <p className="search__noresults">{message}</p>
  : <div>
      {searchProducts}
      {searchPages}
    </div>;

  return areResultsShown
  ? <div className="search__dropdown">{dropDownContent}</div>
  : null;
};

SearchDropdown.propTypes = {
  areResultsShown: PropTypes.bool,
  message: PropTypes.string,
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
  }),
  pages: PropTypes.shape({
    url: PropTypes.string,
    items: PropTypes.arrayOf(PropTypes.shape({
      url: PropTypes.string.isRequired,
      title: PropTypes.string.isRequired
    }))
  })
};

export default SearchDropdown;
