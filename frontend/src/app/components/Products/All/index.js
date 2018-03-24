import React, { Component } from 'react';
import { connect } from 'react-redux';
import PropTypes from 'prop-types';
import Spinner from 'app.dump/Spinner';
import SVG from 'app.dump/SVG';
import { Tooltip } from 'react-tippy';
import { loadProducts, markProductFavourite, unmarkProductFavourite } from 'app.ac/products';
import { PRODUCTS as PRODUCTS_GLOBAL } from 'app.globals';


class Products extends Component {
  static propTypes = {
    categories: PropTypes.array.isRequired,
    isLoading: PropTypes.bool.isRequired,
    loadProducts: PropTypes.func.isRequired,
    markProductFavourite: PropTypes.func.isRequired,
    products: PropTypes.array.isRequired,
    unmarkProductFavourite: PropTypes.func.isRequired
  };

  componentDidMount() {
    // load Products
    this.props.loadProducts();
  }

  render() {
    const { categories, isLoading, markProductFavourite, products, unmarkProductFavourite } = this.props;

    if (isLoading) {
      return (<Spinner />);
    }

    return (
      <div className="row">
        { categories.map(category => (
          <div key={category.id} className="col-lg-4 col-xl-3">
            <a href={category.url} className="category">
              <div className="category__picture">
                <img
                  src={category.imageUrl}
                  style={{
                    border: category.border.exists ? category.border.value : 'none'
                  }}
                />
              </div>
              <div className="category__title">
                <h2>{category.title}</h2>
              </div>
            </a>
          </div>
        )) }

        { products.map(product => (
          <div key={product.id} className="col-lg-4 col-xl-3">
            <div className="template">
              <div
                className="template__favourite"
                onClick={() => {
                  product.isFavourite ? unmarkProductFavourite(product.id) : markProductFavourite(product.id);
                }}
              >
                {product.isFavourite ?
                  <SVG name="star--filled" className="icon-star" />
                  : <Tooltip
                    title={PRODUCTS_GLOBAL.addToFavorites}
                    position="right"
                    animation="fade"
                    arrow={true}
                    theme="dark"
                  >
                    <SVG name="star--unfilled" className="icon-star" />
                  </Tooltip>
                }
              </div>

              <a href={product.url} className="category">
                <div className="category__picture">
                  <img
                    src={product.imageUrl}
                    style={{
                      border: product.border.exists ? product.border.value : 'none'
                    }}
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
  const { categories, isLoading, products } = state.products;
  return { categories, isLoading, products };
}, {
  loadProducts,
  markProductFavourite,
  unmarkProductFavourite
})(Products);
