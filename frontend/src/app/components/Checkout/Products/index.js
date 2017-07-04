import React from 'react';
import PropTypes from 'prop-types';
/* components */
import Product from 'app.dump/Product/Checkout';

const Products = (props) => {
  const { ui, removeProduct, changeProductQuantity } = props;
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
};

Products.propTypes = {
  ui: PropTypes.shape({
    number: PropTypes.string.isRequired,
    items: PropTypes.arrayOf(PropTypes.object.isRequired).isRequired
  }).isRequired,
  removeProduct: PropTypes.func.isRequired,
  changeProductQuantity: PropTypes.func.isRequired
};

export default Products;
