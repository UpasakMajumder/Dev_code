import React, { Component } from 'react';
import Product from './Product';

class Products extends Component {
  render() {
    const { ui, removeProduct, loadingProducts, changeProductQuantity, loadingQuantities } = this.props;

    const { number, items } = ui;

    const products = items.map((item) => {
      return <Product key={item.id} {...item}
                      removeProduct={removeProduct}
                      loadingProducts={loadingProducts}
                      changeProductQuantity={changeProductQuantity}
                      loadingQuantities={loadingQuantities}/>;
    });

    return (
      <div>
        <p className="text-right">{number}</p>
        {products}
      </div>
    );
  }
}

export default Products;
