import React, { Component } from 'react';
import SearchProducts from '../SearchProducts';
import SearchPages from '../SearchPages';

class SearchDropdown extends Component {
  render() {
    const { areResultsShown, message, products, pages } = this.props;

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

    const content = areResultsShown
    ? <div className="search__dropdown">{dropDownContent}</div>
    : null;

    return content;
  }
}

export default SearchDropdown;
