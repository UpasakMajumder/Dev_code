import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';
/* globals */
import { FILTERED_RECENT_ORDERS } from 'app.globals';
/* AC */
import { getCampaigns, getOrders } from 'app.ac/filteredRecentOrders';

class FilteredRecentOrders extends Component {
  getOrders = (selectedCampaign) => {
    const url = FILTERED_RECENT_ORDERS.getOrdersUrl;
    const selectedOrderType = this.props.filteredRecentOrders.orderType.selected;
    this.props.getOrders(url, selectedOrderType, selectedCampaign);
  };

  getCampaigns = (selectedOrderType) => {
    const url = FILTERED_RECENT_ORDERS.getCampaignsUrl;
    this.props.getCampaigns(url, selectedOrderType);
  };

  render() {

    return (

    );
  }
}

FilteredRecentOrders.propTypes = {
  getCampaigns: PropTypes.func.isRequired,
  getOrders: PropTypes.func.isRequired,
  filteredRecentOrders: PropTypes.shape({
    orderType: PropTypes.shape({
      isFetching: PropTypes.bool.isRequired,
      isBlocked: PropTypes.bool.isRequired,
      selected: PropTypes.string
    }).isRequired,
    campaign: PropTypes.shape({
      isFetching: PropTypes.bool.isRequired,
      isBlocked: PropTypes.bool.isRequired,
      selected: PropTypes.string,
      placeholder: PropTypes.string,
      items: PropTypes.array.isRequired
    }).isRequired,
    orders: PropTypes.object.isRequired
  }).isRequired
};

export default connect((state) => {
  const { filteredRecentOrders } = state;
  return { filteredRecentOrders };
}, {
  getCampaigns,
  getOrders
})(FilteredRecentOrders);
