import React, { Component } from 'react';
import { connect } from 'react-redux';
import PropTypes from 'prop-types';
import SVG from 'app.dump/SVG';
import { markProductFavourite, unmarkProductFavourite, loadProducts } from 'app.ac/products';


class Products extends Component {
  static propTypes = {
    products: PropTypes.array.isRequired,
    categories: PropTypes.array.isRequired,
    isLoading: PropTypes.bool.isRequired,
    markProductFavourite: PropTypes.function.isRequired,
    unmarkProductFavourite: PropTypes.function.isRequired,
    loadProducts: PropTypes.function.isRequired,
  };

  componentDidMount() {
    //TODO
    console.log('I want to load products...');

    // load Products
    this.props.loadProducts();
  }

  render() {
    //TODO
    console.log('rendering products...');
    const { products, categories, isLoading } = this.props;

    return (
      <div className="row">
        {/* TODO add loading spinner */}
        {isLoading && <div>isLoading...</div>}


        { categories.map((category) => (
          <div key={category.id} className="col-lg-4 col-xl-3">
            <a href={category.url} className="category">
              <div
                className="category__picture"
                style={{ backgroundImage: `url(${category.imageUrl})` }}>
              </div>
              <div className="category__title">
                <h2>{category.title}</h2>
              </div>
            </a>
          </div>
        )) }

        { products.map((product) => (
          <div key={product.id} className="col-lg-4 col-xl-3">
            {/*TODO favourite */}
            {/*TODO dataTooltipPlacement vs data-tooltip-placement */}
            <div className="template__favourite js-collapse js-tooltip" dataTooltipPlacement="right" title="Set product as favorite">
              <div className="js-toggle">
                {product.isFavourite ?
                  <SVG name="star--filled" className="template__icon--filled icon-star"/>
                  : <SVG name="star--unfilled" className="template__icon--unfilled icon-star"/>
                }
              </div>
            </div>

            <a href={product.url} className="category">
              <div
                className="category__picture"
                style={{ backgroundImage: `url(${bgImage || product.imageUrl})` }}>
              </div>
              <div className="category__title">
                <h2>{product.title}</h2>
              </div>
            </a>
          </div>
        )) }
      </div>
    );
  }
}

export default connect(({ products }) => {
  //TODO
  con
  //TODO is better ES6 syntax possible?
  //const { products, categories, isLoading } = products;
  const { categories, isLoading } = products;
  return { products.products, categories, isLoading };
}, {
  markProductFavourite,
  unmarkProductFavourite,
  loadProducts
})(Products);
