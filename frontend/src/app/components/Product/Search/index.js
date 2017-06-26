import React, { Component } from 'react';
import PropTypes from 'prop-types';
import Stock from '../../Stock';

class SearchProduct extends Component {
  render() {
    const {
      image,
      category,
      title,
      stock,
      url
    } = this.props;

    return (
      <a href={url} className="search-result">
        <div className="search-result__img" style={{ backgroundImage: `url(${image})` }}> </div>

        <div className="search-result__content">
          <p className="search-result__category">{category}</p>
          <p className="search-result__title">{title}</p>
            <Stock text={stock.text} type={stock.type}/>
        </div>
      </a>
    );
  }
}

SearchProduct.propTypes = {
  image: PropTypes.string,
  category: PropTypes.string,
  title: PropTypes.string.isRequired,
  url: PropTypes.string.isRequired,
  stock: PropTypes.shape({
    type: PropTypes.string.isRequired,
    text: PropTypes.string.isRequired
  })
};

export default SearchProduct;
