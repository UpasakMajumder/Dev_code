import React from 'react';
import PropTypes from 'prop-types';
import moment from 'moment';
/* components */
import Datepicker from 'app.dump/Form/Datepicker';

const DeliveryDate = ({
  ui,
  deliveryDate,
  changeDeliveryDate,
  blurDeliveryDate
}) => {
  if (!ui) return null;
  return (
    <div className="shopping-cart__block">
      <h2>{ui.title}</h2>
      <div className="flex--start--start mb-5">
        <Datepicker
          selected={deliveryDate.value}
          startDate={moment()}
          onChange={date => changeDeliveryDate(date)}
          minDate={moment().add(1, 'days')}
          onBlur={e => blurDeliveryDate(e)}
          error={deliveryDate.error}
        />
      </div>
    </div>
  );
};

DeliveryDate.propTypes = {
  ui: PropTypes.shape({
    title: PropTypes.string.isRequired
  }),
  deliveryDate: PropTypes.shape({
    value: PropTypes.object,
    error: PropTypes.string
  }).isRequired,
  changeDeliveryDate: PropTypes.func.isRequired,
  blurDeliveryDate: PropTypes.func.isRequired
};

export default DeliveryDate;
