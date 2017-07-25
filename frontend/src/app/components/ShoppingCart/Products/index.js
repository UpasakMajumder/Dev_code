import React, { Component } from 'react';
import Product from '../../Product/Checkout';

class Products extends Component {
  render() {
    const { ui, removeProduct, changeProductQuantity } = this.props;

    const { number, items } = ui;

    const products = items.map((item) => {
      return <Product key={item.id} {...item}
                      removeProduct={removeProduct}
                      changeProductQuantity={changeProductQuantity} />;
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
