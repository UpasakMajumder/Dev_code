import React from 'react';
import PropTypes from 'prop-types';
/* components */
import Product from 'app.dump/Product/Checkout';

const Products = ({
  ui,
  items,
  quantityText,
  removeProduct,
  changeProductQuantity
}) => {
  const products = items.map((item) => {
    return (
      <Product
        key={item.id}
        {...item}
        productionTimeLabel={ui.productionTimeLabel}
        shipTimeLabel={ui.shipTimeLabel}
        buttonLabels={ui.buttonLabels}
        changeProductQuantity={changeProductQuantity}
        removeProduct={removeProduct}
      />
    );
  });

  return (
    <div id="products">
      <p className="text-right">{quantityText}</p>
      {products}
    </div>
  );
};

Products.propTypes = {
  ui: PropTypes.shape({
    productionTimeLabel: PropTypes.string.isRequired,
    shipTimeLabel: PropTypes.string.isRequired,
    buttonLabels: PropTypes.shape({
      edit: PropTypes.string.isRequired,
      remove: PropTypes.string.isRequired
    }).isRequired
  }).isRequired,
  quantityText: PropTypes.string.isRequired,
  items: PropTypes.arrayOf(PropTypes.object.isRequired).isRequired
};

export default Products;
