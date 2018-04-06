import React, { Component } from 'react';
import { connect } from 'react-redux';
import PropTypes from 'prop-types';
import Spinner from 'app.dump/Spinner';
import { loadFavoritesProducts } from 'app.ac/products';


class ProductsFavorites extends Component {
  static propTypes = {
    isLoading: PropTypes.bool.isRequired,
    loadFavoritesProducts: PropTypes.func.isRequired,
    products: PropTypes.array.isRequired
  };

  componentDidMount() {
    // load Products
    this.props.loadFavoritesProducts();
  }

  render() {
    const { isLoading, products } = this.props;

    if (isLoading) {
      return (<Spinner />);
    }

    return (
      <div className="row">
        { products.map(product => (
          <div key={product.id} className="col-lg-4 col-xl-3">
            <div className="template">
              <a href={product.url} className="category">
                <div className="category__picture">
                  <img
                    src={product.imageUrl}
                  />
                </div>
                <div className="category__title">
                  <h2>{product.title}</h2>
                </div>
              </a>
            </div>
          </div>
        )) }
      </div>
    );
  }
}


export default connect((state) => {
  const { isLoading, products } = state.products;
  return { isLoading, products };
}, {
  loadFavoritesProducts
})(ProductsFavorites);
