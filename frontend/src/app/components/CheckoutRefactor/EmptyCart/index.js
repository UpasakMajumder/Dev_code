import React from 'react';
import PropTypes from 'prop-types';

const EmptyCart = ({
  ui
}) => {
  return (
    <div>
      <div className="alert alert--full alert--info isOpen js-collapse">
        <p className="p-info weight--normal">{ui.text}</p>
      </div>

      <div className="btn-group btn-group--center">
        <a href={ui.dashboardButtonUrl} type="type" className="btn-action btn-action--secondary">{ui.dashboardButtonText}</a>
        <a href={ui.productsButtonUrl} type="type" className="btn-action">{ui.productsButtonText}</a>
      </div>
    </div>
  );
};

EmptyCart.propTypes = {
  ui: PropTypes.shape({
    text: PropTypes.string.isRequired,
    dashboardButtonText: PropTypes.string.isRequired,
    dashboardButtonUrl: PropTypes.string.isRequired,
    productsButtonText: PropTypes.string.isRequired,
    productsButtonUrl: PropTypes.string.isRequired
  }).isRequired
};

export default EmptyCart;
