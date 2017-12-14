import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';
/* globals */
import { FILTERED_RECENT_ORDERS } from 'app.globals';
/* AC */
import { getCampaigns, getOrders } from 'app.ac/filteredRecentOrders';
/* components */
import Select from 'app.dump/Form/Select';
import Spinner from 'app.dump/Spinner';
import Orders from './Orders';

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

  getCampaignElement = () => {
    const { campaign, orderType } = this.props.filteredRecentOrders;
    if (orderType.isFetching) return <Spinner />;
    if (!campaign.items.length) return null;
    return (
      <Select
        disabled={campaign.isBlocked}
        options={campaign.items}
        onChange={(e) => { this.getOrders(e.target.value); }}
        value={campaign.selected}
        placeholder={campaign.placeholder}
      />
    );
  };

  getOrdersElement = () => {
    const { campaign, orders } = this.props.filteredRecentOrders;
    if (campaign.isFetching) return <Spinner />;
    if (!Object.keys(orders).length) return null;
    return <Orders orders={orders} />;
  };

  render() {
    const { orderType } = this.props.filteredRecentOrders;

    return (
      <div>
        <Select
          disabled={orderType.isBlocked}
          options={FILTERED_RECENT_ORDERS.orderTypes.items}
          onChange={(e) => { this.getCampaigns(e.target.value); }}
          value={orderType.selected}
          placeholder={FILTERED_RECENT_ORDERS.orderTypes.placeholder}
        />
        {this.getCampaignElement()}
        {this.getOrdersElement()}
      </div>
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
